using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This incoherence adds explosion force to the object's rigidbody at random intervals.

public class RigidbodyIncoherence : Incoherence {

	Rigidbody rb;

	// The force applied for each explosion.
	public float forceMin;
	public float forceMax;

	// The radius of each explosion.
	public float explosionRangeMin;
	public float explosionRangeMax;

	SavedRigidbodyValues savedRigidbodyValues;


	void Start () {
		base.Start();

		savedRigidbodyValues = new SavedRigidbodyValues ();

		rb = transform.parent.parent.GetComponentInChildren<Rigidbody>();
	}

//	void FixedUpdate() {
//		ChangeRandomValue ();
//		CallRandomFunction ();
//	}


	public override void ExpressTimedIncoherence(float magnitude) {

        if (rb == null) return;

		base.ExpressTimedIncoherence(magnitude);
		CallRandomFunction ();
		ChangeRandomValue ();
	}

	ForceMode RandomForceMode() {
		ForceMode fm = new ForceMode ();
		int rand2 = Random.Range(0, 3);
		if (rand2 == 0) {
			fm = ForceMode.Acceleration;
		} else if (rand2 == 1) {
			fm = ForceMode.Force;
		} else if (rand2 == 2) {
			fm = ForceMode.Impulse;
		} else if (rand2 == 3) {
			fm = ForceMode.VelocityChange;
		}
		return fm;
	}

	void CallRandomFunction() {

		int rand = Random.Range (0, 5);

		if (rand == 0) {
			float mag = MapIncoherenceMagnitude (0f, 1000f);
			float mag1 = MapIncoherenceMagnitude (0f, 2.5f);
			float mag2 = MapIncoherenceMagnitude (0f, 10f);
			Collider[] cols = Physics.OverlapSphere (transform.position + Random.insideUnitSphere * Random.Range (-mag1, mag1), Random.Range (-mag2, mag2));
			foreach (Collider col in cols) {
				if (col.attachedRigidbody == null) {
					return;
				}
				col.attachedRigidbody.AddExplosionForce (Random.Range (-mag, mag), Random.insideUnitSphere * Random.Range (-mag1, mag1), Random.Range (-mag2, mag2), Random.Range (-mag2, mag2));
			}
		} else if (rand == 1) {
			float mag = MapIncoherenceMagnitude(0f, 100f);
			rb.AddForce(Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());
		} else if (rand == 2) {
			float mag = MapIncoherenceMagnitude(0f, 100f);
			float mag2 = MapIncoherenceMagnitude (-2.5f, 2.5f);
			rb.AddForceAtPosition(Random.insideUnitSphere*Random.Range(-mag, mag), Random.insideUnitSphere*Random.Range(-mag2, mag2), RandomForceMode());
		} else if (rand == 3) {
			float mag = MapIncoherenceMagnitude(0f, 100f);
			rb.AddRelativeForce (Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());
		} else if (rand == 4) {
			float mag = MapIncoherenceMagnitude(0f, 100f);
			rb.AddRelativeTorque(Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());	
		} else if (rand == 5) {
			float mag = MapIncoherenceMagnitude(0f, 100f);
			rb.AddTorque(Random.insideUnitSphere*Random.Range(-mag, mag), RandomForceMode());
		}
	}


	void ChangeRandomValue()
	{
		int rand = Random.Range (0, savedRigidbodyValues.numberOfValues);
		float magFactor;

		if (rand == 0) {
			rb.angularDrag += ModifyFloat (0.1f);
		} else if (rand == 1) {
			rb.angularVelocity += ModifyVector3 (90f);
		} else if (rand == 2) {
			rb.centerOfMass += ModifyVector3 (1f);
		} else if (rand == 3) {
			int rand2 = Random.Range (0, 2);
			if (rand2 == 0) {
				rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			} else if (rand2 == 1) {
				rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			} else if (rand2 == 2) {
				rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
			}
		} else if (rand == 4) {
			int rand2 = Random.Range (0, 9);
			if (rand2 == 0) {
				rb.constraints = RigidbodyConstraints.FreezeAll;
			} else if (rand2 == 1) {
				rb.constraints = RigidbodyConstraints.FreezePosition;
			} else if (rand2 == 2) {
				rb.constraints = RigidbodyConstraints.FreezePositionX;
			} else if (rand2 == 3) {
				rb.constraints = RigidbodyConstraints.FreezePositionY;
			} else if (rand2 == 4) {
				rb.constraints = RigidbodyConstraints.FreezePositionZ;
			} else if (rand2 == 5) {
				rb.constraints = RigidbodyConstraints.FreezeRotation;
			} else if (rand2 == 6) {
				rb.constraints = RigidbodyConstraints.FreezeRotationX;
			} else if (rand2 == 7) {
				rb.constraints = RigidbodyConstraints.FreezeRotationY;
			} else if (rand2 == 8) {
				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			} else if (rand2 == 9) {
				rb.constraints = RigidbodyConstraints.None;
			}
		} else if (rand == 5) {
//			rb.detectCollisions = ModifyBool (rb.detectCollisions);
		} else if (rand == 6) {
			rb.drag += ModifyFloat (20f);
		} else if (rand == 7) {
			rb.freezeRotation = ModifyBool (rb.freezeRotation);
		} else if (rand == 9) {
			rb.inertiaTensor += ModifyVector3 (1f, true);
		} else if (rand == 10) {
			rb.inertiaTensorRotation = ModifyQuaternion (rb.inertiaTensorRotation.eulerAngles, 1f, true);
		} else if (rand == 11) {
			int rand2 = Random.Range (0, 2);
			if (rand2 == 0) {
				rb.interpolation = RigidbodyInterpolation.Extrapolate;
			} else if (rand2 == 1) {
				rb.interpolation = RigidbodyInterpolation.Interpolate;
			} else if (rand2 == 2) {
				rb.interpolation = RigidbodyInterpolation.None;
			}
		} else if (rand == 12) {
			rb.isKinematic = ModifyBool (rb.isKinematic);
		} else if (rand == 13) {
			rb.mass += ModifyFloat (100f);
		} else if (rand == 14) {
			rb.maxAngularVelocity += ModifyFloat (250f);
		} else if (rand == 15) {
			rb.maxDepenetrationVelocity += ModifyFloat (250f);
		} else if (rand == 16) {
			rb.sleepThreshold += ModifyFloat (10f);
		} else if (rand == 17) {
			rb.useGravity = ModifyBool (rb.useGravity);
		} else if (rand == 18) {
			rb.velocity += ModifyVector3 (100f);
		}

		UpdateSavedValues ();
	}


	public void UpdateSavedValues()
	{
		// Copy over all values from my the rigidbody component
		savedRigidbodyValues.angularDrag = rb.angularDrag;
		savedRigidbodyValues.angularVelocity = rb.angularVelocity;
		savedRigidbodyValues.centerOfMass = rb.centerOfMass;
		savedRigidbodyValues.collisionDetectionMode = rb.collisionDetectionMode;
		savedRigidbodyValues.constraints = rb.constraints;
		savedRigidbodyValues.detectCollisions = rb.detectCollisions;
		savedRigidbodyValues.drag = rb.drag;
		savedRigidbodyValues.freezeRotation = rb.freezeRotation;
		savedRigidbodyValues.inertiaTensor = rb.inertiaTensor;
		savedRigidbodyValues.inertiaTensorRotation = rb.inertiaTensorRotation;
		savedRigidbodyValues.interpolation = rb.interpolation;
		savedRigidbodyValues.isKinematic = rb.isKinematic;
		savedRigidbodyValues.mass = rb.mass;
		savedRigidbodyValues.maxAngularVelocity = rb.maxAngularVelocity;
		savedRigidbodyValues.maxDepenetrationVelocity = rb.maxDepenetrationVelocity;
		savedRigidbodyValues.sleepThreshold = rb.sleepThreshold;
		savedRigidbodyValues.useGravity = rb.useGravity;
		savedRigidbodyValues.velocity = rb.velocity;
	}

	public class SavedRigidbodyValues {

		public float angularDrag;
		public Vector3 angularVelocity;
		public Vector3 centerOfMass;
		public CollisionDetectionMode collisionDetectionMode;
		public RigidbodyConstraints constraints;
		public bool detectCollisions;
		public float drag;
		public bool freezeRotation;
		public Vector3 inertiaTensor;
		public Quaternion inertiaTensorRotation;
		public RigidbodyInterpolation interpolation;
		public bool isKinematic;
		public float mass;
		public float maxAngularVelocity;
		public float maxDepenetrationVelocity;
		public float sleepThreshold;
		public bool useGravity;
		public Vector3 velocity;

		public int numberOfValues = 18;

		public SavedRigidbodyValues() {
			
		}
	}
}
