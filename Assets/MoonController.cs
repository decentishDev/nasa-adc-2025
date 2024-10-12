using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : MonoBehaviour {
    public float PositionSmoothing = 0f;
    private float ActualPosSmooth = 1f;
    void Start(){
        
    }

    void Update(){
        if(PositionSmoothing <= 0f){
            PositionSmoothing = 0f;
            ActualPosSmooth = 1f;
        }else{
            ActualPosSmooth = (20f / PositionSmoothing) * Time.deltaTime;
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(SimVars.rMoon[0] / 1000f, SimVars.rMoon[1] / 1000f, SimVars.rMoon[2] / 1000f), ActualPosSmooth);
    }
}
