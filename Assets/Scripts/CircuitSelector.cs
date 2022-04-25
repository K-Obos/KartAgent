using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircuitSelector : MonoBehaviour
{
    [SerializeField] private List<GameObject> circuits;
    [SerializeField] private AgentSelector agentSelector;
    [SerializeField] private MultiTarget multiTarget;
    [SerializeField] private GameObject Monitors;
    
    private GameObject karts;
    private GameObject cameraSystem;
    
    private Dropdown circuitDropdown;
    public int index{ get {return circuitDropdown.value;} private set {circuitDropdown.value = value;} }   
    
    private void Awake()
    {
        karts = circuits[0].transform.Find("Karts").gameObject;
        int count = karts.transform.childCount;
        int i = 0;
        foreach (MonitorBoard monitorBoard in 
                      Monitors.GetComponentsInChildren<MonitorBoard>() )
        {
             monitorBoard.monitoringObject = karts.transform.GetChild(i++).gameObject;
        }

        agentSelector.circuit = circuits[0];
        multiTarget.circuit = circuits[0];
        circuitDropdown = GetComponentInChildren<Dropdown>();
    }

    void Start()
    {
        circuitDropdown.ClearOptions();
        foreach (GameObject circuit in circuits)
            circuitDropdown.options.Add(new Dropdown.OptionData { text = circuit.name });
        circuitDropdown.RefreshShownValue();        
    }

    public void ChangeCircuit()
    {
        karts = circuits[index].transform.Find("Karts").gameObject;
        int count = karts.transform.childCount;
        int i = 0;
        foreach (MonitorBoard monitorBoard in 
                      Monitors.GetComponentsInChildren<MonitorBoard>() )
        {
            if (i<count) {
                monitorBoard.enabled = true;
                monitorBoard.monitoringObject = karts.transform.GetChild(i++).gameObject;
                monitorBoard.Restart();
            }
            else
            {
                monitorBoard.enabled = false;
            }
        }
        multiTarget.ResetCircuit(circuits[index]);
        agentSelector.ResetCircuit(circuits[index]);
    }
}
