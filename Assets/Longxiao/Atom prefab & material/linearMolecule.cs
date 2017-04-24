using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linearMolecule : MonoBehaviour {
	public GameObject[] atom;
	public GameObject key;
	// Use this for initialization
	void Start () {
		GameObject centerAtom = Instantiate (atom[Random.Range(0, atom.Length-1)], transform.position, Quaternion.identity);
		//print (centerAtom.transform.position.x);

		Vector3 key1 = new Vector3((centerAtom.transform.position.x + (centerAtom.transform.localScale.x) / 2), centerAtom.transform.position.y, centerAtom.transform.position.z);
		Vector3 key2 = new Vector3((centerAtom.transform.position.x - (centerAtom.transform.localScale.x) / 2), centerAtom.transform.position.y, centerAtom.transform.position.z);
		GameObject boundingKey1 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		GameObject boundingKey2 = Instantiate (key, key2, Quaternion.Euler(0,0,90));
		boundingKey1.transform.parent = centerAtom.transform;
		boundingKey2.transform.parent = centerAtom.transform;

		//GameObject 
		print (centerAtom.transform.position.x+(centerAtom.transform.localScale.x)/2);
		print (centerAtom.transform.position.x-(centerAtom.transform.localScale.x)/2);
		//GameObject key1 = Instantiate(key, 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
