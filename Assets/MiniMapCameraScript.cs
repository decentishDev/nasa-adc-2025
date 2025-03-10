using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraScript : MonoBehaviour
{
    public Transform spaceship;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
  
        if (CameraFollow.miniMap == false) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -75f) + SimVars.r, SimVars.lerpConstant);
            transform.LookAt(spaceship.position);


        } else {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-215f, -80f, CameraFollow.fullSystemZoom - 100), SimVars.lerpConstant);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), SimVars.lerpConstant);  

         
        }
    }
}
