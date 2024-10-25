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

    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI PositionText;
    public TextMeshProUGUI VelocityText;
    public LineRenderer trailRenderer;
    public LineRenderer moonTrailRenderer;

    public GameObject loadingScreen;
    public RectTransform loadingBar;

    public static float AutoTimeSpeed = 10f;

    public static double time = 0;
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

    private float[] allT = new float[12981];
    private Vector3[] allR = new Vector3[12981];
    private Vector3[] allV = new Vector3[12981];
    private float[] allM = new float[12981];
    private float[] allWPSA = new float[12981];
    private float[] allDS54 = new float[12981];
    private float[] allDS24 = new float[12981];
    private float[] allDS34 = new float[12981];
    private Vector3[] allMoonR = new Vector3[12981];
    private Vector3[] allMoonV = new Vector3[12981];
    public static int currentRow = 0;

    public static bool TSliderActive = true;
    public static bool enlargedProportions = false;

    public static bool cameraZoomer = false;

    public static bool flamer = false;


    public float minTime = 0;
    public float maxTime = 0;

    public static float lerpConstant = 0.05f;

    void Awake(){
        loadingScreen.SetActive(true);
    }

    void Start(){
        StartCoroutine(ParseDataAndInitializeUI());
    }

    IEnumerator ParseDataAndInitializeUI(){
        string[] data = csvFile.text.Split(new char[] {'\n'});
        string[] dataExtra = csvFileExtra.text.Split(new char[] {'\n'});

        for (int i = 1; i < data.Length; i++)
        {
            string[] fields = data[i].Split(new char[] {','});
            string[] fieldsExtra = dataExtra[i].Split(new char[] {','});
            float[] fieldsF = new float[fields.Length];
            float[] fieldsExtraF = new float[fieldsExtra.Length];

            for(int j = 0; j < fields.Length; j++)
            {
                if (!float.TryParse(fields[j], out fieldsF[j]))
                {
                    fieldsF[j] = 0f;
                }
            }

            for(int j = 0; j < fieldsExtra.Length; j++)
            {
                if (!float.TryParse(fieldsExtra[j], out fieldsExtraF[j]))
                {
                    fieldsExtraF[j] = 0f;
                }
            }

            allT[i - 1] = fieldsF[0] * 60f;
            allR[i - 1] = new Vector3(fieldsExtraF[1], fieldsExtraF[2], fieldsExtraF[3]) / 1000f;
            allV[i - 1] = new Vector3(fieldsExtraF[4], fieldsExtraF[5], fieldsExtraF[6]) / 1000f;
            allM[i - 1] = fieldsF[7];
            allWPSA[i - 1] = fieldsF[9];
            allDS54[i - 1] = fieldsF[11];
            allDS24[i - 1] = fieldsF[13];
            allDS34[i - 1] = fieldsF[15];
            allMoonR[i - 1] = new Vector3(fieldsExtraF[14], fieldsExtraF[15], fieldsExtraF[16]) / 1000f;
            allMoonV[i - 1] = new Vector3(fieldsExtraF[17], fieldsExtraF[18], fieldsExtraF[19]) / 1000f;

            if (i % 100 == 0)
            {
                yield return null;
            }

            loadingBar.offsetMax = new Vector2((((float) i) / ((float) data.Length)) * 500f - 500f, loadingBar.offsetMax.y);
        }

        minTime = allT[0];
        maxTime = allT[allT.Length - 1];
        rowSlider.minValue = minTime;
        rowSlider.maxValue = maxTime;
        rowSlider.onValueChanged.AddListener(UpdateRow);

        speedInputField.text = AutoTimeSpeed.ToString();

        UpdateRow(minTime);
        loadingScreen.SetActive(false);
    }

    private int findRow(float t){
        int low = 0;
        int high = allT.Length - 1;

        if (t < minTime) {
            return -1;
        }

        while (low <= high) {
            int mid = (low + high) / 2;

            if (allT[mid] == t) {
                return mid;
            }
            else if (allT[mid] < t) {
                low = mid + 1;
            }
            else {
                high = mid - 1;
            }
        }

        return high;
    }

    void UpdateRow(float tValue) {
        
        currentRow = findRow(tValue);
        
        if(TSliderActive){
            time = tValue;
        }
        //time = tValue;
        r = allR[currentRow];
        v = allV[currentRow];
        if(currentRow != 0){
            lastV = allV[currentRow - 1];
        }else{
            lastV = v;
        }
        rMoon = allMoonR[currentRow];
        vMoon = allMoonV[currentRow];

        PositionText.text = $"Position: ({(int) (r.x * 1000f)}, {(int) (r.y * 1000f)}, {(int) (r.z * 1000f)})";
        VelocityText.text = $"Velocity: <{Mathf.Round(v.x * 1000f * 100f) * 0.01f}, {Mathf.Round(v.y * 1000f * 100f) * 0.01f}, {Mathf.Round(v.z * 1000f * 100f) * 0.01f}>";

        if(allWPSA[currentRow] != 0){
            wpsaText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            wpsa = CalculateLinkBudget(12, allWPSA[currentRow]);
            wpsaText.text = "WSPA data rate: " + ((int) wpsa).ToString() + " kbps" + ((wpsa > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            wpsaText.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            wpsa = 0f;
            wpsaText.text = "WSPA unavailable";
        }
        if(allDS54[currentRow] != 0){
            ds54Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds54 = CalculateLinkBudget(34, allDS54[currentRow]);
            ds54Text.text = "DS54 data rate: " + ((int) ds54).ToString() + " kbps" + ((ds54 > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            ds54Text.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            ds54 = 0f;
            ds54Text.text = "DS54 unavailable";
        }
        if(allDS24[currentRow] != 0){
            ds24Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds24 = CalculateLinkBudget(34, allDS24[currentRow]);
            ds24Text.text = "DS24 data rate: " + ((int) ds24).ToString() + " kbps" + ((ds24 > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            ds24Text.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            ds24 = 0f;
            ds24Text.text = "DS24 unavailable";
        }
        if(allDS34[currentRow] != 0){
            ds34Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds34 = CalculateLinkBudget(34, allDS34[currentRow]);
            ds34Text.text = "DS34 data rate: " + ((int) ds34).ToString() + " kbps" + ((ds34 > 10000) ? " (max 10,000 kbps)" : "");
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
        lerpConstant = 10f * Time.deltaTime;

        if(!TSliderActive){
            time += Time.deltaTime * AutoTimeSpeed;
            
            if(time > maxTime){
                time = minTime;
            }
            if(time < minTime){
                time = maxTime;
            }
            UpdateRow((float) time);
        }else{
            
        }

        TimeText.text = "t = " + (Mathf.Round((float) time * 100f) * 0.01f).ToString() + FormatTime((float) time);

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
        rowSlider.gameObject.SetActive(TSliderActive);
        rowSlider.value = (float) time;
        speedInputMenu.SetActive(!TSliderActive);
    }

    public void SizeToggle(){
        enlargedProportions = !enlargedProportions;
    }

    public void CameraZoom(){
        if(cameraZoomer) {
            cameraZoomer = false;
        } else {
            cameraZoomer = true;
        }    }

    public void FlameOn(){
        if(flamer) {
            flamer = false;
        } else {
            flamer = true;
        }
    }

    public void ChangeSimSpeed(){
        if (!float.TryParse(speedInputField.text, out AutoTimeSpeed)){
            AutoTimeSpeed = 0f;
        }
        speedInputField.text = AutoTimeSpeed.ToString();
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
