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
    public TextMeshProUGUI MassText;

    public LineRenderer trailRenderer1;
    public LineRenderer trailRenderer2;
    public LineRenderer trailRenderer3;
    public LineRenderer trailRenderer4;
    public LineRenderer trailRenderer1Off;
    public LineRenderer trailRenderer2Off;
    public LineRenderer trailRenderer3Off;
    public LineRenderer trailRenderer4Off;
    public LineRenderer fullTrailRenderer1;
    public LineRenderer fullTrailRenderer2;
    public LineRenderer fullTrailRenderer3;
    public LineRenderer fullTrailRenderer4;
    public LineRenderer moonTrailRenderer;
    public LineRenderer fullMoonTrailRenderer;

    public GameObject loadingScreen;
    public RectTransform loadingBar;
    public Image blackOverlay;

    public Toggle HighestDataRateToggle;
    public Toggle LeastChangesToggle;

    public static float AutoTimeSpeed = 10f;

    public static decimal time = 0m;
    public static Vector3 r;
    public static Vector3 rOff;
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
    public static Vector3[] allR = new Vector3[12981];
    private Vector3[] allExpandedR = new Vector3[12981];
    public static Vector3[] allROff = new Vector3[12981];
    private Vector3[] allExpandedROff = new Vector3[12981];
    private Vector3[] allV = new Vector3[12981];
    private float[] allM = new float[12981];
    private float[] allWPSA = new float[12981];
    private float[] allDS54 = new float[12981];
    private float[] allDS24 = new float[12981];
    private float[] allDS34 = new float[12981];
    private Vector3[] allMoonR = new Vector3[12981];
    private Vector3[] allMoonV = new Vector3[12981];
    private float[] allTotalDistance = new float[12981];
    private int[] satellitesForLeastChanges = new int[12981];

    public static int currentRow = 0;
    public static bool TSliderActive = true;
    public static bool enlargedProportions = false;
    public bool usingLeastChanges = false;

    public static bool cameraZoomer = false;
    public static bool isSimulation = false;

    public static bool flamer = true;

    public float minTime = 0;
    public float maxTime = 0;

    public static float lerpConstant = 0.05f;

    public static bool showingOffNominal = false;

    void Awake(){
        loadingScreen.SetActive(true);
    }

    void Start(){
        StartCoroutine(ParseDataAndInitializeUI());
        StartCoroutine(FadeInLoading(1f));
    }

    IEnumerator FadeInLoading(float startAlpha){
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration){
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            blackOverlay.color = new Color(0f, 0f, 0f, newAlpha);
            yield return null;
        }

        blackOverlay.color = new Color(0f, 0f, 0f, 0f);
    }

    IEnumerator ParseDataAndInitializeUI(){
        string[] data = csvFile.text.Split(new char[] {'\n'});
        string[] dataExtra = csvFileExtra.text.Split(new char[] {'\n'});

        Vector3 previousR = new Vector3(0f, 0f, 0f);

        int currentSatellite = 0;

        for (int i = 1; i < dataExtra.Length; i++){
            string[] fields = data[i].Split(new char[]{','});
            string[] fieldsExtra = dataExtra[i].Split(new char[]{','});
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

            Vector3 thisR = new Vector3(fieldsF[1], fieldsF[2], fieldsF[3]);
            Vector3 thisROff = new Vector3(fieldsExtraF[1], fieldsExtraF[2], fieldsExtraF[3]);
            
            allT[i - 1] = fieldsF[0] * 60f;
            allR[i - 1] = thisR / 1000f;
            allROff[i - 1] = thisROff / 1000f;
            allV[i - 1] = new Vector3(fieldsExtraF[4], fieldsExtraF[5], fieldsExtraF[6]) / 1000f;
            allM[i - 1] = fieldsF[7];
            allWPSA[i - 1] = fieldsF[9];
            allDS54[i - 1] = fieldsF[11];
            allDS24[i - 1] = fieldsF[13];
            allDS34[i - 1] = fieldsF[15];
            allMoonR[i - 1] = new Vector3(fieldsExtraF[14], fieldsExtraF[15], fieldsExtraF[16]) / 1000f;
            allMoonV[i - 1] = new Vector3(fieldsExtraF[17], fieldsExtraF[18], fieldsExtraF[19]) / 1000f;

            allExpandedR[i - 1] = expandPoint(thisR / 1000f);
            allExpandedROff[i - 1] = expandPoint(thisROff / 1000f);
            
            if(i > 1){
                allTotalDistance[i - 1] = allTotalDistance[i - 2] + (thisR - previousR).magnitude;
            }

            previousR = thisR;

            if (i % 100 == 0)
            {
                yield return null;
            }

            loadingBar.offsetMax = new Vector2((((float) i) / ((float) data.Length)) * 500f - 500f, loadingBar.offsetMax.y);

            bool WPSAavailable = allWPSA[i - 1] != 0f;
            bool DS54available = allDS54[i - 1] != 0f;
            bool DS24available = allDS24[i - 1] != 0f;
            bool DS34available = allDS34[i - 1] != 0f;

            bool[] available = {WPSAavailable, DS54available, DS24available, DS34available};

            if(currentSatellite == -1 || !available[currentSatellite]){
                for(int j = 0; j < 4; j++){
                    if(available[j]){
                        currentSatellite = j;
                        break;
                    }else if(j == 3){
                        currentSatellite = -1;
                    }
                }
            }
            satellitesForLeastChanges[i - 1] = currentSatellite;
        }

        minTime = allT[0];
        maxTime = allT[allT.Length - 1];
        rowSlider.minValue = minTime;
        rowSlider.maxValue = maxTime;
        rowSlider.onValueChanged.AddListener(UpdateRow);

        speedInputField.text = AutoTimeSpeed.ToString();

        UpdateRow(minTime);

        Vector3[] subArray;
            
        subArray = new Vector3[12981];
        Array.Copy(allR, subArray, 12981);

        int segment1End = 1496;
        fullTrailRenderer1.positionCount = segment1End + 1;
        for(int i = 0; i <= segment1End; i++){
            fullTrailRenderer1.SetPosition(i, subArray[i]);
        }

        int segment2End = 7164 - 1496;
        fullTrailRenderer2.positionCount = segment2End + 1;
        for (int i = 0; i <= segment2End; i++){
            fullTrailRenderer2.SetPosition(i, subArray[1496 + i]);
        }
    
        int segment3End = 12913 - 7164;
        fullTrailRenderer3.positionCount = segment3End + 1;
        for (int i = 0; i <= segment3End; i++){
            fullTrailRenderer3.SetPosition(i, subArray[7163 + i]);//7164
        }

        int segment4End = 12980 - 12913;
        fullTrailRenderer4.positionCount = segment4End + 1;
        for (int i = 0; i <= segment4End; i++){
            fullTrailRenderer4.SetPosition(i, subArray[12913 + i]);
        }            

        Vector3[] moonSubArray = new Vector3[12981];
        Array.Copy(allMoonR, moonSubArray, 12981);
        moonTrailRenderer.positionCount = 12981;
        for (int i = 0; i < 12981; i++){
            fullMoonTrailRenderer.SetPosition(i, moonSubArray[i]);
        }

        loadingScreen.SetActive(false);
    }

    private Vector3 expandPoint(Vector3 currentPosition){

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
        return Vector3.Lerp(currentPosition, targetPosition, lerpFactor);
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
            rOff = allExpandedROff[currentRow];
        }else{
            r = allR[currentRow];
            rOff = allROff[currentRow];
        }
        v = allV[currentRow];
        if(currentRow != 0){
            lastV = allV[currentRow - 1];
        }else{
            lastV = v;
        }
        m = allM[currentRow];
        rMoon = allMoonR[currentRow];
        vMoon = allMoonV[currentRow];
        PositionXText.text = $"{(int) (r.x * 1000f)},";
        PositionYText.text = $"{(int) (r.y * 1000f)},";
        PositionZText.text = $"{(int) (r.z * 1000f)}";
        VelocityXText.text = $"{Mathf.Round(v.x * 1000f * 1000f) * 0.001f},";
        VelocityYText.text = $"{Mathf.Round(v.y * 1000f * 1000f) * 0.001f},";
        VelocityZText.text = $"{Mathf.Round(v.z * 1000f * 1000f) * 0.001f}";
        MassText.text = $"Mass: {m} kg";
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
        if(usingLeastChanges){
            switch (satellitesForLeastChanges[currentRow]){
            case 0:
                wpsaText.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
                break;
            case 1:
                ds54Text.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
                break;

            case 2:
                ds24Text.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
                break;

            default:
                ds34Text.color = new Color(0.1f, 1.0f, 0.1f, 1.0f);
                break;
            }
        }else{
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

        totalDistanceText.text = "Total distance traveled: \n" + ((int) allTotalDistance[currentRow]) + " km";

        if(time < 90000){
            MissionStatusText.text = "Orbiting Earth";
            MissionStatusText.color = new Color(0f, 1f, 1f, 1f);
        }else if(time < 430000){
            MissionStatusText.text = "On the way to the Moon";
            MissionStatusText.color = new Color(0f, 1f, 0f, 1f);
        }else if(time < 775000){
            MissionStatusText.text = "Returning to Earth";
            MissionStatusText.color = new Color(1f, 1f, 0f, 1f);
        }else{
            MissionStatusText.text = "Entry, Descent, and Landing";
            MissionStatusText.color = new Color(1f, 107f / 255f, 0f, 1f);
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
            trailRenderer1.gameObject.SetActive(true);
            trailRenderer2.gameObject.SetActive(currentRow > 1495);
            trailRenderer3.gameObject.SetActive(currentRow > 7163);
            trailRenderer4.gameObject.SetActive(currentRow > 12912);
            if(showingOffNominal){
                trailRenderer1Off.gameObject.SetActive(true);
                trailRenderer2Off.gameObject.SetActive(currentRow > 1495);
                trailRenderer3Off.gameObject.SetActive(currentRow > 7163);
                trailRenderer4Off.gameObject.SetActive(currentRow > 12912);
            }else{
                trailRenderer1Off.gameObject.SetActive(false);
                trailRenderer2Off.gameObject.SetActive(false);
                trailRenderer3Off.gameObject.SetActive(false);
                trailRenderer4Off.gameObject.SetActive(false);
            }

            Vector3[] subArray;
            Vector3[] subArrayOff;

            subArray = new Vector3[currentRow + 1];
            subArrayOff = new Vector3[currentRow + 1];

            if(enlargedProportions){
                Array.Copy(allExpandedR, subArray, currentRow + 1);
                Array.Copy(allExpandedROff, subArrayOff, currentRow + 1);
            }else{
                Array.Copy(allR, subArray, currentRow + 1);
                Array.Copy(allROff, subArrayOff, currentRow + 1);
            }

            int segment1End = Mathf.Min(currentRow, 1496);
            trailRenderer1.positionCount = segment1End + 1;
            trailRenderer1Off.positionCount = segment1End + 1;
            for(int i = 0; i <= segment1End; i++){
                trailRenderer1.SetPosition(i, subArray[i]);
                trailRenderer1Off.SetPosition(i, subArrayOff[i]);
            }

            if(currentRow > 1495){
                int segment2End = Mathf.Min(currentRow, 7164) - 1496;
                trailRenderer2.positionCount = segment2End + 1;
                trailRenderer2Off.positionCount = segment2End + 1;
                for (int i = 0; i <= segment2End; i++){
                    trailRenderer2.SetPosition(i, subArray[1496 + i]);
                    trailRenderer2Off.SetPosition(i, subArrayOff[1496 + i]);
                }
            }

            if(currentRow > 7163){
                int segment3End = Mathf.Min(currentRow, 12913) - 7164;
                trailRenderer3.positionCount = segment3End + 1;
                trailRenderer3Off.positionCount = segment3End + 1;
                for (int i = 0; i <= segment3End; i++)
                {
                    trailRenderer3.SetPosition(i, subArray[7164 + i]);
                    trailRenderer3Off.SetPosition(i, subArrayOff[7164 + i]);
                }
            }

            if(currentRow > 12912){
                int segment4End = currentRow - 12913;
                trailRenderer4.positionCount = segment4End + 1;
                trailRenderer4Off.positionCount = segment4End + 1;
                for (int i = 0; i <= segment4End; i++)
                {
                    trailRenderer4.SetPosition(i, subArray[12913 + i]);
                    trailRenderer4Off.SetPosition(i, subArrayOff[12913 + i]);
                }
            }

            moonTrailRenderer.gameObject.SetActive(true);
            Vector3[] moonSubArray = new Vector3[currentRow + 1];
            Array.Copy(allMoonR, moonSubArray, currentRow + 1);
            moonTrailRenderer.positionCount = currentRow + 1;
            for (int i = 0; i < currentRow + 1; i++){
                moonTrailRenderer.SetPosition(i, moonSubArray[i]);
            }
        }else{
            trailRenderer1.gameObject.SetActive(false);
            trailRenderer2.gameObject.SetActive(false);
            trailRenderer3.gameObject.SetActive(false);
            trailRenderer4.gameObject.SetActive(false);
            moonTrailRenderer.gameObject.SetActive(false);
            trailRenderer1Off.gameObject.SetActive(false);
            trailRenderer2Off.gameObject.SetActive(false);
            trailRenderer3Off.gameObject.SetActive(false);
            trailRenderer4Off.gameObject.SetActive(false);
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

    public void ChangeIsShowingOffNominal(){
        showingOffNominal = !showingOffNominal;
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

    public void ChangeHighestDataRate(){
        if(LeastChangesToggle.isOn == HighestDataRateToggle.isOn){
            usingLeastChanges = !usingLeastChanges;
            LeastChangesToggle.isOn = !HighestDataRateToggle.isOn;
        }
    }

    public void ChangeLeastChanges(){
        if(LeastChangesToggle.isOn == HighestDataRateToggle.isOn){
            usingLeastChanges = !usingLeastChanges;
            HighestDataRateToggle.isOn = !LeastChangesToggle.isOn;
        }
    }

    public float CalculateLinkBudget(float diameter, float range){
        float Pt = 10f;
        float Gt = 9f;
        float Losses = 19.43f;
        float etaR = 0.55f;
        float lambda = 0.136363636f;
        float kb = -228.6f;
        float Ts = 222f;

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
