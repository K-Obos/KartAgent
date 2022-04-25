using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class AgentSelector : MonoBehaviour 
{
    [SerializeField] public GameObject circuit;

    private List<KartAgent> kartAgents = new List<KartAgent>();
    private List<CheckpointManager> checkpointManagers = new List<CheckpointManager>();
    private AutomaticCameraSystem automaticCameraSystem; 

    private Dropdown agentDropdown;
    public int index{ get {return agentDropdown.value;} private set {agentDropdown.value = value;} }    

    private void Awake()
    {
        agentDropdown = GetComponent<Dropdown>();
    }

    void Start()
    {
        circuit.GetComponentsInChildren<KartAgent>(kartAgents);
        circuit.GetComponentsInChildren<CheckpointManager>(checkpointManagers);
        automaticCameraSystem = circuit.GetComponentInChildren<AutomaticCameraSystem>();

        int i = 0;
        agentDropdown.ClearOptions();
        foreach (KartAgent kartAgent in kartAgents)
        {
            agentDropdown.options.Add(new Dropdown.OptionData { text = kartAgent.name });
            if (++i >= 4) break;
        }
        if (automaticCameraSystem.followObject)
            index = automaticCameraSystem.followObjectIndex;
        else
            index = 0;
        agentDropdown.RefreshShownValue();
    }

    public void ResetCircuit(GameObject newCircuit)
    {
        circuit = newCircuit;
        Start();
    }

    public void ChangeAgent()
    {
        automaticCameraSystem.resetTarget(kartAgents[index].transform.gameObject);
    }
}
