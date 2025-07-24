using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CharacterViewer : MonoBehaviour {
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public Slider senseSlide;

    public PhotonView PV;

    public Canvas escPanel;

    private void Update()
    {
        //if (PV.IsMine)
        //{
            sensX = senseSlide.value;
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        if(escPanel.enabled == false)
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        //}
    }
}
