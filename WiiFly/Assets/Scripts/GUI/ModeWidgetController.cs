using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeWidgetController : MonoBehaviour
{
    private Text textWidget; 

    private void Start()
    {
        textWidget = GetComponent<Text>();
    }

    public void ChangeToOrbitMode()
    {
        textWidget.text = "Orbit Mode";
    }

    public void ChangeToFlyMode()
    {
        textWidget.text = "Fly Mode";
    }
}
