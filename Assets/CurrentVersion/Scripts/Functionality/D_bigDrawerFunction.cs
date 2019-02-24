using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_bigDrawerFunction : D_Function {
	public GameObject meteor;
	public float meteorSizeMin=0.5f;
	public float meteorSizeMax=4f;

	// Use this for initialization
	new void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use ();

		Vector3 pos = transform.position;

		float meteorScale = Random.Range (meteorSizeMin, meteorSizeMax);
		for (int i = 1; i<2; i++) 
		{
			GameObject newMeteor = Instantiate (meteor, pos, Quaternion.identity);
			newMeteor.transform.localScale = new Vector3 (meteorScale, meteorScale, meteorScale);
		}
	}

	/*void MeteorShower () {
		GameObject newMeteor = Instantiate (meteor, transform.position, Quaternion.identity);
		newMeteor.transform.localScale = new Vector3 (meteorScale, meteorScale, meteorScale);
		//Invoke ("MeteorShower1", 0.2);
	}*/
		
}
