using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_beamGunFunction : D_Function {
	LineRenderer line;
	public GameObject spark;
	public AudioClip TreeGrowSound;

	// Use this for initialization
	new void Start () {
		base.Start ();
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();
		line.enabled = true;

		Vector3 pos = transform.position;
		Ray beamRay = new Ray (pos, -transform.forward);

		RaycastHit Hit;
		line.SetPosition (0, beamRay.origin);

        if (Physics.Raycast(beamRay, out Hit, 100))
        {
            line.SetPosition(1, Hit.point);
            if (Hit.collider.GetComponent<Rigidbody>() != null)
            {
                Hit.collider.GetComponent<Rigidbody>().AddExplosionForce(5f, Hit.point, 3f, 2f, ForceMode.Impulse);
            }

            if (Hit.collider.GetComponentInChildren<NPC>() != null)
            {
                Hit.collider.GetComponentInChildren<NPC>().health -= 10f;
            }

			Instantiate (spark, Hit.point, Quaternion.identity);
			Services.AudioManager.Play3DSFX(TreeGrowSound, Hit.point, 1f, Random.Range(0.9f, 1.1f));


        }
		else{
			line.SetPosition(1, beamRay.GetPoint(100));	
		} 

		Invoke ("BeamOff", 0.1f);
	}

	public void BeamOff() {
		line.enabled = false;
	}
	/*IEnumerator FireLaser(){
		line.enabled = true;
	}*/
}
