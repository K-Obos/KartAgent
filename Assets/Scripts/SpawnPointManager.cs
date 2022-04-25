using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public Transform[] spawnPoints;

    private int index = -1;

    public Vector3 SelectRandomSpawnpoint()
    {
        //int rnd = Random.Range(0, spawnPoints.Length);
        
        index = (index + 1) % spawnPoints.Length;
        return spawnPoints[index].position;
    }
}
