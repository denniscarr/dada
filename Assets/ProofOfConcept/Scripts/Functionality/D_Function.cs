using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Function : MonoBehaviour {

    public InteractionSettings intSet;
    public KeyCode useKey = KeyCode.Mouse0;
	public AudioClip[] useSFX;
	public float audioJitter = 0f;
    float cooldownTimer = 0.5f;
    float currentCooldown = 0.0f;

    public void Start()
    {
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
            if (intSet.carryingObject == Services.Player.transform)
            {
                Debug.Log("Held by player");
            }

            // If we're being carried by the player and the player presses the use key then get used.
            if (intSet.carryingObject == Services.Player.transform && Input.GetKey(useKey))
            {
                Use();
            }

            currentCooldown = cooldownTimer;
        }
    }

    public virtual void Use()
    {
//		float pitchJitter = (Random.value - 0.5f) * audioJitter + 1f;
//		Services.AudioManager.Play3DSFX(useSFX[Random.Range(0,useSFX.Length-1)], transform.position, 1f, pitchJitter);
    }
}
