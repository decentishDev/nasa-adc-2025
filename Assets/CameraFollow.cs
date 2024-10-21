using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SimVars.cameraZoomer) {
            if (SimVars.enlargedProportions) {
                transform.position = SimVars.r + new Vector3(0,0,-40);
            } else {
                transform.position = SimVars.r + new Vector3(0,0,-1f);

            }

        } else {
            transform.position = new Vector3(-218, -82, -235);
        }
    }
}
