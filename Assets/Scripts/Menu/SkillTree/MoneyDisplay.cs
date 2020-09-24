using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    public TextMeshProUGUI money;

    void Start()
    {
        Team.money = 3000;
        money.text = "Money : " + Team.money.ToString() + " $";
    }

    void Update()
    {
        money.text = "Money : " + Team.money.ToString() + " $";
    }
}
