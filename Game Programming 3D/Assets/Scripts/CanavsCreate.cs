using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanavsCreate : MonoBehaviour
{
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();

        canvas.worldCamera.name ="BasicCamera";
    }

    void Update()
    {
    }
}
