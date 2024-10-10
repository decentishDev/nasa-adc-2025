using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimVars : MonoBehaviour
{
    public TextAsset csvFile;
    public Slider rowSlider;

    public static float time = 0;
    public static float[] R = new float[3];

    private List<string[]> dataRows = new List<string[]>();
    private int currentRow = 0;

    void Start()
    {
        string[] data = csvFile.text.Split(new char[] { '\n' });
        for (int i = 1; i < data.Length; i++)
        {
            string[] fields = data[i].Split(new char[] { ',' });
            dataRows.Add(fields);
        }

        rowSlider.maxValue = dataRows.Count - 1;
        rowSlider.onValueChanged.AddListener(UpdateRow);
    }

    void UpdateRow(float value)
    {
        currentRow = Mathf.FloorToInt(value);
        time = float.Parse(dataRows[currentRow][0]);
        R[0] = float.Parse(dataRows[currentRow][1]);
        R[1] = float.Parse(dataRows[currentRow][2]);
        R[2] = float.Parse(dataRows[currentRow][3]);
    }
}
