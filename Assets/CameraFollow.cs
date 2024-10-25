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
    private float currentRotationAngle = 0; 

    private float verticalRotation = 0f;

    private Vector3 direction = new Vector3();

    private Vector3 lastMousePosition;
    private bool isDragging = false;
    public float mouseSensitivity = 10f;
    public float minVerticalAngle = -360;
    public float maxVerticalAngle = 360f;
    void Start()
    {
        
    }

    void Update(){
        if(cameraMode == 0f){
            transform.position = Vector3.Lerp(transform.position, new Vector3(-218f, -82f, -235f), 0.05f);
            currentRotationAngle = 0f;
            // transform.rotation = Quaternion.Euler(0, 0, 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.05f);
            
            direction = new Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;

         } else if (cameraMode == 1f) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -75f) + SimVars.r, 0.05f);
        } else if (cameraMode == 2f) {
            HandleMouseInput();
            UpdateCameraPosition();
        }
        // transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -1 * (distanceFromSpaceship)) + SimVars.r, 0.05f);

            float horizontalInput = Input.GetAxis("Horizontal");

            float verticalInput = Input.GetAxis("Vertical");

            //Vector3 direction = Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;


            if (horizontalInput != 0f) {

                currentRotationAngle -= horizontalInput * rotationSpeed * Time.deltaTime;

                direction = new Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;
            //transform.position = spaceship.position + direction;

            }

            if (verticalInput != 0f) {

                currentRotationAngle -= verticalInput * rotationSpeed * Time.deltaTime;
        
                direction = new Vector3(0, Mathf.Sin(currentRotationAngle), Mathf.Cos(currentRotationAngle)) * distance;

            }
            transform.position = Vector3.Lerp(transform.position, spaceship.position + direction, 0.05f);

            transform.LookAt(spaceship.position);

    }
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            
            currentRotationAngle += delta.x * mouseSensitivity * Time.deltaTime;
            
            verticalRotation -= delta.y * mouseSensitivity * Time.deltaTime;
        //    verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

            lastMousePosition = Input.mousePosition;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distance = Mathf.Clamp(distance - scroll * 25f, 10f, 200f);
        }
    }

    void UpdateCameraPosition()
    {
        float horizontalDistance = distance * Mathf.Cos(verticalRotation * Mathf.Deg2Rad);
        float height = distance * Mathf.Sin(verticalRotation * Mathf.Deg2Rad);
        
        Vector3 targetPosition = spaceship.position + new Vector3(
            horizontalDistance * Mathf.Sin(currentRotationAngle * Mathf.Deg2Rad),
            height,
            horizontalDistance * Mathf.Cos(currentRotationAngle * Mathf.Deg2Rad)
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);
        
        transform.LookAt(spaceship.position);
    }

    public void SwitchCameraMode() {
        cameraMode += 1f;
        if(cameraMode > cameraModes.Length - 1f){
            cameraMode = 0f;
        }

        modeText.text = "Camera: " + cameraModes[(int) cameraMode];
    }
}

