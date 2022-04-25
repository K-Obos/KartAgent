using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

[Serializable]
public struct CameraCheckpoint
{
    public Camera camera;
    public Checkpoint checkpoint;
}

public class AutomaticCameraSystem : MonoBehaviour
{
    public bool autoStart = false;

    public int followObjectIndex = 0;
    public GameObject followObject;
    public CameraCheckpoint[] cameraCheckpoints;
    
    private CheckpointManager kartToFollow{ get; set; } 

    private void Awake()
    {
        if (followObject == null)
            followObject = transform.parent.gameObject.GetComponentInChildren<KartAgent>().transform.gameObject;

        SetCameraTarget(followObject.transform);

        DisnabledAllCamera();
    }

    private void Start()
    {
        if (autoStart) SetTarget(followObject);
    }

    public void Restart()
    {
        SetTarget(followObject);
    }

    public void Stop()
    {
        releaseTarget();
        DisnabledAllCamera();
    }

    public void SetTarget(GameObject targetObject)
    {
        SetCameraTarget(targetObject.transform);

        kartToFollow = targetObject.GetComponentInChildren<CheckpointManager>();
        int j = 1;
        for (int i = 0; i <= kartToFollow.currentCheckpointIndex;  i++)
        {
            if (j >= cameraCheckpoints.Length) break;
            if (kartToFollow.checkpoints[i] == cameraCheckpoints[j].checkpoint) j++;
        }
        setCameraEnabled(--j);
        
        kartToFollow.reachedCheckpoint += OnReachedCheckpoint;
    }

    public void resetTarget(GameObject targetObject)
    {
        followObject = targetObject;
        releaseTarget();
        SetTarget(followObject);
    }

    private void SetCameraTarget(Transform target)
    {
        foreach (LookatTarget lookatTarget in 
                    GetComponentsInChildren<LookatTarget>() )
        {
            lookatTarget.SetTarget(target);
        }

        foreach (TargetFieldOfView targetFieldOfView in 
                    GetComponentsInChildren<TargetFieldOfView>() )
        {
            targetFieldOfView.SetTarget(target);
        }
    }

    private void setCameraEnabled(int index)
    {
        DisnabledAllCamera();
        cameraCheckpoints[index].camera.enabled = true;
    }

    private void DisnabledAllCamera()
    {
        foreach (CameraCheckpoint cameraCheckpoint in cameraCheckpoints)
        {
            cameraCheckpoint.camera.enabled = false;
        }
    }

    private void releaseTarget()
    {
        kartToFollow.reachedCheckpoint -= OnReachedCheckpoint;
    }

    public void OnReachedCheckpoint(Checkpoint checkpoint)
    {
        foreach (CameraCheckpoint cameraCheckpoint in cameraCheckpoints)
        {
            if (cameraCheckpoint.checkpoint == checkpoint)
            {
                DisnabledAllCamera();
                cameraCheckpoint.camera.enabled = true;
            }
        }
    }

    public void SetPosition(int pos)
    {
        foreach (Camera camera in GetComponentsInChildren<Camera>() )
        {
            switch(pos)
            {
                case 0: // Top Left
                    camera.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f);
                    break;
                case 1: // Bottom Left
                    camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f);
                    break;
                case 2: // Top Rigt
                    camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case 3: // Top Right
                    camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f);
                    break;
                default: // Full
                    camera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
                    break;
            }
        }
    }
}
