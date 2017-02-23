using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjectsManager : SimpleManager.Manager<GameObject>{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override GameObject Create(){
		GameObject g = new GameObject();
		return  g;
	}

	public override void Destroy(GameObject g){
//		e.OnDestroyed ();
//		ManagedObjects.Remove (e);
	}
}
