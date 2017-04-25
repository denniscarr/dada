using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triagonalPlanarMolecule : MonoBehaviour {
	private GameObject[] atom;
	private GameObject key;
	// Use this for initialization
	void Start () {
		//Spawn the center atom
		atom = GameObject.Find ("Vanilla's gun function").GetComponent<D_vanillaGunFunction> ().atoms;
		GameObject centerAtom = Instantiate (atom[Random.Range(0, atom.Length-1)], transform.position, Quaternion.identity);

		//Spawn bounding keys around the center atom
		key = GameObject.Find ("Vanilla's gun function").GetComponent<D_vanillaGunFunction> ().key;
		Vector3 key1 = centerAtom.transform.position + new Vector3(((centerAtom.transform.localScale.x) / 2)+0.4f, 0, 0);
		GameObject boundingKey1 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		GameObject boundingKey2 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		GameObject boundingKey3 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		boundingKey1.transform.parent = centerAtom.transform;
		boundingKey2.transform.parent = centerAtom.transform;
		boundingKey3.transform.parent = centerAtom.transform;
		boundingKey2.transform.RotateAround (centerAtom.transform.position, centerAtom.transform.up, 120);
		boundingKey3.transform.RotateAround (centerAtom.transform.position, centerAtom.transform.up, 240);

		//Spawn surrounding atoms
		//int num = Random.Range (0, atom.Length - 1);
		GameObject sideAtom1 = Instantiate (atom[Random.Range (0, atom.Length - 1)]);
		GameObject sideAtom2 = Instantiate (atom[Random.Range (0, atom.Length - 1)]);
		GameObject sideAtom3 = Instantiate (atom[Random.Range (0, atom.Length - 1)]);
		Destroy(sideAtom1.GetComponent<Rigidbody>());
		Destroy(sideAtom2.GetComponent<Rigidbody>());
		Destroy(sideAtom3.GetComponent<Rigidbody>());
		sideAtom1.transform.position = key1 + new Vector3 (sideAtom1.transform.localScale.x/2, 0, 0);
		sideAtom2.transform.position = boundingKey2.transform.position + -boundingKey2.transform.up*sideAtom2.transform.localScale.x/2f;//new Vector3 ( - sideAtom2.transform.localScale.x/2, 0, -sideAtom2.transform.localScale.z / 2);
		sideAtom3.transform.position = boundingKey3.transform.position + -boundingKey3.transform.up*sideAtom3.transform.localScale.x/2f;// new Vector3 ( - sideAtom3.transform.localScale.x/2, 0, sideAtom3.transform.localScale.z / 2);
		sideAtom1.transform.SetParent(centerAtom.transform, true);// centerAtom.transform;
		sideAtom2.transform.SetParent(centerAtom.transform, true);//parent = centerAtom.transform;
		sideAtom3.transform.SetParent(centerAtom.transform, true);//parent = centerAtom.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
