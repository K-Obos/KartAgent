using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class WallCollider : MonoBehaviour
{
    public KartAgent kartAgent;

    void OnTriggerEnter(Collider collider)
    {
        kartAgent.AddCollisionReward(collider.gameObject.tag);
    }
}
