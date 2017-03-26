using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_GunFunction: D_Function
{
	public GameObject projectile;
	public float muzzleVelocity = 100f;

    new void Start()
    {
        base.Start();
    }
	
	public override void Use ()
    {
        base.Use();

        Debug.Log("Used Gun");

		GameObject shoot = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
        shoot.GetComponent<Rigidbody>().AddForce(Services.Player.transform.Find("Main Camera").transform.forward * muzzleVelocity);
    }
}
