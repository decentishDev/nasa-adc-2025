using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class SliderScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public static bool sliderMoving = false;
    public Slider slider;  

    public void OnPointerDown(PointerEventData eventData){
        sliderMoving = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        sliderMoving = false;
    }
}
