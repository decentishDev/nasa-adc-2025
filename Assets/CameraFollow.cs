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
    public float rotationSpeed = 10f;
    public float distance = 75f; 
    public float currentRotationAngle = 0; 

    public float verticalRotation = 180f;

    private Vector3 direction = new Vector3();

    private Vector3 lastMousePosition;
    private bool isDragging = false;
    public float mouseSensitivity = 10f;
    public float minVerticalAngle = -80;
    public float maxVerticalAngle = 80f;

    void Start()
    {
        
    }

    void Update(){

        if(cameraMode == 0f){
            transform.position = Vector3.Lerp(transform.position, new Vector3(-218f, -82f, -235f), SimVars.lerpConstant);

            //currentRotationAngle = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), SimVars.lerpConstant);
            
            direction = new Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;

        }else if (cameraMode == 1f){
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -75f) + SimVars.r, SimVars.lerpConstant);

        } else if (cameraMode == 2f) {
            HandleMouseInput();
            UpdateCameraPosition();

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //Vector3 direction = Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;

            if (horizontalInput != 0f) {
                 currentRotationAngle -= horizontalInput * rotationSpeed * Time.deltaTime;
                 direction = new Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;
                //transform.position = spaceship.position + direction;
                //transform.RotateAround(spaceship.position, Vector3.up, horizontalInput * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, spaceship.position + direction, SimVars.lerpConstant);

            }

            if (verticalInput != 0f) {
                currentRotationAngle -= verticalInput * rotationSpeed * Time.deltaTime;
                direction = new Vector3(0, Mathf.Sin(currentRotationAngle), Mathf.Cos(currentRotationAngle)) * distance;
                transform.position = Vector3.Lerp(transform.position, spaceship.position + direction, SimVars.lerpConstant);

            }

            transform.LookAt(spaceship.position);
        }
    }
    void HandleMouseInput(){
        if(Input.GetMouseButtonDown(0)){
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }else if (Input.GetMouseButtonUp(0)){
            isDragging = false;
        }

        if(isDragging){
            Vector3 delta = Input.mousePosition - lastMousePosition;
            
            currentRotationAngle += delta.x * mouseSensitivity * Time.deltaTime;
            
            verticalRotation -= delta.y * mouseSensitivity * Time.deltaTime;
        //    verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

            lastMousePosition = Input.mousePosition;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0){
            distance = Mathf.Clamp(distance - scroll * 25f, 10f, 200f);
        }
    }

    void UpdateCameraPosition(){
        float horizontalDistance = distance * Mathf.Cos(verticalRotation * Mathf.Deg2Rad);
        float height = distance * Mathf.Sin(verticalRotation * Mathf.Deg2Rad);
        
        Vector3 targetPosition = spaceship.position + new Vector3(
            horizontalDistance * Mathf.Sin(currentRotationAngle * Mathf.Deg2Rad),
            height,
            horizontalDistance * Mathf.Cos(currentRotationAngle * Mathf.Deg2Rad)
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, SimVars.lerpConstant);
        
        transform.LookAt(spaceship.position);

    }

    public void SwitchCameraMode(){
        cameraMode += 1f;
        if(cameraMode > cameraModes.Length - 1f){
            cameraMode = 0f;
        }

        modeText.text = "Camera: " + cameraModes[(int) cameraMode];
    }
}

