using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisController : MonoBehaviour {

    public float PositionSmoothing = 0f;
    private float ActualPosSmooth = 1f;
    public float RotationSmoothing = 0f;
    private float ActualRotSmooth = 1f;

    void Start(){
        
    }

    void Update(){
        if(PositionSmoothing <= 0f){
            PositionSmoothing = 0f;
            ActualPosSmooth = 1f;
        }else{
            ActualPosSmooth = (20f / PositionSmoothing) * Time.deltaTime;
        }

        if(RotationSmoothing <= 0f){
            RotationSmoothing = 0f;
            ActualRotSmooth = 1f;
        }else{
            ActualRotSmooth = (20f / RotationSmoothing) * Time.deltaTime;
        }
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(SimVars.r[0] / 1000f, SimVars.r[1] / 1000f, SimVars.r[2] / 1000f), ActualPosSmooth);
        
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
