using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyFunction : D_Function {

    int value;


    new private void Start()
    {
        base.Start();

        // Name game.
        value = Random.Range(50, 100);
        transform.parent.name = "$" + value;

        intSet.isOwnedByPlayer = true;
    }


    public override void Use()
    {
        base.Use();

        if (intSet.carryingObject.name == "Player")
        {
            // Play cha-ching sound....
            Debug.Log("cha ching");

            // Give player money
            GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds += value;
        }

        Destroy(transform.parent.gameObject);
    }
}
