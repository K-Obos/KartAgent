using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private float maxTimeToReachNextCheckpoint = 15f;
    [SerializeField] private float timeLeft = 30f;
    [SerializeField] public float sectionTime = 10f;
 
    [SerializeField] public Checkpoint nextCheckPointToReach;
    public int currentCheckpointIndex{ get; private set; }

    public KartAgent kartAgent{ get; set; }
    public List<Checkpoint> checkpoints{ get; set; }

    public float lapTimer{ get; private set; }
    public float lastLaptime{ get; private set; }
    public float bestLapTime{ get; private set; }

    public int numberOfClear{ get; private set; }
    public int numberOfTimeup{ get; private set; }

    public event Action<Checkpoint> reachedCheckpoint; 

    void Start()
    {
        ResetCheckpoints(0);
        lapTimer = lastLaptime = bestLapTime = 0f;
        numberOfClear = numberOfTimeup = 0;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        lapTimer += Time.deltaTime;

        if (timeLeft < 0f)
        {     
            kartAgent.AddTimeupReward();
            lapTimer = 0f;
            reachedCheckpoint?.Invoke(checkpoints[checkpoints.Count-1]);
            numberOfTimeup++;
        }
        else
        {
            kartAgent.AddUpdateReward();
        }
    }

    public void ResetCheckpoints(int index)
    {
        currentCheckpointIndex = index;
        timeLeft = maxTimeToReachNextCheckpoint;
       
        SetNextCheckpoint();
    }

    public void CheckPointReached(Checkpoint checkpoint)
    {
        if (nextCheckPointToReach != checkpoint) return;
        
        reachedCheckpoint?.Invoke(checkpoint);
        currentCheckpointIndex++;

        if (currentCheckpointIndex >= checkpoints.Count) // Goal
        {
            lastLaptime = lapTimer;
            if (bestLapTime > lastLaptime || bestLapTime == 0) {
                bestLapTime = lastLaptime;
            }
            lapTimer = 0f;
            kartAgent.AddGoalReward();
            ResetCheckpoints(0);
            numberOfClear++;
        }
        else
        {
            kartAgent.AddCheckpointReward();
            SetNextCheckpoint();
        }
    }

    private void SetNextCheckpoint()
    {
        if (checkpoints.Count > 0)
        {
            timeLeft = maxTimeToReachNextCheckpoint;
            nextCheckPointToReach = checkpoints[currentCheckpointIndex];
        }
    }
}
