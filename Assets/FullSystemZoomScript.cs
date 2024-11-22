using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullSystemZoomScript : MonoBehaviour{

    public Slider sizeSlider;

    void Start(){
        sizeSlider.minValue = -350f;
        sizeSlider.maxValue = -230f;
        sizeSlider.value = -260f;
    }

    public void SizeChanged(){
        CameraFollow.fullSystemZoom = sizeSlider.value; 
    }

    void Update() {
        CameraFollow.fullSystemZoom = sizeSlider.value; 
    }
}
