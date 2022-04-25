using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public List<Checkpoint> checkPoints;
    
    private void Awake()
    {
        int index = 0;
        foreach (Checkpoint checkpoint in checkPoints)
        {
            checkpoint.name = "Checkpoint " + index.ToString();
            index++;
        }
    }
}
