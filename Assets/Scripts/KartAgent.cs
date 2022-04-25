using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

using UnityStandardAssets.Vehicles.Car;

public class KartAgent : Agent
{
   [SerializeField] private bool updateReward = true;
   [SerializeField] private bool goalReward = true;
   [SerializeField] private bool timeupReward = true;
   [SerializeField] private bool collisionReward = true;
   [SerializeField] private bool checkpointReward = true;
      
   [SerializeField] private bool trackReset;
   [SerializeField] private int respawnCheckpointIndex = 0;

   public int RespawnCheckpointIndex{ get { return respawnCheckpointIndex; }}
   public SpawnPointManager spawnPointManager{ get; set ; }
   
   private Rigidbody rigidBody;
   private CarController carController;
   private CheckpointManager checkpointManager;

   public float lastReward{ get; private set; }
   public float bestReward{ get; private set; }

   public float lapTimer{ get { return checkpointManager.lapTimer; }}
   public float lastLaptime{ get { return checkpointManager.lastLaptime; }}
   public float bestLapTime{ get { return checkpointManager.bestLapTime; }}

   public int numberOfClear{ get { return checkpointManager.numberOfClear; }}
   public int numberOfTimeup{ get { return checkpointManager.numberOfTimeup; }}

   public float currentSpeed{ get { return carController.CurrentSpeed; }}
   public float topSpeed{ get { return carController.TopSpeed; }}
   public float maxSpeed{ get { return carController.MaxSpeed; } }
   public float motorTorque{ get { return carController.motorTorque; }}
   public float brakeTorque{ get { return carController.brakeTorque; }}
   public float accelInput{ get { return carController.AccelInput; }}
   public float brakeInput{ get { return carController.BrakeInput; }}

   public float fullMotorTorque{ get { return carController.fullMotorTorque; } }
   public float fullBrakeTorque{ get { return carController.fullBrakeTorque; } }

   public bool isFrontTorque { get { return carController.isFrontTorque; } }
   public bool isRearTorque { get { return carController.isRearTorque; } }
   public bool isFourWheelDrive { get { return carController.isFourWheelDrive; } }

   private float targetTime;

   private void Awake()
   {
      rigidBody = GetComponent<Rigidbody>();
      
      carController = GetComponentInChildren<CarController>();
      carController.m_Rigidbody = rigidBody;
      
      checkpointManager = GetComponentInChildren<CheckpointManager>();
      checkpointManager.kartAgent = this;
   }
   
   private void Start()
   {
      lastReward = bestReward = -9.9999f;
      targetTime = checkpointManager.sectionTime * checkpointManager.checkpoints.Count;
      Respawn(); 
   }

   public override void OnEpisodeBegin()
   {
      if (lastReward > bestReward) bestReward = lastReward;

   }

// ##########################
// ######  R e w a r d  #####
// ##########################

   public void AddUpdateReward()
   {
      if (! updateReward) return;

      AddReward(0.0f);
   }

   public void AddGoalReward()
   {
      if (! goalReward) return;

      AddReward(1.0f);
      lastReward = GetCumulativeReward();
      EndEpisode();
      if (trackReset) Respawn();
      targetTime = checkpointManager.bestLapTime;
   }

   public void AddTimeupReward()
   {
      if (! timeupReward) return;

      AddReward(-1.0f);
      lastReward = GetCumulativeReward();
            
      EndEpisode();
      Respawn(); 
   }
      
   public void AddCollisionReward(string tag)
   {
      if (! collisionReward) return;

      if (tag == "Wall") AddReward(0.0f);
      if (tag == "Kart") AddReward(0.0f);   
   }
   
   public void AddCheckpointReward()
   {
      if (! checkpointReward) return;

      AddReward(0.0f);
   }
   
// ##########################
// ######  R e w a r d  #####
// ##########################

   private void Respawn()
   {
      Vector3 pos = spawnPointManager.SelectRandomSpawnpoint();
     
      rigidBody.transform.localEulerAngles = Vector3.zero;
      rigidBody.velocity = Vector3.zero;
      rigidBody.MovePosition(pos);

      checkpointManager.ResetCheckpoints(respawnCheckpointIndex);
   }

   public override void CollectObservations(VectorSensor sensor)
   {
      sensor.AddObservation(currentSpeed/maxSpeed);
      sensor.AddObservation(motorTorque/fullMotorTorque);
      sensor.AddObservation(brakeTorque/fullBrakeTorque);
   }

   public override void OnActionReceived(ActionBuffers actions)
   {
      var Actions = actions.ContinuousActions;
      
      carController.Move(Actions[0], Actions[1], Actions[1], 0f);
   }
   
   public override void Heuristic(in ActionBuffers actionsOut)
   {
      var continousActions = actionsOut.ContinuousActions;
      continousActions.Clear();
      
       continousActions[0] = Input.GetAxis("Horizontal");
       continousActions[1] = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f: 0f;
   }
}
