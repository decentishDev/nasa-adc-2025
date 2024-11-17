using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class SliderScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool sliderMoving = false;
    public Slider slider;  

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        
    }

    private void OnSliderValueChanged(float value)
    {
        if (sliderMoving)
        {
            Debug.Log("Slider is moving. Current value: " + value);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        sliderMoving = true;
        Debug.Log("Slider start drag detected");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        sliderMoving = false;
    }
}
