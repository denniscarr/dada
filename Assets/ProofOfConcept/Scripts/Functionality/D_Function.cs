using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Function : MonoBehaviour {

	protected InteractionSettings intSet;
    KeyCode useKey = KeyCode.Mouse1;
	public AudioClip[] useSFX;
	public float audioJitter = 0f;
    float cooldownTimer = 0.2f;
    float currentCooldown = 0.2f;
	protected Transform LOWER_EQUIP_REFERENCE;
	protected Transform t_player;

    public void Start()
    {
		LOWER_EQUIP_REFERENCE = GameObject.Find("Equip Reference").transform;
		t_player = GameObject.Find("Player").transform;
        intSet = transform.parent.GetComponentInChildren<InteractionSettings>();
    }

    public void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (currentCooldown <= 0)
        {
            //if (intSet.carryingObject == Services.Player.transform)
            //{
            //    Debug.Log("Held by player");
            //}

            // If we're being carried by the player and the player presses the use key then get used.
            if (intSet.
                carryingObject != null 
                && intSet.carryingObject == 
                Services.Player.transform && 
                Input.GetKey(useKey))
            {
                Use();
            }

            currentCooldown = cooldownTimer;
        }
    }

	public void ResetTransformOnTheGround(){
		transform.parent.position = LOWER_EQUIP_REFERENCE.position + intSet.equipPosition;

		transform.parent.rotation = Quaternion.LookRotation(t_player.forward,t_player.up);

	}

    public virtual void Use()
    {
		float pitchJitter = (Random.value - 0.5f) * audioJitter + 1f;
        if (useSFX.Length > 0)
        {
            Services.AudioManager.Play3DSFX(useSFX[Random.Range(0, useSFX.Length)], transform.position, 1f, pitchJitter);
        }
    }

    protected void GetDropped()
    {
        if (intSet.carryingObject != null && intSet.carryingObject == Services.Player.transform)
            Services.Player.BroadcastMessage("AbandonItem");

        else if (transform.parent.parent != null && transform.parent.parent.name == "UpperNode")
        {
            Services.Player.transform.parent.BroadcastMessage("StopHoldingItemInMouse", SendMessageOptions.DontRequireReceiver);
        }

        //else
        //{
        transform.parent.SetParent(null);

            // Re-enable collision & stuff.
            transform.parent.GetComponent<Collider>().enabled = true;
            if (transform.parent.GetComponent<Rigidbody>() != null) transform.parent.GetComponent<Rigidbody>().isKinematic = false;
            intSet.carryingObject = null;
        //}
    }
}
