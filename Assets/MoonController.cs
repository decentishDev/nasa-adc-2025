using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : MonoBehaviour {

    private Vector3 lastPos = new Vector3(0f, 0f, 0f);

    public Transform MoonModel;
    private Vector3 idealScale = new Vector3(1f, 1f, 1f);

    void Start(){
        
    }

    void Update(){
        Vector3 newPos = SimVars.rMoon;
        if(SimVars.TSliderActive){
            transform.position = newPos;
        }else{
            if(newPos != lastPos){
                lastPos = newPos;
                transform.position = newPos;
            }else{
                if(SimVars.isSimulation){
                    transform.position += SimVars.vMoon * Time.deltaTime;
                }else{
                    transform.position += SimVars.vMoon * (SimVars.AutoTimeSpeed * Time.deltaTime);
                }
            }
        }

        if(SimVars.enlargedProportions){
            idealScale = new Vector3(5f, 5f, 5f);
        }else{
            idealScale = new Vector3(1f, 1f, 1f);
        }

        MoonModel.localScale = Vector3.Lerp(MoonModel.localScale, idealScale, 0.2f);
    }
}