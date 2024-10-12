using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisController : MonoBehaviour {

    private Vector3 lastPos = new Vector3(0f, 0f, 0f);
    public float RotationSmoothing = 0f;
    private float ActualRotSmooth = 1f;
    

    void Start(){
        
    }

    void Update(){

        Vector3 newPos = new Vector3(SimVars.r[0] / 1000f, SimVars.r[1] / 1000f, SimVars.r[2] / 1000f);
        if(SimVars.TSliderActive){
            transform.position = newPos;
        }else{
            if(newPos != lastPos){
                lastPos = newPos;
                transform.position = newPos;
            }else{
                transform.position += ( new Vector3(SimVars.v[0] / 1000f, SimVars.v[1] / 1000f, SimVars.v[2] / 1000f)) * (60 * SimVars.AutoTimeSpeed * Time.deltaTime);
            }
        }

        Vector3 velocity = new Vector3(SimVars.v[0], SimVars.v[1], SimVars.v[2]);
        if(velocity.magnitude > 0.001f){
            Vector3 normalizedVelocity = new Vector3(
                Mathf.Sign(velocity.x) * Mathf.Abs(velocity.x), 
                Mathf.Sign(velocity.y) * Mathf.Abs(velocity.y), 
                Mathf.Sign(velocity.z) * Mathf.Abs(velocity.z)
            );
            Quaternion targetRotation = Quaternion.LookRotation(normalizedVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ActualRotSmooth);
        }
    }
}
