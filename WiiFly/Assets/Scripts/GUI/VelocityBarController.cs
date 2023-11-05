using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VelocityBarController : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxIntensity(float intensity)
    {
        slider.maxValue = intensity;
    }

    public void SetMinIntensity(float intensity)
    {
        slider.minValue = intensity;
    }

    public void SetIntensity(float intensity)
    {
        slider.value = intensity;
        UpdateSliderSense();
        fill.color = Color.Lerp(Color.green, Color.red, Mathf.Abs(intensity));
    }

    private void UpdateSliderSense()
    {
        slider.fillRect.anchorMin = new Vector2(Mathf.Clamp(slider.handleRect.anchorMin.x, 0, 0.5f), 0);
        slider.fillRect.anchorMax = new Vector2(Mathf.Clamp(slider.handleRect.anchorMax.x, 0.5f, 1), 1);
    }
}
