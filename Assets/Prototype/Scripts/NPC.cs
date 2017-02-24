using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {

	NavMeshAgent navMeshAgent;
	NPCAnimation npcAnimation;

	// Behavior states
	int currentState;
	const int GET_TARGET_POSITION = 0;
	const int MOVE_TO_TARGET_POSITION = 1;
	const int MOVE_TOWARDS_OBJECT = 2;

	bool pickupObjects = false;

	// Movement types
	int movementType;
	const int WANDER_RANDOMLY = 0;
	const int WANDER_RANDOMLY_NOISE = 1;

	// How far the NPC looks for a new point to wander to.
	public float wanderRange = 10f;

	// How far the NPC looks for objects to interact with.
	public float interactRange = 10f;

	Transform targetObject;

	// Noise variables
	float noiseSpeed = 0.04f;
	float noiseX = 0.0f;
	float noiseY = 0.0f;
	float noiseXOffset = 100f;
	float noiseYOffset = 1000f;


	void Start()
	{
		navMeshAgent = transform.parent.GetComponent<NavMeshAgent> ();
		npcAnimation = GetComponent<NPCAnimation> ();

		currentState = GET_TARGET_POSITION;
	}


	void Update()
	{
		if (currentState == GET_TARGET_POSITION)
		{
			GetTargetPosition ();
			currentState = MOVE_TO_TARGET_POSITION;
		}

		else if (currentState == MOVE_TO_TARGET_POSITION)
		{
			// See if I've reached my target position
			if (navMeshAgent.velocity.magnitude == 0.0f) {
				CheckArea ();
				if (targetObject != null) {
					currentState = MOVE_TOWARDS_OBJECT;
				} else {
					currentState = GET_TARGET_POSITION;
				}
			}
		}

		else if (currentState == MOVE_TOWARDS_OBJECT)
		{
			// CHECK IF OBJECT IS IN RANGE. IF SO, PICK IT UP.
		}

		npcAnimation.Move (navMeshAgent.desiredVelocity);
	}


	void GetTargetPosition()
	{
		Vector3 targetPosition = transform.localPosition;

		if (movementType == WANDER_RANDOMLY_NOISE)
		{
			// Get a random direction with perlin noise.
			Vector3 direction = new Vector3 (
				MyMath.Map (Mathf.PerlinNoise (noiseX, 0.0f), 0f, 1f, -1f, 1f),
				transform.localPosition.y,
				MyMath.Map (Mathf.PerlinNoise(0.0f, noiseY), 0f, 1f, -1f, 1f)
			);

			direction.Normalize ();

			targetPosition += direction;

			noiseX += noiseSpeed;
			noiseY += noiseSpeed;
		}

		if (movementType == WANDER_RANDOMLY)
		{
			// Get a random direction with perlin noise.
			Vector2 randomCircle = Random.insideUnitCircle*wanderRange;
			targetPosition.x += randomCircle.x;
			targetPosition.z += randomCircle.y;
		}

		navMeshAgent.SetDestination(targetPosition);
	}


	void CheckArea()
	{
		// Get a list of all the nearby objects that I could pick up.
		List<Transform> carriableObjects = new List<Transform>();
		Collider[] nearbyObjects = Physics.OverlapSphere (transform.position, interactRange);
		foreach (Collider collider in nearbyObjects)
		{
			InteractionSettings intSet = collider.transform.GetComponentInChildren<InteractionSettings> ();
			if (intSet != null && intSet.ableToBeCarried)
			{
				carriableObjects.Add (collider.transform);
			}
		}

		// Choose a random object from that list to become my target.
		if (carriableObjects.Count > 0)
		{
			int randomNum = Random.Range (0, carriableObjects.Count);
			targetObject = carriableObjects [randomNum];
			navMeshAgent.SetDestination (targetObject.position);
			Debug.Log ("NPC is targeting: " + targetObject.name);
		}

		else
		{
			Debug.Log ("NPC found nothing in range to pick up.");
		}
	}
}
