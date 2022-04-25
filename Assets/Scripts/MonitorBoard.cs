using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityStandardAssets.Vehicles.Car;

public class MonitorBoard : MonoBehaviour
{
    public GameObject monitoringObject;

    public Text nameBoard;
    public Text speedBoard;
    public Text topSpeedBoard;
    public Text morterTorqueBoard;
    public Text brakeTorqueBoard;

    public Text laptimeBoard;
    public Text lastLaptimeBoard;
    public Text bestLaptimeBoard;
    public Text rewardBoard;
    public Text lastRewardBoard;
    public Text bestRewardBoard;

    public Text goalBoard;
    public Text retireBoard;

	[SerializeField]
	public Slider morterTorqueSlider;
    [SerializeField]
	public Slider brakeTorqueSlider;
    [SerializeField]
	public Slider SpeedSlider;

    private KartAgent kartAgent;

    // Start is called before the first frame update
    void Start()
    {
        nameBoard.text = monitoringObject.name;
        kartAgent = monitoringObject.GetComponentInChildren<KartAgent>();
        morterTorqueSlider.maxValue = kartAgent.fullMotorTorque;
        brakeTorqueSlider.maxValue = kartAgent.fullBrakeTorque;
        SpeedSlider.maxValue = kartAgent.maxSpeed;
    }

    public void Restart(){  Start();  }

    // Update is called once per frame
    void Update()
    {
        if (kartAgent==null) return;

        speedBoard.text = kartAgent.currentSpeed.ToString("F1");
        topSpeedBoard.text = kartAgent.topSpeed.ToString("F1");

        laptimeBoard.text = kartAgent.lapTimer.ToString("F1");
        lastLaptimeBoard.text = kartAgent.lastLaptime.ToString("F1");
        bestLaptimeBoard.text = kartAgent.bestLapTime.ToString("F1");

        goalBoard.text = kartAgent.numberOfClear.ToString();
        retireBoard.text = kartAgent.numberOfTimeup.ToString();

        morterTorqueBoard.text = kartAgent.motorTorque.ToString("F0");
        brakeTorqueBoard.text = kartAgent.brakeTorque.ToString("F0");
        morterTorqueSlider.value = kartAgent.motorTorque;
        brakeTorqueSlider.value = kartAgent.brakeTorque;
        SpeedSlider.value = kartAgent.currentSpeed;
        
        rewardBoard.text = kartAgent.GetCumulativeReward().ToString("F5");
        lastRewardBoard.text = kartAgent.lastReward.ToString("F5");
        bestRewardBoard.text = kartAgent.bestReward.ToString("F5");
    }
}
