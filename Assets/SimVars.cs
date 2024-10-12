using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimVars : MonoBehaviour {
    public TextAsset csvFile;
    public Slider rowSlider;
    public GameObject speedInputMenu;
    public TMP_InputField speedInputField;
    public TextMeshProUGUI actualSpeedText;

    public TextMeshProUGUI PositionText;
    public TextMeshProUGUI VelocityText;

    public float AutoTimeSpeed = 10f;
    private float interval;
    private float timeSinceLastCall = 0f;

    public static float time = 0;
    public static float[] r = new float[3];
    public static float[] v = new float[3];
    public static float m = 0f;
    public static float wpsa = 0f;
    public static float wpsaRange = 0f;
    public static float ds54 = 0f;
    public static float ds54Range = 0f;
    public static float ds24 = 0f;
    public static float ds24Range = 0f;
    public static float ds34 = 0f;
    public static float ds34Range = 0f;
    public static float[] rMoon = new float[3];
    public static float[] vMoon = new float[3];

    private List<string[]> dataRows = new List<string[]>();
    public int currentRow = 0;

    public static bool TSliderActive = true;

    void Start(){
        string[] data = csvFile.text.Split(new char[] {'\n'});

        for (int i = 1; i < data.Length; i++){
            string[] fields = data[i].Split(new char[] {','});
            dataRows.Add(fields);
        }

        rowSlider.maxValue = dataRows.Count - 1;
        rowSlider.onValueChanged.AddListener(UpdateRow);

        interval = 1f / AutoTimeSpeed;
        speedInputField.text = AutoTimeSpeed.ToString();
        string multiplier = (60 * AutoTimeSpeed).ToString("F2");
        actualSpeedText.text = $"data points per second  -  {multiplier}x actual speed";
    }

    void UpdateRow(float value) {
        currentRow = Mathf.FloorToInt(value);
        time = float.Parse(dataRows[currentRow][0]);
        r[0] = float.Parse(dataRows[currentRow][1]);
        r[1] = float.Parse(dataRows[currentRow][2]);
        r[2] = float.Parse(dataRows[currentRow][3]);
        v[0] = float.Parse(dataRows[currentRow][4]);
        v[1] = float.Parse(dataRows[currentRow][5]);
        v[2] = float.Parse(dataRows[currentRow][6]);
        // m = float.Parse(dataRows[currentRow][7]);
        // wpsa = float.Parse(dataRows[currentRow][8]);
        // wpsaRange = float.Parse(dataRows[currentRow][9]);
        // ds54 = float.Parse(dataRows[currentRow][10]);
        // ds54Range = float.Parse(dataRows[currentRow][11]);
        // ds24 = float.Parse(dataRows[currentRow][12]);
        // ds24Range = float.Parse(dataRows[currentRow][13]);
        // ds34 = float.Parse(dataRows[currentRow][14]);
        // ds34Range = float.Parse(dataRows[currentRow][15]);

        rMoon[0] = float.Parse(dataRows[currentRow][14]);
        rMoon[1] = float.Parse(dataRows[currentRow][15]);
        rMoon[2] = float.Parse(dataRows[currentRow][16]);
        vMoon[0] = float.Parse(dataRows[currentRow][17]);
        vMoon[1] = float.Parse(dataRows[currentRow][18]);
        vMoon[2] = float.Parse(dataRows[currentRow][19]);

        PositionText.text = $"Position: ({r[0]}, {r[1]}, {r[2]})";
        VelocityText.text = $"Velocity: <{v[0]}, {v[1]}, {v[2]}>";
    }

    void Update(){
        if(!TSliderActive){
            timeSinceLastCall += Time.deltaTime;
            if (timeSinceLastCall >= interval){
                currentRow += 1;
                if(currentRow > (dataRows.Count - 1)){
                    currentRow = 0;
                }
                UpdateRow(currentRow);
                timeSinceLastCall = 0f;
            }
        }
    }

    public void TSliderToggle(){
        TSliderActive = !TSliderActive;
        rowSlider.gameObject.SetActive(TSliderActive);
        rowSlider.value = currentRow;
        speedInputMenu.SetActive(!TSliderActive);
    }

    public void ChangeSimSpeed(){
        AutoTimeSpeed = float.Parse(speedInputField.text);
        string multiplier = (60 * AutoTimeSpeed).ToString("F2");
        actualSpeedText.text = $"data points per second  -  {multiplier}x actual speed";
        interval = 1f / AutoTimeSpeed;
    }
}
