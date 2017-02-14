using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceIncoherence : Incoherence {

	public float maxShift = 0.1f;

	List<AudioSource> audioSources;
	List<SavedAudioSourceValues> savedAudioSourceValues;

	void Start () {
		base.Start();

		audioSources = new List<AudioSource> ();
		foreach (AudioSource aud in transform.parent.GetComponentsInChildren<AudioSource>()) {
			audioSources.Add (aud);
			savedAudioSourceValues.Add (new SavedAudioSourceValues ());
		}
	}

	//	void FixedUpdate() {
	//		ChangeRandomValue ();
	//		CallRandomFunction ();
	//	}


	public override void ExpressTimedIncoherence(float magnitude) {
//		base.ExpressTimedIncoherence(magnitude);
//		ChangeRandomValue ();
	}


	void CallRandomFunction() {

//		int rand = Random.Range (0, 2);
//		if (rand == 0 && myController.incoherenceMagnitude > 0.9f) {
//			grandparent.DetachChildren ();
//		} else if (rand == 1 && myController.incoherenceMagnitude > 0.9f) {
//			grandparent.SetAsFirstSibling ();
//		} else if (rand == 2 && myController.incoherenceMagnitude > 0.9f) {
//			grandparent.SetAsLastSibling ();
//		}
	}


	void ChangeRandomValue()
	{
//		if (myController.incoherenceMagnitude < 0.5f)
//			return;
//
//		int rand = Random.Range (0, savedAudioSourceValues.numberOfValues);
//		float magFactor;
//
//		if (rand == 0) {
//			grandparent.eulerAngles += ModifyVector3 (maxShift);
//		} else if (rand == 1) {
//			grandparent.localEulerAngles += ModifyVector3 (maxShift);
//		} else if (rand == 2) {
//			grandparent.forward += ModifyVector3 (maxShift);
//		} else if (rand == 3) {
//			grandparent.right += ModifyVector3 (maxShift);
//		} else if (rand == 4) {
//			if (myController.incoherenceMagnitude < 0.5f)
//				return;
//			grandparent.hasChanged = ModifyBool (grandparent.hasChanged);
//		} else if (rand == 5) {
//			//			grandparent.position = ModifyVector3 (maxShift*0.3f);
//		} else if (rand == 6) {
//			//			grandparent.localPosition = ModifyVector3 (maxShift*0.3f);
//		} else if (rand == 7) {
//			grandparent.localScale = ModifyVector3 (maxShift);
//		} else if (rand == 8) {
//			if (myController.incoherenceMagnitude < 0.5f)
//				return;
//			GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject> ();
//			int rand2 = Random.Range (0, allGameObjects.Length);
//			if (allGameObjects [rand2].tag == "MainCamera" || allGameObjects [rand2].transform.root.name.Contains("Player")) {
//				return;
//			}
//			grandparent.parent = allGameObjects [rand2].transform;
//		} else if (rand == 9) {
//			grandparent.rotation = ModifyQuaternion (grandparent.rotation.eulerAngles, maxShift);
//		} else if (rand == 10) {
//			grandparent.hierarchyCapacity += Random.Range (-1, 1);
//		}
//
//		UpdateSavedValues ();
	}


	public void UpdateSavedValues()
	{
		// Copy over all values from my the rigidbody component
//		savedAudioSourceValues.eulerAngles = grandparent.eulerAngles;
//		savedAudioSourceValues.localEulerAngles = grandparent.localEulerAngles;
//		savedAudioSourceValues.forward = grandparent.forward;
//		savedAudioSourceValues.right = grandparent.right;
//		savedAudioSourceValues.hasChanged = grandparent.hasChanged;
//		savedAudioSourceValues.position = grandparent.position;
//		savedAudioSourceValues.localPosition = grandparent.localPosition;
//		savedAudioSourceValues.localScale = grandparent.localScale;
//		savedAudioSourceValues.parent = grandparent.parent;
//		savedAudioSourceValues.rotation = grandparent.rotation;
//		savedAudioSourceValues.hierarchyCapacity = grandparent.hierarchyCapacity;
	}

	public class SavedAudioSourceValues {

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

		public SavedAudioSourceValues() {

		}
}
