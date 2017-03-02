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
    const int THROWING_OBJECT = 4;

	//bool _pickupObjects = false;
//	bool _carryingObject;

	// Movement types
	int movementType;
	const int WANDER_RANDOMLY = 0;
	const int WANDER_RANDOMLY_NOISE = 1;

    // Used for movement/pathfinding
//    Vector3 finalDestination = transform.position;
    float rotateDirection = 0f;     // Which direction I am currently rotating to find my way around an obstacle.

	// How far the NPC looks for a new point to wander to.
	public float wanderRange = 10f;

	// How far the NPC looks for objects to interact with.
	public float interactRange = 10f;

	// Object pickup distance from the NPC.
	public float pickupRange = 1f;
    public float pickupProbability = 0.8f;

    public float throwProbability = 0.8f;  // How likely I am to throw an object.
    public float throwForce = 20f;  // The force with which I throw objects.


	// Reference to this NPC's Hand Transform -- SET IN INSPECTOR
	[SerializeField] Transform handTransform;

	[SerializeField] Transform targetObject;

    // A reference to the object that I am currently carrying.
    [SerializeField]Transform carriedObject;

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
//		_carryingObject = false;
	}


	void Update()
	{
        if (currentState == GET_TARGET_POSITION)
        {
            // If I am not currently carrying an object, look for one to pick up.
            if (carriedObject == null && targetObject == null)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < pickupProbability)
                {
                    CheckArea();
                }
            }

            // If I am carrying an object, decide if I want to throw it.
            if (carriedObject != null && targetObject == null)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < throwProbability)
                {
                    npcAnimation.ThrowObject();
                    currentState = THROWING_OBJECT;
                }
            }

            // If I didn't find a target object just move randomly.
            if (carriedObject == null && targetObject == null)
            {
                GetTargetPosition();
                currentState = MOVE_TO_TARGET_POSITION;
            }
        }

        else if (currentState == MOVE_TO_TARGET_POSITION)
        {
            // See if I've reached my target position
//            if (Vector3.Distance(finalDestination, transform.position) < 0.5f)
//            {
//                currentState = GET_TARGET_POSITION;
//            }

            // If I haven't reached my target position, check to see if there is an obstacle in my way.
//            else if (Physics.Raycast(transform.position, transform.forward, 5f))
//            {
//                // If I haven't chosen a rotation direction, choose one.
//                if (rotateDirection == 0f)
//                {
//                    // See if my target position is to my left or right.
////                    if (Vector3.Angle(
//                }
//            }
        }

        else if (currentState == MOVE_TOWARDS_OBJECT)
        {
            Debug.Log("Moving towards object");
            // Check if object is in range. If so, pick it up.
            if ((targetObject.transform.position - gameObject.transform.position).magnitude <= pickupRange)
            {
                // Freeze the object instantly so that it doesn't bounce off my collider & go flying.
                // Trying to disable the colliders?  for some reason the child object behaves erratically
                Collider[] childColliders = targetObject.GetComponents<Collider>();
                foreach (Collider collider in childColliders)
                {
                    collider.enabled = false;
                }
                targetObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                targetObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
                targetObject.gameObject.GetComponent<Rigidbody>().detectCollisions = false;

                // Disable Incoherence controller.
                targetObject.FindChild("Incoherence Controller").gameObject.SetActive(false);

                // Tell my animator to show the picking up animation.
                npcAnimation.PickupObject();

                Debug.Log("NPC is picking up " + targetObject);
                currentState = PICK_UP_OBJECT;
            }
        }

        else if (currentState == PICK_UP_OBJECT)
        {   
            AnimatorStateInfo asi = npcAnimation.animator.GetCurrentAnimatorStateInfo(0);

            if (asi.IsName("PickingUp") && asi.normalizedTime >= 0.26f && carriedObject == null)
            {
                AttachToHand();
            }
            else if (asi.IsName("PickingUp") && asi.normalizedTime >= 0.95f && carriedObject != null)
            {
                FinishedPickingUp();
            }
        }
       
        else if (currentState == THROWING_OBJECT)
        {
            Debug.Log("In throwing mode.");
            AnimatorStateInfo asi = npcAnimation.animator.GetCurrentAnimatorStateInfo(0);

            if (asi.IsName("Throwing") && asi.normalizedTime >= 0.5f && carriedObject != null)
            {
                ThrowObject();
            }
            else if (asi.IsName("Throwing") && asi.normalizedTime >= 0.95f && carriedObject == null)
            {
                currentState = GET_TARGET_POSITION;
            }
        }

        // Stop sending info to the animator if we're in pick up mode.
        if (currentState != PICK_UP_OBJECT && currentState != THROWING_OBJECT) {
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

//        finalDestination = targetPosition;
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
            currentState = MOVE_TOWARDS_OBJECT; 
			Debug.Log ("NPC is targeting: " + targetObject.name);
		}

		else
		{
			Debug.Log ("NPC found nothing in range to pick up.");
		}
	}


	//This will be called about 40% into the the pickup object animation 
	void AttachToHand () {
		//targetObject.gameObject.GetComponent<Rigidbody> ().enabled= false;
        carriedObject = targetObject;
		targetObject.position = handTransform.position;
		targetObject.SetParent (handTransform);
	}

	//Called at end of animation in order to reset state to wander
	void FinishedPickingUp () {
		npcAnimation.ObjectPickedUp ();
        Debug.Log("Picked up " + targetObject);
        targetObject = null;
		currentState = GET_TARGET_POSITION;
	}

    void ThrowObject()
    {
        Debug.Log("Tried to throw");
        if (carriedObject != null)
        {
            Debug.Log("Throwing");

            // Re-activate all the object's dormant properties.
            Collider[] childColliders = carriedObject.GetComponents<Collider> ();
            foreach (Collider collider in childColliders) {
                collider.enabled = true;
            }
            carriedObject.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
            carriedObject.gameObject.GetComponent<Rigidbody> ().useGravity = true;
            carriedObject.gameObject.GetComponent<Rigidbody> ().detectCollisions = true;
            carriedObject.FindChild("Incoherence Controller").gameObject.SetActive(true);

            // Deparent object.
            carriedObject.transform.parent = null;

            // Throw object by adding force to it.
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward*throwForce, ForceMode.Impulse);

            npcAnimation.ObjectThrown();

            carriedObject = null;
        }
    }
}
