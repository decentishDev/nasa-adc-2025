using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour {
    public Transform EarthModel;
    private Vector3 idealScale = new Vector3(0.02477213f, 0.02477213f, 0.02477213f);

    void Start(){
        
    }

    void Update(){
        if(SimVars.enlargedProportions){
            idealScale = new Vector3(0.1f, 0.1f, 0.1f);
        }else{
            idealScale = new Vector3(0.02477213f, 0.02477213f, 0.02477213f);
        }

        EarthModel.localScale = Vector3.Lerp(EarthModel.localScale, idealScale, 0.2f);
    }
}
