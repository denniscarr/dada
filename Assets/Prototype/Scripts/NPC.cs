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
	const int PICK_UP_OBJECT = 3;

	//bool _pickupObjects = false;
	bool _carryingObject;

	// Movement types
	int movementType;
	const int WANDER_RANDOMLY = 0;
	const int WANDER_RANDOMLY_NOISE = 1;

	// How far the NPC looks for a new point to wander to.
	public float wanderRange = 10f;

	// How far the NPC looks for objects to interact with.
	public float interactRange = 10f;

	// Object pickup distance from the NPC
	public float pickupRange = 1f;

	// Reference to this NPC's Hand Transform -- SET IN INSPECTOR
	[SerializeField] Transform handTransform;

	[SerializeField] Transform targetObject;

	// Noise variables
	float noiseSpeed = 0.04f;
	float noiseX = 0.0f;
	float noiseY = 0.0f;
	float noiseXOffset = 100f;
	float noiseYOffset = 1000f;


	void Start()
	{
		navMeshAgent = transform.GetComponent<NavMeshAgent> ();
		npcAnimation = GetComponent<NPCAnimation> ();

		currentState = GET_TARGET_POSITION;
		_carryingObject = false;
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
				if (!_carryingObject) {
					CheckArea ();
				}

				if (targetObject != null) {
					currentState = MOVE_TOWARDS_OBJECT;
				} else {
					currentState = GET_TARGET_POSITION;
				}
			}
		}

		else if (currentState == MOVE_TOWARDS_OBJECT)
		{
			
			if ((targetObject.transform.position - gameObject.transform.position).magnitude <= pickupRange) {
				currentState = PICK_UP_OBJECT;
				npcAnimation.PickupObject ();

			}

			// CHECK IF OBJECT IS IN RANGE. IF SO, PICK IT UP.
		}

		if (currentState != PICK_UP_OBJECT) {
			npcAnimation.Move (navMeshAgent.desiredVelocity);
		}

		//TODO have them randomly drop object, you know, just for fun

		/*
		if (_carryingObject && Random.value <= 0.01f) {
			targetObject.parent = null;
			_carryingObject = false;
		}
		*/
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


	//This will be called about 40% into the the pickup object animation 
	public void AttachToHand () {

		//trying to disable the colliders?  for some reason the child object behaves erratically
		Collider[] childColliders = targetObject.gameObject.GetComponents<Collider> ();
		foreach (Collider collider in childColliders) {
			collider.enabled = false;
		}
		targetObject.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		targetObject.gameObject.GetComponent<Rigidbody> ().useGravity = false;
		targetObject.gameObject.GetComponent<Rigidbody> ().detectCollisions = false;

		//targetObject.gameObject.GetComponent<Rigidbody> ().enabled= false;
		targetObject.position = handTransform.position;
		targetObject.SetParent (handTransform);
		_carryingObject = true;
		targetObject = null;
	}

	//Called at end of animation in order to reset state to wander
	public void FinishedPickingUp () {
		npcAnimation.ObjectPickedUp ();
		currentState = GET_TARGET_POSITION;
	}
}
