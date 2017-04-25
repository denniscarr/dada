using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyFunction : D_Function {

    int _value;
    public int value
    {
        get
        {
            return _value;
        }

        set
        {
            transform.parent.name = "$" + value;
            _value = value;
        }
    }


    new private void Start()
    {
        base.Start();

        // Name game.
        value = Random.Range(10, 50);
        transform.parent.name = "$" + value;

        intSet.isOwnedByPlayer = true;
    }


    public override void Use()
    {
        base.Use();

        // If I was used by the player
        if (intSet.carryingObject.name == "Player")
        {
            // Play cha-ching sound....

            // Give player money
            GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds += value;
        }

        // If I was used by an NPC give them money.
        else if (intSet.carryingObject.GetComponentInChildren<NPC>() != null)
        {
            intSet.carryingObject.GetComponentInChildren<NPC>().funds += value;
        }

        Destroy(transform.parent.gameObject);
    }
}
