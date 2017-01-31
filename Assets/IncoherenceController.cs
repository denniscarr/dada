using UnityEngine;
using System.Collections;

public class IncoherenceController : MonoBehaviour {

	// 'Incoherence' is expressed in various ways depending on the inherited. Ranges from 0 to 1.
	public float incoherence = 0.0f;

	// How often incoherence is expressed (seconds)
	public float interval = 1f;

	// Seconds since last interval (used as a timer)
	float elapsedSeconds = 0.0f;

	void Update()
	{
		Debug.Log("Doing It");

		// See if it is time to express incoherence
		elapsedSeconds += Time.deltaTime;

		if (elapsedSeconds > interval) {
			BroadcastMessage("ExpressIncoherence", incoherence);
			elapsedSeconds = 0.0f;
		}
	}

	void IncreaseIncoherence(float increaseAmt) {
		incoherence += increaseAmt;
	}
}
