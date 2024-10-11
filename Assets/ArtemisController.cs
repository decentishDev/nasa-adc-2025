using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisController : MonoBehaviour {

    public float PositionSmoothingSpeed = 5f;
    public float RotationSmoothingSpeed = 5f;

    void Start(){
        
    }

    void Update(){
        transform.position = Vector3.Lerp(transform.position, new Vector3(SimVars.r[0] / 1000f, SimVars.r[1] / 1000f, SimVars.r[2] / 1000f), PositionSmoothingSpeed * Time.deltaTime);
        
        ///  I think this is incorrect but idk how
        Vector3 velocity = new Vector3(SimVars.r[0], SimVars.r[1], SimVars.r[2]);
        if(velocity.magnitude > 0.001f){
            Vector3 normalizedVelocity = new Vector3(
                Mathf.Sign(velocity.x) * Mathf.Abs(velocity.x), 
                Mathf.Sign(velocity.y) * Mathf.Abs(velocity.y), 
                Mathf.Sign(velocity.z) * Mathf.Abs(velocity.z)
            );
            Quaternion targetRotation = Quaternion.LookRotation(normalizedVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSmoothingSpeed * Time.deltaTime);
        }

    }
}
