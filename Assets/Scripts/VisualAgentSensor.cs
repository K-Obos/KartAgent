using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualAgentSensor : MonoBehaviour
{
    public Transform parentTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = parentTransform.position;
        transform.eulerAngles = new Vector3(0f, parentTransform.eulerAngles.y,0f);
    }
}
