using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPBar : MonoBehaviour
{
    public UnitCondition Player;
    TMP_Text HPValue;
    Image HPBar;

    private void Start()
    {
        HPBar = GetComponent<Image>();
        HPValue = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        HPValue.text = ((int)Player.HP).ToString() + "%";
        HPBar.fillAmount = (float)Player.HP / 100f;
    }
}
