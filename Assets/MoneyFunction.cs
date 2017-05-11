using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyFunction : D_Function {
    
    // How much I'm worth.
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

    bool collisionWithPlayer;   // Set to true when I collide with the player.

    [SerializeField] GameObject gainParticles;


    new private void Start()
    {
        base.Start();

        // Name game.
        value = Random.Range(10, 50);
        transform.parent.name = "$" + value;

        intSet.isOwnedByPlayer = true;
    }


    new private void Update()
    {
        if (intSet.carryingObject != null && intSet.carryingObject.name == "Player")
        {
            Use();
        }
    }


    public override void Use()
    {
        base.Use();

        // If I was used by the player
        if ((intSet.carryingObject != null && intSet.carryingObject.name == "Player") || collisionWithPlayer)
        {
            // Give player money
            GameObject.Find("Bootstrapper").GetComponent<PlayerMoneyManager>().funds += value;
        }

        // If I was used by an NPC give them money.
        else if (intSet.carryingObject.GetComponentInChildren<NPC>() != null)
        {
            intSet.carryingObject.GetComponentInChildren<NPC>().funds += value;
        }

        Instantiate(gainParticles, transform.parent.position, Quaternion.Euler(270f, 0f, 0f));
        GameObject.Find("Money Display").GetComponent<Animator>().SetTrigger("FlashGreen");

        Destroy(transform.parent.gameObject);
    }


    void OnTriggerEnterParent(Collider other)
    {
        if (other.transform == Services.Player.transform)
        {
            Debug.Log("Money collided with player");
            collisionWithPlayer = true;
            Use();
        }
    }
}
