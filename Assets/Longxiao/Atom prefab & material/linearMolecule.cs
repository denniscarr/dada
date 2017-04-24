using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linearMolecule : MonoBehaviour {
	public GameObject[] atom;
	public GameObject key;
	// Use this for initialization
	void Start () {
        //Spawn an atom at the center of this molecule
		GameObject centerAtom = Instantiate (atom[Random.Range(0, atom.Length-1)], transform.position, Quaternion.identity);
		//print (centerAtom.transform.position.x);

        //Spawn bonding key at the side of the center atom along its x axis and parent them into the center atom
		Vector3 key1 = new Vector3((centerAtom.transform.position.x + (centerAtom.transform.localScale.x) / 2)+0.4f, centerAtom.transform.position.y, centerAtom.transform.position.z);
		Vector3 key2 = new Vector3((centerAtom.transform.position.x - (centerAtom.transform.localScale.x) / 2)-0.4f, centerAtom.transform.position.y, centerAtom.transform.position.z);
		GameObject boundingKey1 = Instantiate (key, key1, Quaternion.Euler(0,0,90));
		GameObject boundingKey2 = Instantiate (key, key2, Quaternion.Euler(0,0,90));
		boundingKey1.transform.parent = centerAtom.transform;
		boundingKey2.transform.parent = centerAtom.transform;
        

        //Spawn more atoms to form a linear molecule
        Vector3 side1 = new Vector3(key1.x+0.8f, key1.y, key1.z);
        Vector3 side2 = new Vector3(key2.x - 0.8f, key2.y, key2.z);
        print(side1.x);
        GameObject sideAtom1 = Instantiate (atom[Random.Range(0, atom.Length-1)], side1, Quaternion.identity);
        GameObject sideAtom2 = Instantiate(atom[Random.Range(0, atom.Length - 1)], side2, Quaternion.identity);
        sideAtom1.transform.parent = centerAtom.transform;
        sideAtom2.transform.parent = centerAtom.transform;
        Destroy(sideAtom1.GetComponent<Rigidbody>());
        Destroy(sideAtom2.GetComponent<Rigidbody>());

        print(centerAtom.transform.position.x+(centerAtom.transform.localScale.x)/2);
		print (centerAtom.transform.position.x-(centerAtom.transform.localScale.x)/2);
		//GameObject key1 = Instantiate(key, 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
