using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour {
    public Transform EarthModel;
    private Vector3 idealScale = new Vector3(0.02477213f, 0.02477213f, 0.02477213f);
    public Renderer EarthModelRend;
    public float magn = 0f;

    void Start(){
        
    }

    void Update(){
        if(SimVars.enlargedProportions){
            idealScale = new Vector3(0.1f, 0.1f, 0.1f);
        }else{
            idealScale = new Vector3(0.02477213f, 0.02477213f, 0.02477213f);
        }

        EarthModel.localScale = Vector3.Lerp(EarthModel.localScale, idealScale, 0.2f);
        magn = SimVars.r.magnitude;
        if(SimVars.r.magnitude < 20f){
            EarthModelRend.material.color = Color.Lerp(EarthModelRend.material.color, new Color(1f, 1f, 1f, 0.6f), 0.05f);
        }else{
            EarthModelRend.material.color = Color.Lerp(EarthModelRend.material.color, new Color(1f, 1f, 1f, 1f), 0.05f);
        }
    }
}
