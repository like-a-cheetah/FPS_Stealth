using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public PlayerController player;

    private Slider HPbar;
    public GameObject unit;
    public GameObject full;

    private void Start()
    {
        HPbar = GetComponent<Slider>();
        player = GetComponentInParent<PlayerController>();

        HPbar.maxValue = unit.GetComponent<UnitCondition>().HP;
    }

    void Update()
    {
        this.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y + 1.5f, unit.transform.position.z);
        this.transform.rotation = player.transform.rotation;
        //float distance = Vector3.Distance(player.transform.position, unit.transform.position);

        //Debug.Log(distance);

        //if (distance <= 1.5f)
        //{

        //}
        //else if(distance <= 0.7f)
        //{
        //    Rect.width = 10f;
        //}
        //else if(distance)
        //GetComponent<RectTransform>().rect.width - 

        HPbar.value = unit.GetComponent<UnitCondition>().HP;

        if(HPbar.value <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
