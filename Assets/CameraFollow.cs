using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraFollow : MonoBehaviour {

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

    public static float fullSystemZoom = -260f;

    public static bool miniMap = false; 

    void Start(){
        
    }

    void LateUpdate(){
        if(cameraMode == 0f){
            miniMap = false;
            transform.position = Vector3.Lerp(transform.position, new Vector3(-215f, -80f, fullSystemZoom), SimVars.lerpConstant);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), SimVars.lerpConstant);
            direction = new Vector3(Mathf.Sin(currentRotationAngle), 0, Mathf.Cos(currentRotationAngle)) * distance;

        } else if (cameraMode == 1f) {
            miniMap = true; 
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -75f) + SimVars.r, SimVars.lerpConstant);

        } else if (cameraMode == 2f) {
            miniMap = true; 
            HandleMouseInput();
            UpdateCameraPosition();
        }
    }
    void HandleMouseInput(){
        if (!SliderScript.sliderMoving) {
            if(Input.GetMouseButtonDown(0)){
                isDragging = true;
                lastMousePosition = Input.mousePosition;
            } else if (Input.GetMouseButtonUp(0)){
                isDragging = false;
            }
        } else {
            isDragging = false; 
        }

        if(isDragging){
            Vector3 delta = Input.mousePosition - lastMousePosition;
            
            currentRotationAngle += delta.x * mouseSensitivity * Time.deltaTime;
            
            verticalRotation -= delta.y * mouseSensitivity * Time.deltaTime;

            lastMousePosition = Input.mousePosition;
        }

        #if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 2) {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            distance = Mathf.Clamp(distance + deltaMagnitudeDiff * 0.3f, 10f, 200f);
        }
        #else
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            distance = Mathf.Clamp(distance - scroll * 25f, 10f, 200f);
        }
        #endif
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
        
        if(!SliderScript.sliderMoving){
            transform.LookAt(spaceship.position);
        }
    }

    public void SwitchCameraMode(){
        cameraMode += 1f;
        if(cameraMode > cameraModes.Length - 1f){
            cameraMode = 0f;
        }

        modeText.text = cameraModes[(int) cameraMode];
    }
}