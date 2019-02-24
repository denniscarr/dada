using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_bathtubFunction : D_Function {
	public float bathtubForce = 500f;
	public float bathtubSpeed = 1000f;

    enum VibrationState { Not, GettingReady, Doing }
    VibrationState vibrationState;

    [SerializeField] private float timeUntilVibrate = 2.4f;

    [SerializeField] private float vibrationTime = 3.6f;
    private float vibrationTimer = 0f;
    
    // Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Use () {
		base.Use();

		while (transform.parent.GetComponent<Rigidbody> ().isKinematic == true) {
			transform.parent.SetParent (null);
            GetDropped();
			GetComponentInParent<Rigidbody> ().velocity = transform.forward * bathtubSpeed * Time.deltaTime;
		}

		GetComponentInParent<Rigidbody>().AddForce(transform.right * bathtubSpeed);

        vibrationState = VibrationState.GettingReady;
        vibrationTimer = 0f;

		//Invoke ("vibrate", 3f);
	}

    public override void Update() {
        base.Update();

        if (vibrationState == VibrationState.GettingReady) {
            vibrationTimer += Time.deltaTime;
            if (vibrationTimer >= timeUntilVibrate) {
                vibrationTimer = 0f;
                vibrationState = VibrationState.Doing;
            }
        }

        else if (vibrationState == VibrationState.Doing) {
            vibrationTimer += Time.deltaTime;
            if (vibrationTimer < vibrationTime) {
                Vector3 vibrateForce = Random.insideUnitSphere * bathtubForce * Time.deltaTime;
                GetComponentInParent<Rigidbody>().AddForce(vibrateForce, ForceMode.Impulse);
                transform.parent.rotation = Random.rotation;
            }
            else {
                vibrationState = VibrationState.Not;
            }
        }
    }

    void vibrate() {
		GetComponentInParent<Rigidbody> ().AddForce (transform.up * bathtubForce * Time.deltaTime);
		Invoke ("vibrate2", 0.3f);
	}

	void vibrate2() {
		GetComponentInParent<Rigidbody> ().AddForce (-transform.right * bathtubForce * Time.deltaTime);
		Invoke ("vibrate3", 0.3f);
	}

	void vibrate3() {
		GetComponentInParent<Rigidbody> ().AddForce (-transform.forward * bathtubForce * Time.deltaTime);
	}
}
