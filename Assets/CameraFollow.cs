using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraFollow : MonoBehaviour
{

    public static float cameraMode = 0f;
    public TextMeshProUGUI modeText;
    private string[] cameraModes = {"Full system", "Zoomed tracking", "Free tracking"};

    public Transform spaceship;
    public float rotationSpeed = 10f; // Speed of rotation around the spaceship
    public float distance = 75f; // Distance between the camera and the spaceship
    private float currentRotationAngle = 0f; 

    void Start()
    {
        
    }

    void Update(){
        if(cameraMode == 0f){
            transform.position = Vector3.Lerp(transform.position, new Vector3(-218f, -82f, -235f), 0.05f);
            currentRotationAngle = 0f;
            // transform.rotation = Quaternion.Euler(0, 0, 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.05f);


         } else if (cameraMode == 1f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -75f) + SimVars.r, 0.05f);
        } else if (cameraMode == 2f) {
         //  transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -1 * (distanceFromSpaceship)) + SimVars.r, 0.05f);

            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput != 0f) {

                currentRotationAngle += horizontalInput * rotationSpeed * Time.deltaTime;
        
            }

            Vector3 direction = new Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;
            //transform.position = spaceship.position + direction;
            transform.position = Vector3.Lerp(transform.position, spaceship.position + direction, 0.05f);

            transform.LookAt(spaceship.position);
        }
    }

    public void SwitchCameraMode() {
        cameraMode += 1f;
        if(cameraMode > cameraModes.Length - 1f){
            cameraMode = 0f;
        }

        modeText.text = "Camera: " + cameraModes[(int) cameraMode];
    }
}

