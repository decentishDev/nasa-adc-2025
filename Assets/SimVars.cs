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
    public GameObject sliderCheckbox;
    public GameObject minimap;
    public RectTransform minimapLine;
    public RectTransform minimapSymbol;
    public TextMeshProUGUI dataModeLabel;
    public TMP_InputField speedInputField;

    public TextMeshProUGUI MissionStatusText;

    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI PositionXText;
    public TextMeshProUGUI PositionYText;
    public TextMeshProUGUI PositionZText;
    public TextMeshProUGUI VelocityXText;
    public TextMeshProUGUI VelocityYText;
    public TextMeshProUGUI VelocityZText;
    public TextMeshProUGUI totalDistanceText;

    public LineRenderer trailRenderer;
    public LineRenderer moonTrailRenderer;

    public GameObject loadingScreen;
    public RectTransform loadingBar;

    public static float AutoTimeSpeed = 10f;

    public decimal time = 0m;
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
    private Vector3[] allExpandedR = new Vector3[12981];
    private Vector3[] allV = new Vector3[12981];
    private float[] allM = new float[12981];
    private float[] allWPSA = new float[12981];
    private float[] allDS54 = new float[12981];
    private float[] allDS24 = new float[12981];
    private float[] allDS34 = new float[12981];
    private Vector3[] allMoonR = new Vector3[12981];
    private Vector3[] allMoonV = new Vector3[12981];
    private float[] allTotalDistance = new float[12981];
    public static int currentRow = 0;

    public static bool TSliderActive = true;
    public static bool enlargedProportions = false;

    public static bool cameraZoomer = false;
    public static bool isSimulation = false;

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

        Vector3 previousR = new Vector3(0f, 0f, 0f);

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

            Vector3 thisR = new Vector3(fieldsExtraF[1], fieldsExtraF[2], fieldsExtraF[3]);
            
            allT[i - 1] = fieldsF[0] * 60f;
            allR[i - 1] = thisR / 1000f;
            allV[i - 1] = new Vector3(fieldsExtraF[4], fieldsExtraF[5], fieldsExtraF[6]) / 1000f;
            allM[i - 1] = fieldsF[7];
            allWPSA[i - 1] = fieldsF[9];
            allDS54[i - 1] = fieldsF[11];
            allDS24[i - 1] = fieldsF[13];
            allDS34[i - 1] = fieldsF[15];
            allMoonR[i - 1] = new Vector3(fieldsExtraF[14], fieldsExtraF[15], fieldsExtraF[16]) / 1000f;
            allMoonV[i - 1] = new Vector3(fieldsExtraF[17], fieldsExtraF[18], fieldsExtraF[19]) / 1000f;

            Vector3 currentPosition = thisR / 1000f;

            Vector3 F1 = new Vector3(-375, -129, -61);
            Vector3 F2 = new Vector3(0, 0, 0);

            float expansionFactorF1 = 25f;
            float expansionFactorF2 = 150f;

            float d1 = Vector3.Distance(currentPosition, F1);
            float d2 = Vector3.Distance(currentPosition, F2);

            float power = 0.5f;
            float w1 = 1f / Mathf.Pow(d1 + 0.01f, power);
            float w2 = 1f / Mathf.Pow(d2 + 0.01f, power);

            float sumWeights = w1 + w2;
            w1 /= sumWeights;
            w2 /= sumWeights;

            Vector3 directionFromF1 = (currentPosition - F1).normalized;
            Vector3 directionFromF2 = (currentPosition - F2).normalized;

            Vector3 targetPosition = currentPosition + expansionFactorF1 * w1 * directionFromF1 + expansionFactorF2 * w2 * directionFromF2;

            float lerpFactor = 0.1f;
            allExpandedR[i - 1] = Vector3.Lerp(currentPosition, targetPosition, lerpFactor);

            
            if(i > 1){
                allTotalDistance[i - 1] = allTotalDistance[i - 2] + (thisR - previousR).magnitude;
            }

            previousR = thisR;

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
        
        if(TSliderActive && !isSimulation){
            time = (decimal) tValue;
        }
        //time = tValue;
        if(enlargedProportions){
            r = allExpandedR[currentRow];
        }else{
            r = allR[currentRow];
        }
        v = allV[currentRow];
        if(currentRow != 0){
            lastV = allV[currentRow - 1];
        }else{
            lastV = v;
        }
        rMoon = allMoonR[currentRow];
        vMoon = allMoonV[currentRow];
        PositionXText.text = $"{(int) (r.x * 1000f)},";
        PositionYText.text = $"{(int) (r.y * 1000f)},";
        PositionZText.text = $"{(int) (r.z * 1000f)}";
        VelocityXText.text = $"{Mathf.Round(v.x * 1000f * 1000f) * 0.001f},";
        VelocityYText.text = $"{Mathf.Round(v.y * 1000f * 1000f) * 0.001f},";
        VelocityZText.text = $"{Mathf.Round(v.z * 1000f * 1000f) * 0.001f}";
        if(allWPSA[currentRow] != 0){
            wpsaText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            wpsa = CalculateLinkBudget(12, allWPSA[currentRow]);
            wpsaText.text = "WSPA: " + ((int) wpsa).ToString() + " kbps" + ((wpsa > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            wpsaText.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            wpsa = 0f;
            wpsaText.text = "WSPA unavailable";
        }
        if(allDS54[currentRow] != 0){
            ds54Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds54 = CalculateLinkBudget(34, allDS54[currentRow]);
            ds54Text.text = "DS54: " + ((int) ds54).ToString() + " kbps" + ((ds54 > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            ds54Text.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            ds54 = 0f;
            ds54Text.text = "DS54 unavailable";
        }
        if(allDS24[currentRow] != 0){
            ds24Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds24 = CalculateLinkBudget(34, allDS24[currentRow]);
            ds24Text.text = "DS24: " + ((int) ds24).ToString() + " kbps" + ((ds24 > 10000) ? " (max 10,000 kbps)" : "");
        }else{
            ds24Text.color = new Color(1.0f, 1.0f, 1.0f, 0.35f);
            ds24 = 0f;
            ds24Text.text = "DS24 unavailable";
        }
        if(allDS34[currentRow] != 0){
            ds34Text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ds34 = CalculateLinkBudget(34, allDS34[currentRow]);
            ds34Text.text = "DS34 : " + ((int) ds34).ToString() + " kbps" + ((ds34 > 10000) ? " (max 10,000 kbps)" : "");
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

        totalDistanceText.text = "Total distance traveled: \n" + ((int) allTotalDistance[currentRow]) + " km";

        if(time < 90000){
            MissionStatusText.text = "Orbiting Earth";
        }else if(time < 430000){
            MissionStatusText.text = "On the way to the Moon";
        }else if(time < 770000){
            MissionStatusText.text = "Returning to Earth";
        }else{
            MissionStatusText.text = "Entry, Descent, and Landing";
        }
    }

    void Update(){
        lerpConstant = 10f * Time.deltaTime;
        decimal deltaT = (decimal) Time.deltaTime;

        if(isSimulation){
            
            time += deltaT;

            if((float) time > maxTime){
                time = (decimal) minTime;
            }
            if((float) time < minTime){
                time = (decimal) maxTime;
            }
            UpdateRow((float) time);
            float width = (float) minimapLine.rect.width - minimapSymbol.rect.width * 0.5f;
            float fTime = (float) time;
            minimapSymbol.anchoredPosition = new Vector2((width * (fTime / maxTime)) - (width / 2f), minimapSymbol.anchoredPosition.y);

        }else if(!TSliderActive){
            time += deltaT * ((decimal) AutoTimeSpeed);
            
            if((float) time > maxTime){
                time = (decimal) minTime;
            }
            if((float) time < minTime){
                time = (decimal) maxTime;
            }
            UpdateRow((float) time);
        }

        TimeText.text = $"Time elapsed: {FormatTime((float) time)}\nt = {Math.Round(time, 2)} s";

        if(RenderingTrail){
            trailRenderer.gameObject.SetActive(true);
            moonTrailRenderer.gameObject.SetActive(true);
            
            Vector3[] subArray = new Vector3[currentRow + 1];
            if(enlargedProportions){
                Array.Copy(allExpandedR, subArray, currentRow + 1);
            }else{
                Array.Copy(allR, subArray, currentRow + 1);
            }

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
        UpdateRow((float) time);
    }
    public void SimulationToggle(){
        isSimulation = !isSimulation;
        minimap.SetActive(isSimulation);
        sliderCheckbox.SetActive(!isSimulation);
        if(isSimulation){
            rowSlider.gameObject.SetActive(false);
            speedInputMenu.SetActive(false);
            dataModeLabel.text = "Simulation";
        }else{
            rowSlider.gameObject.SetActive(TSliderActive);
            rowSlider.value = (float) time;
            speedInputMenu.SetActive(!TSliderActive);
            dataModeLabel.text = "Data explorer";
        }
    }

    public void CameraZoom(){
        if(cameraZoomer) {
            cameraZoomer = false;
        } else {
            cameraZoomer = true;
        }
    }

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

    public static string FormatTime(float totalSeconds) {
        int days = (int)(totalSeconds / 86400);
        int hours = (int)((totalSeconds % 86400) / 3600);
        int minutes = (int)((totalSeconds % 3600) / 60);
        int seconds = (int)(totalSeconds % 60);

        return $"{days:D2} : {hours:D2} : {minutes:D2} : {seconds:D2}";
    }
}
