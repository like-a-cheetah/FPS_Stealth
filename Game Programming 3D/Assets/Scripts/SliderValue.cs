using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Slider sensitivitySlider;

    void Start()
    {
        //sensitivitySlider = GameObject.Find("Sensitivity").GetComponent<Slider>();
        sensitivitySlider.onValueChanged.AddListener(delegate { OnSensitiveChanged(); });
    }

    private void OnSensitiveChanged()
    {
    }
}
