using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public static float cameraMode = 0f;
    private float numberOfModes = 2f;
    void Start()
    {
        
    }

    void Update(){
        if(cameraMode == 0f){
            transform.position = Vector3.Lerp(transform.position, new Vector3(-218f, -82f, -235f), 0.05f);
        }else if(cameraMode == 1f){
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -75f) + SimVars.r, 0.05f);
        }
    }

    public void SwitchCameraMode(){
        cameraMode += 1f;
        if(cameraMode > numberOfModes - 1f){
            cameraMode = 0f;
        }
    }
}
