using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_GunFunction: D_Function
{
	public GameObject[] projectile;
	public float muzzleVelocity = 100f;

    new void Start()
    {
        base.Start();
    }
	
	public override void Use ()
    {
        base.Use();

        Debug.Log("Used Gun");
		Vector3 pos = LOWER_EQUIP_REFERENCE.position + intSet.equipPosition + transform.localPosition;
		GameObject shoot = Instantiate (projectile[Random.Range (0, projectile.Length)], pos, Quaternion.identity) as GameObject;
		shoot.GetComponent<Rigidbody>().AddForce(t_player.forward * muzzleVelocity);
    }
}
