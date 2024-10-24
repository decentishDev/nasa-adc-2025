using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public static float cameraMode = 0f;
    private float numberOfModes = 2f;

    public Transform spaceship;
    public float rotationSpeed = 50f; // Speed of rotation around the spaceship
    public float distance = 75f; // Distance between the camera and the spaceship
    private float currentRotationAngle = 0f; 

    void Start()
    {
        
    }

    void Update(){
        if(cameraMode == 0f){
            transform.position = Vector3.Lerp(transform.position, new Vector3(-218f, -82f, -235f), 0.05f);
            currentRotationAngle = 0f; 
            transform.rotation = Quaternion.Euler(0, 0, 0);


         } else if (cameraMode == 1f) {
         //  transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -1 * (distanceFromSpaceship)) + SimVars.r, 0.05f);

            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput != 0f) {

                currentRotationAngle += horizontalInput * rotationSpeed * Time.deltaTime;
        
            }

            Vector3 direction = new Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;
            transform.position = spaceship.position + direction;

            transform.LookAt(spaceship.position);
        }
    }

    public void SwitchCameraMode() {
        cameraMode += 1f;
        if(cameraMode > numberOfModes - 1f){
            cameraMode = 0f;
        }
    }
}

