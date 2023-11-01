using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityBarController : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxSpeed(float speed)
    {
        slider.maxValue = speed;
    }

    public void SetMinSpeed(float speed)
    {
        slider.minValue = speed;
    }

    public void SetSpeed(float speed) { 
        slider.value = speed; 
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
