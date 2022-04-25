using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class MultiTarget : MonoBehaviour
{
    [SerializeField] public GameObject circuit;

    [SerializeField] private AgentSelector agentSelector;
    [SerializeField] private List<MonitorBoard> monitorBoard; 

    private AutomaticCameraSystem baseAutomaticCameraSystem;
    private GameObject[] cameraSystems = new GameObject[4];
    
    private Toggle multiToggle;
    
    private void Awake()
    {
        multiToggle = GetComponent<Toggle>();
    }

    void Start()
    {
        cameraSystems[0] = circuit.transform.Find("Camera System").gameObject;
        baseAutomaticCameraSystem = cameraSystems[0].GetComponent<AutomaticCameraSystem>();
        baseAutomaticCameraSystem.Restart();
    }

    public void ResetCircuit(GameObject newCircuit)
    {        
        if (multiToggle.isOn) SingleMode();
        baseAutomaticCameraSystem.Stop();
        cameraSystems[0] = newCircuit.transform.Find("Camera System").gameObject;
        baseAutomaticCameraSystem = cameraSystems[0].GetComponent<AutomaticCameraSystem>();
        baseAutomaticCameraSystem.followObject = monitorBoard[agentSelector.index].monitoringObject;
        baseAutomaticCameraSystem.Restart();
        if (multiToggle.isOn) MultiMode();
    }

    public void ChangeMode()
    {
        if (multiToggle.isOn)
            MultiMode();
        else
            SingleMode(); 
    }

    private void SingleMode()
    {
        for (int i = 1; i < 4; ++i)
            if (cameraSystems[i])
            {
                cameraSystems[i].GetComponent<AutomaticCameraSystem>().Stop();
                Destroy(cameraSystems[i]);
            }
        
        baseAutomaticCameraSystem.SetPosition(-1);
        baseAutomaticCameraSystem.resetTarget(monitorBoard[agentSelector.index].monitoringObject);        
    }

    private void MultiMode()
    { 
        int count = baseAutomaticCameraSystem.transform.childCount;
   
        for (int i = 1; i < 4; ++i)
        {
            cameraSystems[i] = Instantiate(cameraSystems[0]);
            AutomaticCameraSystem automaticCameraSystem = cameraSystems[i].GetComponent<AutomaticCameraSystem>();
            automaticCameraSystem.followObject = monitorBoard[i].monitoringObject;
            automaticCameraSystem.Restart();
            for (int j=0; j < count; j++)
            {
                LookatTarget lookatTarget = baseAutomaticCameraSystem.transform.GetChild(j).gameObject.GetComponent<LookatTarget>();
                if ((lookatTarget) && lookatTarget.enabled) 
                    automaticCameraSystem.transform.GetChild(j).localRotation = lookatTarget.m_OriginalRotation;
            }

            cameraSystems[i].transform.position = cameraSystems[0].transform.position;
            cameraSystems[i].name = "Camera System " + i;
            automaticCameraSystem.SetPosition(i);
        }

        baseAutomaticCameraSystem.SetPosition(0);
        baseAutomaticCameraSystem.resetTarget(monitorBoard[0].monitoringObject);
    }
}

