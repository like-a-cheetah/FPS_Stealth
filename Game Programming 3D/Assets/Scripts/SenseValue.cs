using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SenseValue : MonoBehaviour
{
    public Slider slide;
    private TMP_Text text;
    
    //public int Value()
    //{
    //    return (int)slide.value;
    //}

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        int value = (int)slide.value;
        text.text = value.ToString();
    }
}
