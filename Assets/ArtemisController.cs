using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisController : MonoBehaviour
{
    void Start(){
        
    }

    void Update(){
        transform.position = Vector3.Lerp(transform.position, new Vector3(SimVars.R[0] / 1000f, SimVars.R[1] / 1000f, SimVars.R[2] / 1000f), 0.5f);
    }
}
