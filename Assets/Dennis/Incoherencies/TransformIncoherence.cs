using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This incoherence adds explosion force to the object's rigidbody at random intervals.

public class TransformIncoherence : Incoherence {

	SavedTransformValues savedTransformValues;


	void Start () {
		base.Start();
		savedTransformValues = new SavedTransformValues ();
	}

	void FixedUpdate() {
		ChangeRandomValue ();
		CallRandomFunction ();
	}


	public override void ExpressTimedIncoherence(float magnitude) {
		base.ExpressTimedIncoherence(magnitude);
	}
		

	void CallRandomFunction() {
//
//		int rand = Random.Range (0, 5);
//
//		if (rand == 0) {
//			float mag = MapIncoherenceMagnitude (0f, 1000f);
//			float mag1 = MapIncoherenceMagnitude (0f, 2.5f);
//			float mag2 = MapIncoherenceMagnitude (0f, 10f);
//			Collider[] cols = Physics.OverlapSphere (transform.position + Random.insideUnitSphere * Random.Range (-mag1, mag1), Random.Range (-mag2, mag2));
//			foreach (Collider col in cols) {
//				if (col.attachedRigidbody == null) {
//					return;
//				}
//				col.attachedRigidbody.AddExplosionForce (Random.Range (-mag, mag), Random.insideUnitSphere * Random.Range (-mag1, mag1), Random.Range (-mag2, mag2), Random.Range (-mag2, mag2));
//			}
//		} else if (rand == 1) {
//			float mag = MapIncoherenceMagnitude(0f, 100f);
//			rb.AddForce(Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());
//		} else if (rand == 2) {
//			float mag = MapIncoherenceMagnitude(0f, 100f);
//			float mag2 = MapIncoherenceMagnitude (-2.5f, 2.5f);
//			rb.AddForceAtPosition(Random.insideUnitSphere*Random.Range(-mag, mag), Random.insideUnitSphere*Random.Range(-mag2, mag2), RandomForceMode());
//		} else if (rand == 3) {
//			float mag = MapIncoherenceMagnitude(0f, 100f);
//			rb.AddRelativeForce (Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());
//		} else if (rand == 4) {
//			float mag = MapIncoherenceMagnitude(0f, 100f);
//			rb.AddRelativeTorque(Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());	
//		} else if (rand == 5) {
//			float mag = MapIncoherenceMagnitude(0f, 100f);
//			rb.AddTorque(Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());
//		}
	}


	void ChangeRandomValue()
	{
		int rand = Random.Range (0, savedTransformValues.numberOfValues);
		float magFactor;

		if (rand == 0) {
			transform.root.eulerAngles += ModifyVector3 (10f);
		} else if (rand == 1) {
			transform.root.localEulerAngles += ModifyVector3 (10f);
		} else if (rand == 2) {
			transform.root.forward += ModifyVector3 (10f);
		} else if (rand == 3) {
			transform.root.right += ModifyVector3 (10f);
		} else if (rand == 4) {
			transform.root.hasChanged = ModifyBool (transform.hasChanged);
		} else if (rand == 5) {
			transform.root.position = ModifyVector3 (2.5f);
		} else if (rand == 6) {
			transform.root.localPosition = ModifyVector3 (2.5f);
		} else if (rand == 7) {
			transform.root.localScale = ModifyVector3 (1.5f);
		} else if (rand == 8) {
			GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject> ();
			int rand2 = Random.Range (0, allGameObjects.Length);
			if (allGameObjects [rand2].tag == "MainCamera" || allGameObjects [rand2].tag == "Player") {
				return;
			}
			transform.root.parent = allGameObjects [rand2].transform;
		} else if (rand == 9) {
			transform.root.rotation = ModifyQuaternion (transform.rotation.eulerAngles, 1.5f);
		} else if (rand == 10) {
			transform.root.hierarchyCapacity += Random.Range (-1, 1);
		}

		UpdateSavedValues ();
	}


	public void UpdateSavedValues()
	{
		// Copy over all values from my the rigidbody component
		savedTransformValues.eulerAngles = transform.root.eulerAngles;
		savedTransformValues.localEulerAngles = transform.root.localEulerAngles;
		savedTransformValues.forward = transform.root.forward;
		savedTransformValues.right = transform.root.right;
		savedTransformValues.hasChanged = transform.root.hasChanged;
		savedTransformValues.position = transform.root.position;
		savedTransformValues.localPosition = transform.root.localPosition;
		savedTransformValues.localScale = transform.root.localScale;
		savedTransformValues.parent = transform.root.parent;
		savedTransformValues.rotation = transform.root.rotation;
		savedTransformValues.hierarchyCapacity = transform.root.hierarchyCapacity;
	}

	public class SavedTransformValues {

		public Vector3 eulerAngles;
		public Vector3 localEulerAngles;
		public Vector3 forward;
		public Vector3 right;
		public bool hasChanged;
		public Vector3 position;
		public Vector3 localPosition;
		public Vector3 localScale;
		public Transform parent;
		public Quaternion rotation;
		public int hierarchyCapacity;

		public int numberOfValues = 10;

		public SavedTransformValues() {
			
		}
	}
}
