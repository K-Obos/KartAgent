using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーンをロードする場合に必要
 
public class LoadScene : MonoBehaviour
{
    public void AllCircuitStart () 
    {
        SceneManager.LoadScene("Scenes/Raycast/All Circuit");
    }
    
    public void RaycastTrainingCircuitStart (String scene) 
    {
        SceneManager.LoadScene("Scenes/Raycast/Training/Circuit/"+ scene);
    }

    public void AllConerStart () 
    {
        SceneManager.LoadScene("Scenes/Raycast/All Corner");
    }
    
    public void RaycastTrainingConerStart (String scene) 
    {
        SceneManager.LoadScene("Scenes/Raycast/Training/Corner/"+ scene);
    }

    public void Back () 
    {
        SceneManager.LoadScene("Scenes/Raycast/Selector");
    }
}
