using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triagonalPlanarMolecule : MonoBehaviour {
	private GameObject[] atom;
	private GameObject key;
	// Use this for initialization
	void Start () {
		//Spawn the center atom
		atom = GameObject.Find ("Linear molecular generator").GetComponent<linearMolecule> ().atom;
		GameObject centerAtom = Instantiate (atom[Random.Range(0, atom.Length-1)], transform.position, Quaternion.identity);

		//Spawn bounding keys around the center atom
		key = GameObject.Find ("Linear molecular generator").GetComponent<linearMolecule> ().key;
		Vector3 key1 = new Vector3((centerAtom.transform.position.x + (centerAtom.transform.localScale.x) / 2)+0.4f, centerAtom.transform.position.y, centerAtom.transform.position.z);
		GameObject boundingKey1 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		GameObject boundingKey2 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		GameObject boundingKey3 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		boundingKey1.transform.parent = centerAtom.transform;
		boundingKey2.transform.parent = centerAtom.transform;
		boundingKey3.transform.parent = centerAtom.transform;
		boundingKey2.transform.RotateAround (centerAtom.transform.position, centerAtom.transform.up, 120);
		boundingKey3.transform.RotateAround (centerAtom.transform.position, centerAtom.transform.up, 240);

		//Spawn surrounding atoms
		int num = Random.Range (0, atom.Length - 1);
		GameObject sideAtom1 = Instantiate (atom[num], key1, Quaternion.identity);
		GameObject sideAtom2 = Instantiate (atom[num], boundingKey2.transform.position, Quaternion.identity);
		GameObject sideAtom3 = Instantiate (atom[num], boundingKey3.transform.position, Quaternion.identity);
		Destroy(sideAtom1.GetComponent<Rigidbody>());
		Destroy(sideAtom2.GetComponent<Rigidbody>());
		Destroy(sideAtom3.GetComponent<Rigidbody>());
		sideAtom1.transform.position =new Vector3 (key1.x + sideAtom1.transform.localScale.x/2, key1.y, key1.z);
		sideAtom2.transform.position =new Vector3 (boundingKey2.transform.position.x - sideAtom2.transform.localScale.x/2, boundingKey2.transform.position.y, boundingKey2.transform.position.z - sideAtom2.transform.localScale.z/2);
		sideAtom3.transform.position = new Vector3 (boundingKey3.transform.position.x - sideAtom3.transform.localScale.x / 2, boundingKey3.transform.position.y, boundingKey3.transform.position.z + sideAtom3.transform.localScale.z / 2);
		sideAtom1.transform.parent = centerAtom.transform;
		sideAtom2.transform.parent = centerAtom.transform;
		sideAtom3.transform.parent = centerAtom.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
