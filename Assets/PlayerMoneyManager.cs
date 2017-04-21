using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour {

    int _funds = 999;
    public int funds
    {
        get { return _funds; }
        set
        {
            if (value < 0) value = 0;
            _funds = value;
        }
    }
}
