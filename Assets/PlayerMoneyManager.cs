using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMoneyManager : MonoBehaviour {

    [SerializeField] int _funds = 499;
    public int funds
    {
        get { return _funds; }
        set
        {
            if (value < 0) value = 0;
			string notes = "$" + value;
			displayText.transform.DOSpiral(2, Vector3.forward, SpiralMode.ExpandThenContract, 100, 100);
			//displayText.transform.DOShakeScale(1.0f,new Vector3(0.1f,0.1f,0.1f),10,90,false);;
			displayText.DOText(notes,1.0f,true,ScrambleMode.Numerals);
            //displayText.text = "$" + value
            //Debug.Log("changing money");
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
