using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    private Checkpoints checkpoints;
    private SpawnPointManager spawnPointManager;

    private void Awake()
    {
        checkpoints = GetComponentInChildren<Checkpoints>();
        spawnPointManager = GetComponentInChildren<SpawnPointManager>();

        foreach (KartAgent kartAgent in 
                    GetComponentsInChildren<KartAgent>() )
        {
            kartAgent.spawnPointManager = spawnPointManager;
        }
        
        foreach (CheckpointManager checkpointManager in 
                    GetComponentsInChildren<CheckpointManager>() )
        {
            checkpointManager.checkpoints = checkpoints.checkPoints;
        } 
    }
}
