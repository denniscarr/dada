using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyScript : MonoBehaviour {

	Transform parentObject;
	
	void OnDestroy(){

		parentObject = this.gameObject.GetComponentInParent<Transform> ();

		GameObject fieryGlow = Instantiate(Resources.Load ("questobject-fire", typeof (GameObject))) as GameObject;
		fieryGlow.transform.parent = parentObject.transform;
	}

}
