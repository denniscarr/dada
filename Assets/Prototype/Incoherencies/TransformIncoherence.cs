using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This incoherence adds explosion force to the object's rigidbody at random intervals.

public class TransformIncoherence : Incoherence {

	SavedTransformValues savedTransformValues;

	public float maxShift = 0.1f;

	// My parent's parent
	Transform grandparent;

	void Start () {
		base.Start();
		savedTransformValues = new SavedTransformValues ();
		grandparent = transform.parent.parent;
	}

//	void FixedUpdate() {
//		ChangeRandomValue ();
//		CallRandomFunction ();
//	}


	public override void ExpressTimedIncoherence(float magnitude) {
		base.ExpressTimedIncoherence(magnitude);
		ChangeRandomValue ();
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
		if (myController.incoherenceMagnitude < 0.5f)
			return;

		int rand = Random.Range (0, savedTransformValues.numberOfValues);
		float magFactor;

		if (rand == 0) {
			grandparent.eulerAngles += ModifyVector3 (maxShift);
		} else if (rand == 1) {
			grandparent.localEulerAngles += ModifyVector3 (maxShift);
		} else if (rand == 2) {
			grandparent.forward += ModifyVector3 (maxShift);
		} else if (rand == 3) {
			grandparent.right += ModifyVector3 (maxShift);
		} else if (rand == 4) {
			if (myController.incoherenceMagnitude < 0.5f)
				return;
			grandparent.hasChanged = ModifyBool (grandparent.hasChanged);
		} else if (rand == 5) {
//			grandparent.position = ModifyVector3 (maxShift*0.3f);
		} else if (rand == 6) {
//			grandparent.localPosition = ModifyVector3 (maxShift*0.3f);
		} else if (rand == 7) {
			grandparent.localScale = ModifyVector3 (maxShift);
		} else if (rand == 8) {
			if (myController.incoherenceMagnitude < 0.5f)
				return;
			GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject> ();
			int rand2 = Random.Range (0, allGameObjects.Length);
			if (allGameObjects [rand2].tag == "MainCamera" || allGameObjects [rand2].transform.root.name.Contains("Player")) {
				return;
			}
			grandparent.parent = allGameObjects [rand2].transform;
		} else if (rand == 9) {
			grandparent.rotation = ModifyQuaternion (grandparent.rotation.eulerAngles, maxShift);
		} else if (rand == 10) {
			grandparent.hierarchyCapacity += Random.Range (-1, 1);
		}

		UpdateSavedValues ();
	}


	public void UpdateSavedValues()
	{
		// Copy over all values from my the rigidbody component
		savedTransformValues.eulerAngles = grandparent.eulerAngles;
		savedTransformValues.localEulerAngles = grandparent.localEulerAngles;
		savedTransformValues.forward = grandparent.forward;
		savedTransformValues.right = grandparent.right;
		savedTransformValues.hasChanged = grandparent.hasChanged;
		savedTransformValues.position = grandparent.position;
		savedTransformValues.localPosition = grandparent.localPosition;
		savedTransformValues.localScale = grandparent.localScale;
		savedTransformValues.parent = grandparent.parent;
		savedTransformValues.rotation = grandparent.rotation;
		savedTransformValues.hierarchyCapacity = grandparent.hierarchyCapacity;
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
