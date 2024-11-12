using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {
    public bool realisticLights = true;
    void Start(){
        
    }

    void Update(){
        if(realisticLights){
            float rotationAngle = ((float) SimVars.time / 86400f) * 360f;
            transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }else{
            transform.rotation = Quaternion.Euler(0, 30, 0);
        }
    }

    public void changeLights(){
        realisticLights = !realisticLights;
    }
}
