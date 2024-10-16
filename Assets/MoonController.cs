using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : MonoBehaviour {
    private Vector3 lastPos = new Vector3(0f, 0f, 0f);
    void Start(){
        
    }

    void Update(){
        Vector3 newPos = SimVars.rMoon / 1000f;
        if(SimVars.TSliderActive){
            transform.position = newPos;
        }else{
            if(newPos != lastPos){
                lastPos = newPos;
                transform.position = newPos;
            }else{
                transform.position += (SimVars.vMoon / 1000f) * (60 * SimVars.AutoTimeSpeed * Time.deltaTime);
            }
        }
    }
}