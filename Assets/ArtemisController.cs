using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisController : MonoBehaviour {

    private Vector3 lastPos = new Vector3(0f, 0f, 0f);
    public float RotationSmoothing = 0f;
    private float ActualRotSmooth = 1f;

    public Transform vArrow;
    public Transform aArrow;
    

    void Start(){
        
    }

    void Update(){
        Vector3 newPos = SimVars.r / 1000f;
        if(SimVars.TSliderActive){
            transform.position = newPos;
        }else{
            if(newPos != lastPos){
                lastPos = newPos;
                transform.position = newPos;
            }else{
                transform.position += (SimVars.v / 1000f) * (60 * SimVars.AutoTimeSpeed * Time.deltaTime);
            }
        }

        // Vector3 velocity = SimVars.v;
        // if(velocity.magnitude > 0.001f){
        //     Vector3 normalizedVelocity = new Vector3(
        //         Mathf.Sign(velocity.x) * Mathf.Abs(velocity.x), 
        //         Mathf.Sign(velocity.y) * Mathf.Abs(velocity.y), 
        //         Mathf.Sign(velocity.z) * Mathf.Abs(velocity.z)
        //     );
        //     Quaternion targetRotation = Quaternion.LookRotation(normalizedVelocity);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ActualRotSmooth);
        // }

        Vector3 velocity = SimVars.v;
        if(velocity.magnitude > 0.001f){
            Vector3 normalizedVelocity = new Vector3(
                Mathf.Sign(velocity.x) * Mathf.Abs(velocity.x), 
                Mathf.Sign(velocity.y) * Mathf.Abs(velocity.y), 
                Mathf.Sign(velocity.z) * Mathf.Abs(velocity.z)
            );
            Quaternion targetRotation = Quaternion.LookRotation(normalizedVelocity);
            vArrow.rotation = targetRotation;
            vArrow.localScale = new Vector3(vArrow.localScale.x, vArrow.localScale.y, (1f - Mathf.Pow(5f, -1f * velocity.magnitude)) * 5f);
        }

        Vector3 lastV = SimVars.lastV;
        Vector3 acceleration = (velocity - lastV) / 60f;
        
        if(acceleration.magnitude > 0.000001f){
            Vector3 normalizedA = new Vector3(
                Mathf.Sign(acceleration.x) * Mathf.Abs(acceleration.x), 
                Mathf.Sign(acceleration.y) * Mathf.Abs(acceleration.y), 
                Mathf.Sign(acceleration.z) * Mathf.Abs(acceleration.z)
            );
            Quaternion targetRotation = Quaternion.LookRotation(normalizedA);
            aArrow.rotation = targetRotation;
            aArrow.localScale = new Vector3(aArrow.localScale.x, aArrow.localScale.y, (1f - Mathf.Pow(20f, -1000f * acceleration.magnitude)) * 3f);
        }
    }
}
