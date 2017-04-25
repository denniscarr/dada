using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_vanillaGunFunction : D_Function {
	private LineRenderer line;
	public GameObject[] atoms;
    public GameObject[] molecules;
	public int atomsCount = 50;
	// Use this for initialization
	new void Start () {
		base.Start ();
		line = gameObject.GetComponent<LineRenderer> ();
		line.enabled = false;
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();
		line.enabled = true;

		Vector3 pos = transform.position;
		Ray beamRay = new Ray (pos, transform.forward);

		RaycastHit Hit;
		line.SetPosition (0, beamRay.origin);

		if (Physics.Raycast (beamRay, out Hit, 100)) {
            GameObject atomicHit = Hit.collider.gameObject;
            if (atomicHit.name.Contains("atom"))
            {
                print("Atom cannot be reduced");
            }
            else
            {
                Vector3 atomicBox = atomicHit.transform.localScale;
                Vector3 atomicPos = new Vector3(Random.value * atomicBox.x, Random.value * atomicBox.y, Random.value * atomicBox.z);
                atomicPos = transform.TransformPoint(atomicBox * 0.5f);
                Destroy(atomicHit);
                for (int i = 0; i < atomsCount; i++)
                {
                    Instantiate(atoms[Random.Range(0, atoms.Length - 1)], atomicPos, Quaternion.identity);
                    Instantiate(molecules[Random.Range(0, molecules.Length - 1)], atomicPos, Quaternion.identity);
                }
            }
            line.SetPosition(1, Hit.point);
        } else {
			line.SetPosition(1, beamRay.GetPoint(100));	
		}
		Invoke ("BeamOff", 0.1f);
	}

	public void BeamOff() {
		line.enabled = false;
	}
}
