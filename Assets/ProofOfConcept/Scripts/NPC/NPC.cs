using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    // BEHAVIOR STATES
    enum BehaviorState {NormalMovement, MoveToObject, PickUpObject, ThrowObject, SayingHello};
    [SerializeField] BehaviorState currentState;

    // USED FOR MOVEMENT
    Vector3 baseDirection;  // The general direction that I want to wander in.
    float meanderRange = 1f;  // How far I meander from the base direction.
    float lookForwardRange = 5f;    // How far this NPC looks in front of them for objects/obstacles to react to.
    [SerializeField] float moveForce = 5f;
    [SerializeField] float maxVelocity = 10f;
    bool updateAnimation = false;    // Whether I should send movement information to the animator script this frame

    // USED FOR CHECKING ENVIRONMENT
    [SerializeField] float evaluateSurroundingsFreqMin = 2f;  // How often I stop to check my surroundings for objects of interest.
    [SerializeField] float evaluateSurroundingsFreqMax = 10f;
    float evaluateSurroundingsFreqCurrent;
    float timeSinceLastEvaluation;
    [SerializeField] float evaluationRange = 5f;

    // USED FOR PICKING UP OBJECTS
    Transform targetObject;
    Transform carriedObject;
    [SerializeField] float objectPickupRange = 3f;  // How close this NPC has to be to an object in order to pick it up.
    [SerializeField] Transform handTransform;   // The transform of this npc's 'hand'.
    [SerializeField] float pickupProbability = 0.2f;
    [SerializeField] float useProbability = 0.2f;   // The probability that I will use an object I'm holding.
    [SerializeField] float giveUpTime = 10f; // How long it takes for this NPC to give up when they are unable to reach their targeted object.
    float giveUpTimer = 0f; // Used for keeping track of when to give up.

    // USED FOR SAYING HELLO
    float helloLength = 2f; // How long this NPC pauses to say hello. (Only applies if this NPC does not have an animator, if they do then we'll just wait until the waving animation ends.
    string helloName;   // The name of the object that we're going to say hello to
    bool saidHello; // Whether we already displayed out hello text during the current wave.
    float helloTimer = 0.0f;

    // USED FOR THROWING OBJECTS
    public float throwProbability = 0.2f;  // How likely I am to throw an object at a nearby object.
    float throwForce = 20f;     
    Transform throwTarget;  // The object I will throw my carried object at.

    // USED FOR HATE
    [SerializeField] float hateProbability = 0.04f; // The probability that I will hate an object if I see it.
    List<string> hatedObjects;  // The name of objects that I hate.

    // USED FOR PAIN & DEATH
    float _health = 100f;
    public float health
    {
        get { return _health; }
        set
        {
            Debug.Log("Current health: " + value);
            if (value <= 0f)
            {
                Die();
            }

            else
                _health = value;
        }
    }
    float painThreshold = 10f;   // The collision magnitude below which health is not damaged.
    Material ghostMaterial;

    // GENERAL USE
    float generalTimer;  // Primarily used to determine how long an action takes if this NPC does not use an animator.

    // FOR PERLIN NOISE
    float noiseSpeed = 1f;
    float noiseX = 0.0f;
    float noiseY = 0.0f;
    float noiseXOffset = 100f;
    float noiseYOffset = 1000f;

    // COMPONENT REFERENCES
    Rigidbody rb;
    NPCAnimation npcAnimation;
    Writer writer;
	AudioSource speakSource;



    void Start()
    {
        ghostMaterial = Resources.Load("Materials/Ghost") as Material;

        if (transform.parent.GetComponent<CollisionReporter>() == null)
        {
            transform.parent.gameObject.AddComponent<CollisionReporter>();
        }

        rb = transform.parent.GetComponent<Rigidbody>();
		speakSource = GetComponent<AudioSource> ();
        // See if I have an animator before I try to use NPCAnimation.
        if (transform.parent.GetComponentInChildren<Animator>() != null && transform.parent.GetComponentInChildren<Animator>().isHuman)
        {
            npcAnimation = GetComponent<NPCAnimation>();
        }

        writer = GetComponent<Writer>();

        // If I have no hand position assigned, create one.
        if (handTransform == null)
        {
            GameObject handObject = new GameObject("Hand");
            handObject.transform.parent = transform.parent;

            handObject.transform.localPosition = Vector3.zero;
            handObject.transform.localScale = new Vector3(1f, 1f, 1f);
            handObject.transform.rotation = Quaternion.identity;
            handObject.transform.Translate(new Vector3(0f, 1f, 0f));
            handTransform = handObject.transform;
        }

        // Decide which objects I should hate.
        int numberOfHatedObjects = Random.Range(0, 5);
        InteractionSettings[] allGameObjects = GameObject.FindObjectsOfType<InteractionSettings>();
        hatedObjects = new List<string>();
        for (int i = 0; i < numberOfHatedObjects; i++)
        {
            hatedObjects.Add(allGameObjects[Random.Range(0, allGameObjects.Length)].transform.parent.name);
            //Debug.Log(hatedObjects[i]);
        }

        // I always hate things that are covered in feces.
        hatedObjects.Add("Feces-Covered");

        EvaluateSurroundings();
        currentState = BehaviorState.NormalMovement;
    }


    void Update()
    {
        updateAnimation = false;

        if (lookForwardRange < transform.parent.GetComponent<Collider>().bounds.extents.z)
        {
            lookForwardRange = transform.parent.GetComponent<Collider>().bounds.extents.z * 2;
            objectPickupRange = lookForwardRange;
        }

        // SEE IF THE OBJECT I WAS CARRYING WAS STOLEN OR RAN AWAY
        if (carriedObject != null &&
            (carriedObject.GetComponentInChildren<InteractionSettings>().carryingObject != transform.parent ||
            Vector3.Distance(carriedObject.transform.position, transform.parent.position) >= 30f))
        {
            // Tell the object to stop ignoring collisions with me.
            Collider[] childColliders = carriedObject.GetComponents<Collider>();
            foreach (Collider collider in childColliders)
            {
                Physics.IgnoreCollision(collider, GetComponentInParent<Collider>(), false);
            }

            if (npcAnimation != null) 
			{
				npcAnimation.ObjectPickedUp();
            	npcAnimation.ObjectThrown();
			}

            if (carriedObject.GetComponentInChildren<InteractionSettings>().carryingObject != null)
            {
                writer.WriteSpecifiedString(
                    "Oh no! " +
                    carriedObject.GetComponentInChildren<InteractionSettings>().carryingObject.name + " stole my " + carriedObject.name + "!"
                    );
            }

            //Play Voice Sound Effect
			Speak();
            
			carriedObject = null;
            EvaluateSurroundings();
        }


        // MOVING NORMALLY (WANDERING & INTERMITENTLY EVALUATING SURROUNDINGS)
        if (currentState == BehaviorState.NormalMovement)
        {
            // See if it's time to check my surroundings.
            timeSinceLastEvaluation += Time.deltaTime;
            if (timeSinceLastEvaluation >= evaluateSurroundingsFreqCurrent)
            {
                EvaluateSurroundings();
            }

            // Check for obstacles.
            Vector3 rayOrigin = transform.parent.position;
            RaycastHit hit;

            // Raise the origin of the ray slightly so that it's above ground.
            rayOrigin.y += 1f;

            Vector3 rayDirection = baseDirection;

            // Get a ray direction that is modified to follow the current slope of the ground.
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 5f))
            {
                rayDirection = Vector3.ProjectOnPlane(rayDirection, hit.normal);
            }

            Debug.DrawRay(rayOrigin, rayDirection * lookForwardRange, Color.red);
            Debug.DrawRay(rayOrigin + rayDirection.normalized * lookForwardRange, Vector3.down * lookForwardRange, Color.magenta, 0.1f);

            // Do a raycast in the base direction to see if we're heading towards a wall or cliff.
            bool sawAnObstacle = false;
            if (Physics.Raycast(rayOrigin, baseDirection, out hit, lookForwardRange))
            {
                // If this is not an object I can interact with, it's probably a wall.
                if (hit.collider.GetComponent<InteractionSettings>() == null)
                {
                    sawAnObstacle = true;
                    RandomizeBaseDirection();
                }
            }

            // If there is nothing directly in front of me then do a raycast downwards to make sure I'm not at the edge of a cliff.
            else if (!Physics.Raycast(rayOrigin + rayDirection.normalized * lookForwardRange, Vector3.down, lookForwardRange))
            {
                sawAnObstacle = true;
                RandomizeBaseDirection();
            }

            // Check for other objects directly in front of me.
            rayDirection = transform.parent.forward;

            // Get a ray direction that is modified to follow the current slope of the ground.
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 5f))
            {
                rayDirection = Vector3.ProjectOnPlane(rayDirection, hit.normal);
            }

            Debug.DrawRay(rayOrigin, rayDirection * lookForwardRange, Color.blue);

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, lookForwardRange))
            {
                // See if I hate the object I'm looking at.
                foreach (string name in hatedObjects)
                {
                    if (hit.collider.gameObject.name.Contains(name))
                    {
                        writer.WriteSpecifiedString("Oh no! I hate " + name + "s! I'm out of here!");
                        baseDirection *= -1;
                    }
                }

                // See if the object we're looking at is a player or another NPC. (For waving hello.)
                if (hit.collider.GetComponentInChildren<NPC>() != null ||
                     hit.collider.name == "Player")
                {
                    // If we're saying hello to ourself, acknowledge it.
                    if (hit.collider.transform == transform.parent)
                    {
                        helloName = "myself";
                    }
                    else
                    {
                        helloName = hit.collider.name;
                    }

                    // Get prepare to switch to 'saying hello' mode.
                    if (npcAnimation != null) npcAnimation.WaveHello();
                    generalTimer = 0f;
                    currentState = BehaviorState.SayingHello;
                }
            }

            // Get a meandering path towards my base direction with perlin noise.
            Vector3 meanderDirection = baseDirection + new Vector3(
                                            MyMath.Map(Mathf.PerlinNoise(noiseX, 0.0f), 0f, 1f, -meanderRange, meanderRange),
                                            0f,
                                            MyMath.Map(Mathf.PerlinNoise(0.0f, noiseY), 0f, 1f, -meanderRange, meanderRange)
                                        );

            noiseX += noiseSpeed * Time.deltaTime;
            noiseY += noiseSpeed * Time.deltaTime;

            // Get a meander that is modified to follow the current slope of the ground
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 5f))
            {
                meanderDirection = Vector3.ProjectOnPlane(meanderDirection, hit.normal);
            }

            if (!sawAnObstacle) MoveInDirection(meanderDirection);
            else
            {
                float tempVel = rb.velocity.z;
                tempVel *= 0.3f;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, tempVel);
            }

            updateAnimation = true;
        }

        // SAYING HELLO TO NPC OR PLAYER
        else if (currentState == BehaviorState.SayingHello)
        {
            // If this NPC uses an animator.
            if (npcAnimation != null)
            {
                AnimatorStateInfo asi = npcAnimation.animator.GetCurrentAnimatorStateInfo(0);
                
                // Display text.
                if (!saidHello && asi.IsName("WavingHello") && asi.normalizedTime >= 0.4f)
                {
                    writer.WriteSpecifiedString("Hello, " + helloName + ". It's me, " + transform.parent.name + ".");
                    saidHello = true;

					//Play Voice Sound Effect
					Speak();
                }

                // Finish waving.
                else if (saidHello && asi.IsName("WavingHello") && asi.normalizedTime >= 0.95f)
                {
                    npcAnimation.WaveHelloFinished();
                    saidHello = false;
                    EvaluateSurroundings();
                }
            }

            // If this NPC does not use an animator.
            else
            {
                helloTimer += Time.deltaTime;

                // Display text.
                if (!saidHello && helloTimer >= helloLength*0.5f)
                {
                    writer.WriteSpecifiedString("Hello, " + helloName + ". It's me, " + transform.parent.name + ".");
                    saidHello = true;
				
					//Play Voice Sound Effect
					Speak();
                }

                // Finish waving.
                else if (saidHello && helloTimer >= helloLength)
                {
                    Debug.Log("doneoe");
                    saidHello = false;
                    EvaluateSurroundings();
                    helloTimer = 0f;
                }
            }
        }

        // MOVING TO A PARTICULAR OBJECT
        else if (currentState == BehaviorState.MoveToObject)
        {
            // See if I have taken too long to reach this object and should thus give up.
            giveUpTimer += Time.deltaTime;
            if (giveUpTimer >= giveUpTime)
            {
                writer.WriteSpecifiedString("I give up.");
                EvaluateSurroundings();
            }

            SeeIfTargetObjectWasPickedUp();

            // Check if target object is in range. If so, pick it up.
            if (Vector3.Distance(targetObject.position, transform.parent.position) <= objectPickupRange)
            {
                // Freeze the object instantly so that it doesn't bounce off my collider & go flying.
                Collider[] childColliders = targetObject.GetComponents<Collider>();
                foreach (Collider collider in childColliders)
                {
                    Physics.IgnoreCollision(collider, GetComponentInParent<Collider>(), true);
                }

                targetObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                targetObject.gameObject.GetComponent<Rigidbody>().useGravity = false;

                // Disable Incoherence controller && NPC AI
                if (targetObject.FindChild("Incoherence Controller") != null) targetObject.FindChild("Incoherence Controller").gameObject.SetActive(false);
                if (targetObject.FindChild("NPC AI") != null) targetObject.FindChild("NPC AI").gameObject.SetActive(false);

                // Tell this object that it is being carried by me.
                targetObject.GetComponentInChildren<InteractionSettings>().carryingObject = transform.parent;

                // Tell my animator to show the picking up animation.
                if (npcAnimation != null) npcAnimation.PickupObject();
                generalTimer = 0f;
                currentState = BehaviorState.PickUpObject;
            }

            // If the current object is not yet in range, move towards it.
            else
            {
                // Move to target object.
                Vector3 directionToTarget = targetObject.position - transform.parent.position;
                MoveInDirection(directionToTarget);
            }

            updateAnimation = true;
        }

        // PICKING UP
        else if (currentState == BehaviorState.PickUpObject)
        {
            SeeIfTargetObjectWasPickedUp();

            // If we are using npcAnimation, then use the animation position to determine when the object should attach to my hand, etc.
            if (npcAnimation != null)
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

            // If we are not using npcAnimation, then just use a timer.
            else
            {
                generalTimer += Time.deltaTime;

                if (generalTimer >= 1f)
                {
                    AttachToHand();
                    FinishedPickingUp();
                }
            }

        }

        // THROWING
        else if (currentState == BehaviorState.ThrowObject)
        {
            // See if I am facing the object that I want to throw at, and if not then rotate towards it.
            Vector3 flatTargetPos = throwTarget.transform.position;
            flatTargetPos = new Vector3(throwTarget.transform.position.x, transform.parent.position.y, throwTarget.transform.position.z);
            float angleToTarget = Vector3.Angle(transform.parent.forward, flatTargetPos - transform.parent.position);
            if (angleToTarget > 0.5f)
            {
                transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, Quaternion.LookRotation(throwTarget.position - transform.parent.position), 5f);
                transform.parent.rotation = Quaternion.Euler(new Vector3(0f, transform.parent.rotation.eulerAngles.y, 0f));
            }

            // If I am facing my target, throw the object
            else
            {
                // If this NPC uses an animator, use that to determine throw timing.
                if (npcAnimation != null)
                {
                    AnimatorStateInfo asi = npcAnimation.animator.GetCurrentAnimatorStateInfo(0);

                    if (asi.IsName("Throwing") && asi.normalizedTime >= 0.5f && carriedObject != null)
                    {
                        ThrowObject();
                    }
                    else if (asi.IsName("Throwing") && asi.normalizedTime >= 0.95f && carriedObject == null)
                    {
                        EvaluateSurroundings();
                    }
                }

                // Otherwise determine timing manually.
                else
                {
                    generalTimer += Time.deltaTime;

                    if (generalTimer >= 1f)
                    {
                        ThrowObject();
                        EvaluateSurroundings();
                    }
                }
            }
        }

        // Send information to the animation script.
        if (npcAnimation != null && updateAnimation) npcAnimation.Move(rb.velocity);
    }


    // Check the surrounding area and see if there is anything that interests me.
    void EvaluateSurroundings()
    {
        // Decide whether to use my carried object.
        if (carriedObject != null && Random.value >= useProbability)
        {
            carriedObject.BroadcastMessage("Use", SendMessageOptions.DontRequireReceiver);
        }

        // Evaluate nearby objects.
        List<Transform> carriableObjects = new List<Transform>();
        List<Transform> throwTargets = new List<Transform>();

        Collider[] nearbyObjects = Physics.OverlapSphere (transform.parent.position, evaluationRange);
        foreach (Collider collider in nearbyObjects)
        {
            // Make sure this collider does not belong to me.
            if (collider.transform != carriedObject && collider.transform != transform.parent)
            {
                // If I am not currently carrying an object, then add this object to a list of carriable objects in range.
                if (carriedObject == null)
                {
                    InteractionSettings intSet = collider.transform.GetComponentInChildren<InteractionSettings>();
                    if (intSet != null && intSet.ableToBeCarried && intSet.carryingObject == null)
                    {
                        carriableObjects.Add(collider.transform);
                    }
                }

                // If I am carrying an object, decide whether I want to throw it at this object.
                else if (carriedObject != null)
                {
                    throwTargets.Add(collider.transform);
                }
            }
        }

        // See if I want to pick something up
        if (carriableObjects.Count > 0 && Random.Range(0f, 1f) <= pickupProbability)
        {
            giveUpTimer = 0f;
            targetObject = carriableObjects [Random.Range (0, carriableObjects.Count)];
            writer.WriteSpecifiedString("I want that " + targetObject.name + ".");
            currentState = BehaviorState.MoveToObject;

            //Play Voice Sound Effect
			Speak();
        }

        // See if I want to throw something.
        else if (throwTargets.Count > 0 && Random.Range(0f, 1f) <= throwProbability)
        {
            throwTarget = throwTargets[Random.Range(0, throwTargets.Count)];
            if (npcAnimation != null) npcAnimation.ThrowObject();
            generalTimer = 0f;
            currentState = BehaviorState.ThrowObject;

            writer.WriteSpecifiedString("Have this " + carriedObject.name + ", " + throwTarget.name + ".");
            //Play Voice Sound Effect

			Speak ();
            
        }

        // If I decided not to pick anything up.
        else
        {
            RandomizeBaseDirection();

            // Decide how long until I next check my surroundings.
            evaluateSurroundingsFreqCurrent = Random.Range(evaluateSurroundingsFreqMin, evaluateSurroundingsFreqMax);
            timeSinceLastEvaluation = 0.0f;

            currentState = BehaviorState.NormalMovement;
        }
    }


    void AttachToHand()
    {
//        writer.WriteSpecifiedString("Ah, what a nice " + targetObject.name);

        carriedObject = targetObject;
        carriedObject.GetComponentInChildren<InteractionSettings>().carryingObject = transform.parent;
        targetObject.position = handTransform.position;
        targetObject.SetParent (handTransform);
    }


    // Called at end of animation in order to reset state to wander
    void FinishedPickingUp ()
    {
//		if (targetObject.gameObject.GetComponent<AudioSource> () && Services.AudioManager != null) {
//			//Services.AudioManager.RetuneRadio (targetObject);
//		}
        if (npcAnimation != null) npcAnimation.ObjectPickedUp ();
        targetObject = null;
        currentState = BehaviorState.NormalMovement;
    }


    // Gets a new base direction.
    void RandomizeBaseDirection()
    {
        Vector2 randomInCircle = Random.insideUnitCircle;
        baseDirection = new Vector3(randomInCircle.x, 0f, randomInCircle.y);
        baseDirection.Normalize();
    }


    void MoveInDirection(Vector3 _direction)
    {
        // Turn me to face the meander direction.
        transform.parent.rotation = Quaternion.RotateTowards(
            Quaternion.LookRotation(transform.parent.forward),
            Quaternion.LookRotation(_direction),
            150f * Time.deltaTime
        );

        // Move me forwards
        rb.AddForce(transform.parent.forward * moveForce * Time.deltaTime, ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }


    void ThrowObject()
    {
        if (carriedObject != null)
        {
            // Re-activate all the object's dormant properties.
            Collider[] childColliders = carriedObject.GetComponents<Collider> ();
            foreach (Collider collider in childColliders) {
                Physics.IgnoreCollision(collider, GetComponentInParent<Collider>(), false);
            }
            carriedObject.gameObject.GetComponent<Rigidbody> ().isKinematic = false;
            carriedObject.gameObject.GetComponent<Rigidbody> ().useGravity = true;
            carriedObject.gameObject.GetComponent<Rigidbody> ().detectCollisions = true;
            if (carriedObject.FindChild("Incoherence Controller") != null) carriedObject.FindChild("Incoherence Controller").gameObject.SetActive(true);
            if (carriedObject.FindChild("NPC AI") != null) carriedObject.FindChild("NPC AI").gameObject.SetActive(true);
            carriedObject.GetComponentInChildren<InteractionSettings>().carryingObject = null;

            // Deparent object.
            carriedObject.transform.parent = null;

            // Throw object by adding force to it.
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward*throwForce, ForceMode.Impulse);

            if (npcAnimation != null) npcAnimation.ObjectThrown();

            carriedObject = null;
        }
    }


    void SeeIfTargetObjectWasPickedUp()
    {
        if (targetObject == null) return;

        if (targetObject.GetComponentInChildren<InteractionSettings>().carryingObject != null && targetObject.GetComponentInChildren<InteractionSettings>().carryingObject != transform.parent)
        {
            writer.WriteSpecifiedString(
                "Hey! I wanted that " + targetObject.name + ", " + targetObject.GetComponentInChildren<InteractionSettings>().carryingObject.name + "!"
                );

            if (npcAnimation != null) npcAnimation.ObjectPickedUp();
            EvaluateSurroundings();
        }
    }

	//this does audio stuff
	void Speak() {

		if (Services.AudioManager != null)
		{


			speakSource.clip = Services.AudioManager.voiceClipPool [Random.Range (0, Services.AudioManager.voiceClipPool.Length - 1)];

			speakSource.Play ();
		}
	}

    public void CollisionInParent(Collision collision)
    {
        // If collision magnitude is over a certain amount, get hurt.
        if (collision.gameObject.name != "GROUND" && collision.relativeVelocity.magnitude > painThreshold)
        {
            Debug.Log("Ouch! That " + collision.gameObject.name + " hurt me!");
            writer.WriteSpecifiedString("Ouch! That " + collision.gameObject.name + " hurt me!");

            // See if we should start hating this object.s
            if (!hatedObjects.Contains(collision.gameObject.name))
            {
                hatedObjects.Add(collision.gameObject.name);
            }

            health -= collision.relativeVelocity.magnitude * 0.2f;
        }
    }

    void Die()
    {
        writer.WriteSpecifiedString("Aargh! I've been murdered!");
        
        // Turn into ghost
        transform.parent.gameObject.name = "Dead " + transform.parent.gameObject.name;

        //foreach (MeshRenderer mr in transform.parent.GetComponentsInChildren<MeshRenderer>())
        //{
        //    mr.material = ghostMaterial;
        //}

        //transform.parent.GetComponent<Rigidbody>().mass *= 0.01f;
        if (transform.parent.GetComponentInChildren<Animator>() != null) transform.parent.GetComponentInChildren<Animator>().enabled = false;
        Destroy(transform.parent.GetComponent<CollisionReporter>());

        transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        transform.parent.GetComponent<Rigidbody>().AddTorque(Random.insideUnitCircle * 10f, ForceMode.Impulse);

        Debug.Log("Dead");

        // Destroy NPC AI prefab
        gameObject.SetActive(false);
    }


    public void CatchOnFire()
    {
        writer.WriteSpecifiedString("Argh! I'm on fire!");
    }
}