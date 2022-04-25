using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAgentSensor : MonoBehaviour
{
    public Transform parentTransform;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0f, parentTransform.eulerAngles.y,0f);
    }
}
