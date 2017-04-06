using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddJoints : MonoBehaviour {

	public bool gravity, collisions = true;
	// Use this for initialization
	void Start () {
		foreach(Transform t in gameObject.GetComponentsInChildren<Transform>()){
			if (t.gameObject.GetComponent<Rigidbody> () == null) {
				t.gameObject.AddComponent<Rigidbody> ().useGravity = gravity;
			}

			t.GetComponent<Rigidbody> ().drag = 3;
			t.GetComponent<Rigidbody> ().mass = 1f;
		}

		AssignComponents (this.gameObject);
	}

	void AssignComponents(GameObject g){
		if (g.transform.childCount != 0) {
			foreach (Transform t in g.GetComponentsInChildren<Transform>()) {
				if (t.parent == g.transform) {
					AssignComponents (t.gameObject);
				}
			}
		}

		if (g.GetComponent<CharacterJoint> () != null) {
			g.GetComponent<CharacterJoint> ().breakForce = 1000;
		}
		if (g.transform.parent.GetComponent<Rigidbody>() != null) {
			g.AddComponent<CharacterJoint> ().connectedBody = g.transform.parent.GetComponent<Rigidbody> ();
			CharacterJoint j = g.GetComponent<CharacterJoint> ();
			SoftJointLimitSpring s = new SoftJointLimitSpring ();
			s.damper = 100;
			s.spring = 500;

			SoftJointLimit a = new SoftJointLimit();
			SoftJointLimit b = new SoftJointLimit();
			a.limit = -5;
			b.limit = 5;

			j.twistLimitSpring = s;
			j.swingLimitSpring = s;

			j.lowTwistLimit = a;
			j.highTwistLimit = b;
			j.swing1Limit = a;
			j.swing2Limit = b;
//			g.GetComponent<SpringJoint> ().damper = 1;
//			g.GetComponent<SpringJoint> ().spring = 10;


//			JointSpring d = new JointSpring ();
//
//			g.GetComponent<HingeJoint> ().spring = d;
//			g.GetComponent<HingeJoint> ().useSpring = true;
		}

		g.AddComponent<CapsuleCollider> ();
		CapsuleCollider c = g.GetComponent<CapsuleCollider> ();
		c.direction = 0;
//		c.radius = 1f;
		c.height = 2;
		c.isTrigger = !collisions;

		Collider[] cols= Physics.OverlapSphere(g.transform.position, c.radius);

		bool stop = false;

		while(cols.Length > 1 && !stop){

//			foreach (SphereCollider s in cols) {
//				s.radius -= 0.1f;
//			}
			c.radius -= 0.1f;
			cols = Physics.OverlapSphere (g.transform.position, c.radius);
				
			if(cols.Length == 1 || c.radius < 0.1f){
				c.isTrigger = true;
				stop = true;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
