using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour {
    public Transform EarthModel;
    private Vector3 idealScale = new Vector3(0.02477213f, 0.02477213f, 0.02477213f);
    public Renderer EarthModelRend;
    public float magn = 0f;
    public Material EarthMat;
    public Material AtmoMat;
    public Transform DirectionalLight;
    private float opacity = 0.6f;

    void Start(){
        
    }

    void Update(){
        if(SimVars.enlargedProportions){
            idealScale = new Vector3(2.5f, 2.5f, 2.5f);
        }else{
            idealScale = new Vector3(1f, 1f, 1f);
        }

        EarthModel.localScale = Vector3.Lerp(EarthModel.localScale, idealScale, 0.2f);
        AtmoMat.SetFloat("_PlanetRadius", 6.31f * EarthModel.localScale.x);
        AtmoMat.SetFloat("_AtmosphereRadius", 6.8f * EarthModel.localScale.x);
        magn = SimVars.r.magnitude;
        if(SimVars.r.magnitude < 20f){
            //EarthModelRend.material.color = Color.Lerp(EarthModelRend.material.color, new Color(1f, 1f, 1f, 0.6f), 0.05f);
            opacity = Mathf.Lerp(opacity, 0.6f, 0.05f);
        }else{
            //EarthModelRend.material.color = Color.Lerp(EarthModelRend.material.color, new Color(1f, 1f, 1f, 1f), 0.05f);
            opacity = Mathf.Lerp(opacity, 1f, 0.05f);
        }

        EarthMat.SetFloat("_Opacity", opacity);
        Vector3 lightDir = -DirectionalLight.forward;
        EarthMat.SetVector("_LightDirection", new Vector4(lightDir.x, lightDir.y, lightDir.z, 0));
        AtmoMat.SetVector("_LightDirection", new Vector4(lightDir.x, lightDir.y, lightDir.z, 0));
    }
}
