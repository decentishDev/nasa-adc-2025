using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingCameraController : MonoBehaviour
{
    public Slider slider;
    public float time = 0f;
    public Animation anim;
    public GameObject button;
    public LineRenderer[] lines;
    public float lineWidth = 0.5f;
    public Transform[] otherObjects;


    public void Update(){
        slider.value = time;
        for(int i = 0; i < lines.Length; i++){
            //lines[i].Parameters.widthMultiplier = lineWidth;
            lines[i].widthMultiplier = lineWidth;
        }
        for(int i = 0; i < otherObjects.Length; i++){
            otherObjects[i].localScale = new Vector3(lineWidth * 0.21336f, lineWidth * 0.21336f, lineWidth * 0.21336f);
        }
    }

    public void StartRecording(){
        anim.Play("RecordingAnimation1");
        button.SetActive(false);
    }
}
