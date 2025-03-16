using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public CanvasScaler scaler;
    public Slider sizeSlider;
    public bool inSettings = false;
    public GameObject settingsMenu;
    public bool inColorKey = false;
    public GameObject colorKeyMenu;

    void Start(){
        sizeSlider.minValue = -3000f;
        sizeSlider.maxValue = -500f;
        sizeSlider.value = -1500f;
    }

    public void SizeChanged() {
        scaler.referenceResolution = new Vector2(1920f, -1f * sizeSlider.value);
    }

    public void SettingToggler() {
        settingsMenu.SetActive(inSettings ? false : true);
        inSettings = !inSettings;
    }
    public void ColorToggler() {
        colorKeyMenu.SetActive(inColorKey ? false : true);
        inColorKey = !inColorKey;
    }
}
