using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigonalPlanarMolecular : MonoBehaviour {
	private GameObject[] atom;
	private GameObject key;
	// Use this for initialization
	void Start () {
		//declear list as in "liner molecule.cs"
		atom = GameObject.Find ("Linear molecular generator").GetComponent<linearMolecule> ().atom;
		key = GameObject.Find ("Linear molecular generator").GetComponent<linearMolecule> ().key;

		//instantiate a center atom
		GameObject centerAtom = Instantiate (atom[Random.Range(0, atom.Length-1)], transform.position, Quaternion.identity);

		//instantiate bounding keys
		Vector3 key1 = new Vector3((centerAtom.transform.position.x + (centerAtom.transform.localScale.x) / 2)+0.4f, centerAtom.transform.position.y, centerAtom.transform.position.z);
		Vector3 Key2 = new Vector3 (centerAtom.transform.position.x + (centerAtom.transform.position.y + centerAtom.transform.localScale.y / 2) + 0.4f + (centerAtom.transform.position.y + centerAtom.transform.localScale.y / 2) + 0.4f);
		GameObject boundingKey1 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		GameObject boundingKey2 = Instantiate (key, key1, Quaternion.Euler(0,120,90));
		GameObject boundingKey3 = Instantiate (key, key1, Quaternion.Euler(0,-120,90));
		boundingKey1.transform.parent = centerAtom.transform;
		boundingKey2.transform.parent = centerAtom.transform;
		boundingKey3.transform.parent = centerAtom.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
