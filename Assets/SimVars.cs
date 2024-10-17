using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimVars : MonoBehaviour {
    public TextAsset csvFile;
    public TextAsset csvFileExtra;
    public Slider rowSlider;
    public GameObject speedInputMenu;
    public TMP_InputField speedInputField;
    public TextMeshProUGUI actualSpeedText;

    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI PositionText;
    public TextMeshProUGUI VelocityText;
    public LineRenderer trailRenderer;
    public LineRenderer moonTrailRenderer;

    public static float AutoTimeSpeed = 10f;
    private float totalElapsedTime = 0f;

    public static float time = 0;
    public static Vector3 r;
    public static Vector3 v;
    public static Vector3 lastV;
    public static float m = 0f;

    public static float wpsa = 0f;
    public TextMeshProUGUI wpsaText;
    public static float ds54 = 0f;
    public TextMeshProUGUI ds54Text;
    public static float ds24 = 0f;
    public TextMeshProUGUI ds24Text;
    public static float ds34 = 0f;
    public TextMeshProUGUI ds34Text;

    public static Vector3 rMoon;
    public static Vector3 vMoon;
    public bool RenderingTrail = true;

    private List<string[]> dataRows = new List<string[]>();
    private List<string[]> dataRowsExtra = new List<string[]>();
    private Vector3[] allR = new Vector3[12981];
    private Vector3[] allMoonR = new Vector3[12981];
    public int currentRow = 0;

    public static bool TSliderActive = true;
    public static bool enlargedProportions = false;

    void Start(){
        string[] data = csvFile.text.Split(new char[] {'\n'});
        string[] dataExtra = csvFileExtra.text.Split(new char[] {'\n'});

        for (int i = 1; i < data.Length; i++){
            string[] fields = data[i].Split(new char[] {','});
            string[] fieldsExtra = dataExtra[i].Split(new char[] {','});
            dataRows.Add(fields);
            dataRowsExtra.Add(fieldsExtra);
            Vector3 rPos = new Vector3(float.Parse(fieldsExtra[1]), float.Parse(fieldsExtra[2]), float.Parse(fieldsExtra[3]));
            Vector3 moonPos = new Vector3(float.Parse(fieldsExtra[14]), float.Parse(fieldsExtra[15]), float.Parse(fieldsExtra[16]));
            allR[i - 1] = rPos / 1000f;
            allMoonR[i - 1] = moonPos / 1000f;
        }

        rowSlider.maxValue = dataRows.Count - 1;
        rowSlider.onValueChanged.AddListener(UpdateRow);

        speedInputField.text = AutoTimeSpeed.ToString();
        string multiplier = (60 * AutoTimeSpeed).ToString("F2");
        actualSpeedText.text = $"data points per second  -  {multiplier}x actual speed";
        UpdateRow(0f);
    }

    void UpdateRow(float value) {
        currentRow = Mathf.FloorToInt(value);
        time = float.Parse(dataRows[currentRow][0]);
        r = new Vector3(float.Parse(dataRowsExtra[currentRow][1]), float.Parse(dataRowsExtra[currentRow][2]), float.Parse(dataRowsExtra[currentRow][3]));
        v = new Vector3(float.Parse(dataRowsExtra[currentRow][4]), float.Parse(dataRowsExtra[currentRow][5]), float.Parse(dataRowsExtra[currentRow][6]));
        if(currentRow != 0){
            lastV = new Vector3(float.Parse(dataRowsExtra[currentRow - 1][4]), float.Parse(dataRowsExtra[currentRow - 1][5]), float.Parse(dataRowsExtra[currentRow - 1][6]));
        }else{
            lastV = v;
        }
        rMoon = new Vector3(float.Parse(dataRowsExtra[currentRow][14]), float.Parse(dataRowsExtra[currentRow][15]), float.Parse(dataRowsExtra[currentRow][16]));
        vMoon = new Vector3(float.Parse(dataRowsExtra[currentRow][17]), float.Parse(dataRowsExtra[currentRow][18]), float.Parse(dataRowsExtra[currentRow][19]));

        PositionText.text = $"Position: ({r.x}, {r.y}, {r.z})";
        VelocityText.text = $"Velocity: <{v.x}, {v.y}, {v.z}>";

        if(float.Parse(dataRows[currentRow][8]) == 1f){
            wpsaText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            wpsa = CalculateLinkBudget(12, float.Parse(dataRows[currentRow][9]));
            wpsaText.text = "WSPA data rate: " + wpsa.ToString() + " kbps" + ((wpsa > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            wpsaText.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            wpsa = 0f;
            wpsaText.text = "WSPA unavailable";
        }
        if(float.Parse(dataRows[currentRow][10]) == 1f){
            ds54Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds54 = CalculateLinkBudget(34, float.Parse(dataRows[currentRow][11]));
            ds54Text.text = "DS54 data rate: " + ds54.ToString() + " kbps" + ((ds54 > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            ds54Text.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            ds54 = 0f;
            ds54Text.text = "DS54 unavailable";
        }
        if(float.Parse(dataRows[currentRow][12]) == 1f){
            ds24Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds24 = CalculateLinkBudget(34, float.Parse(dataRows[currentRow][13]));
            ds24Text.text = "DS24 data rate: " + ds24.ToString() + " kbps" + ((ds24 > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            ds24Text.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            ds24 = 0f;
            ds24Text.text = "DS24 unavailable";
        }
        if(float.Parse(dataRows[currentRow][14]) == 1f){
            ds34Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds34 = CalculateLinkBudget(34, float.Parse(dataRows[currentRow][15]));
            ds34Text.text = "DS34 data rate: " + ds34.ToString() + " kbps" + ((ds34 > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            ds34Text.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            ds34 = 0f;
            ds34Text.text = "DS34 unavailable";
        }
        
        if(wpsa > ds54 && wpsa > ds24 && wpsa > ds34){
            wpsaText.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
        }
        if(ds54 > wpsa && ds54 > ds24 && ds54 > ds34){
            ds54Text.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
        }
        if(ds24 > wpsa && ds24 > ds54 && ds24 > ds34){
            ds24Text.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
        }
        if(ds34 > wpsa && ds34 > ds54 && ds34 > ds24){
            ds34Text.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
        }
    }

    void Update(){
        if(!TSliderActive){
            totalElapsedTime += Time.deltaTime * AutoTimeSpeed;
            if(Mathf.FloorToInt(totalElapsedTime) >= dataRows.Count){
                totalElapsedTime = 0f;
            }
            currentRow = Mathf.FloorToInt(totalElapsedTime);
            UpdateRow(currentRow);
            TimeText.text = "t = " + (60f * (totalElapsedTime - 1f + 8.236480545f)).ToString() + "s" + FormatTime(60f * (totalElapsedTime - 1f + 8.236480545f));
        }else{
            TimeText.text = "t = " + (60f * (currentRow - 1f + 8.236480545f)).ToString() + "s" + FormatTime(60f * (currentRow - 1f + 8.236480545f));
        }

        if(RenderingTrail){
            trailRenderer.gameObject.SetActive(true);
            moonTrailRenderer.gameObject.SetActive(true);
            
            Vector3[] subArray = new Vector3[currentRow + 1];
            Array.Copy(allR, subArray, currentRow + 1);

            Vector3[] moonSubArray = new Vector3[currentRow + 1];
            Array.Copy(allMoonR, moonSubArray, currentRow + 1);

            trailRenderer.positionCount = currentRow + 1;
            moonTrailRenderer.positionCount = currentRow + 1;

            for (int i = 0; i < currentRow + 1; i++) {
                trailRenderer.SetPosition(i, subArray[i]);
                moonTrailRenderer.SetPosition(i, moonSubArray[i]);
            }
        }else{
            trailRenderer.gameObject.SetActive(false);
            moonTrailRenderer.gameObject.SetActive(false);
        }
    }

    public void TSliderToggle(){
        TSliderActive = !TSliderActive;
        if (!TSliderActive) {
            totalElapsedTime = currentRow;
        }
        rowSlider.gameObject.SetActive(TSliderActive);
        rowSlider.value = currentRow;
        speedInputMenu.SetActive(!TSliderActive);
    }

    public void SizeToggle(){
        enlargedProportions = !enlargedProportions;
    }

    public void ChangeSimSpeed(){
        AutoTimeSpeed = float.Parse(speedInputField.text);
        string multiplier = (60 * AutoTimeSpeed).ToString("F2");
        actualSpeedText.text = $"data points per second  -  {multiplier}x actual speed";
    }

    public float CalculateLinkBudget(float diameter, float range){
        float Pt = 10f;
        float Gt = 9f;
        float Losses = 19.43f;
        float etaR = 0.55f;
        float lambda = 0.136363636f;
        float kb = -228.6f;
        float Ts = 22f;

        float part1 = Pt + Gt - Losses;
        float part2 = 10f * Mathf.Log10(etaR * Mathf.Pow((Mathf.PI * diameter) / lambda, 2));
        float part3 = -20f * Mathf.Log10(4000f * Mathf.PI * range / lambda);
        float part4 = -kb - 10f * Mathf.Log10(Ts);

        float numerator = (part1 + part2 + part3 + part4) / 10f;
        float Bn = Mathf.Pow(10, numerator) / 1000f;

        return Bn;
    }

    public string FormatTime(float totalSeconds){
        int days = (int)(totalSeconds / 86400);
        totalSeconds %= 86400;

        int hours = (int)(totalSeconds / 3600);
        totalSeconds %= 3600;

        int minutes = (int)(totalSeconds / 60);
        int seconds = (int)(totalSeconds % 60);

        string result = "";

        if (minutes == 0) return result;
        result += " (";

        if (days > 0) result += string.Format("{0:D2} days, ", days);
        if (hours > 0) result += string.Format("{0:D2} hours, ", hours);

        result += string.Format("{0:D2} minutes, {1:D2} seconds)", minutes, seconds);

        return result;
    }
}
