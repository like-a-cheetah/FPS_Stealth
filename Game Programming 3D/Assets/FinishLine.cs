using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public Canvas FinishCanvas;
    public Camera MainCamera;
    public Canvas AimCanvas;

    private void Start()
    {
        MainCamera.enabled = false;
        FinishCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("승리!");
            FinishCanvas.enabled = true;
            MainCamera.enabled = true;
        AimCanvas.enabled = false;
    }
}
