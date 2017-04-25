using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyManager : MonoBehaviour {

    int _funds = 999;
    public int funds
    {
        get { return _funds; }
        set
        {
            if (value < 0) value = 0;
            displayText.text = "$" + value;
            Debug.Log("changing money");
            _funds = value;
        }
    }

    Text displayText;


    void Start()
    {
        displayText = GameObject.Find("Money Display").GetComponent<Text>();
        displayText.text = "$" + _funds;
    }
}
