using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniArtemisController : MonoBehaviour {
    void LateUpdate(){
        transform.position = SimVars.allR[SimVars.currentRow];
    }
}
