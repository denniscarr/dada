﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    // BEHAVIOR STATES
    public enum BehaviorState {NormalMovement, MoveToObject, PickUpObject, ThrowObject};
    public BehaviorState currentState = BehaviorState.NormalMovement;

    // USED FOR MOVEMENT
    Vector3 baseDirection;  // The general direction that I want to wander in.
    float meanderRange = 1f;  // How far I meander from the base direction.
    [SerializeField] float moveForce = 5f;
    [SerializeField] float maxVelocity = 10f;
    bool updateAnimation = false;    // Whether I should send movement information to the animator script this frame.

    // USED FOR CHECKING ENVIRONMENT
    [SerializeField] float evaluateSurroundingsFreqMin = 2f;  // How often I stop to check my surroundings for objects of interest.
    [SerializeField] float evaluateSurroundingsFreqMax = 10f;
    float evaluateSurroundingsFreqCurrent;
    float timeSinceLastEvaluation;
    [SerializeField] float evaluationRange = 5f;

    // USED FOR PICKING UP OBJECTS
    Transform targetObject;
    Transform carriedObject;
    float objectPickupRange = 2f;
    [SerializeField] Transform handTransform;   // The transform of this npc's 'hand'.
    float pickupThrowTimer;  // Used to determine how long picking up/throwing takes if this NPC does not use an animator.
    [SerializeField] float pickupProbability = 0.2f;

    // USED FOR THROWING OBJECTS
    public float throwProbability = 0.2f;  // How likely I am to throw an object at a nearby object.
    float throwForce = 20f;     
    Transform throwTarget;  // The object I will throw my carried object at.

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

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();

        // See if I have an animator before I try to use NPCAnimation.
        if (transform.parent.GetComponentInChildren<Animator>() != null)
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

        EvaluateSurroundings();
    }


    void Update()
    {
        updateAnimation = false;

        // MOVING NORMALLY (WANDERING & INTERMITENTLY EVALUATING SURROUNDINGS)
        if (currentState == BehaviorState.NormalMovement)
        {
            // See if it's time to check my surroundings.
            timeSinceLastEvaluation += Time.deltaTime;
            if (timeSinceLastEvaluation >= evaluateSurroundingsFreqCurrent)
            {
                EvaluateSurroundings();
            }

            // See if I'm about to hit something. If so, get a new direction.
            Vector3 rayPos = transform.parent.position;
            rayPos.y = 1f;
            if (Physics.Raycast(rayPos, baseDirection, 5f))
            {
                RandomizeBaseDirection();
            }
                
            // Get a meandering path towards my base direction with perlin noise.
            Vector3 meanderDirection = baseDirection + new Vector3(
                                           MyMath.Map(Mathf.PerlinNoise(noiseX, 0.0f), 0f, 1f, -meanderRange, meanderRange),
                                           0f,
                                           MyMath.Map(Mathf.PerlinNoise(0.0f, noiseY), 0f, 1f, -meanderRange, meanderRange)
                                       );
                
            noiseX += noiseSpeed * Time.deltaTime;
            noiseY += noiseSpeed * Time.deltaTime;

            MoveInDirection(meanderDirection);

            updateAnimation = true;
        }

        // MOVING TO A PARTICULAR OBJECT
        else if (currentState == BehaviorState.MoveToObject)
        {
            // Check to see if this object has become unable to be picked up (usually because it was picked up by another NPC)
            if (targetObject.GetComponentInChildren<InteractionSettings>().ableToBeCarried == false)
            {
                EvaluateSurroundings();
                return;
            }

            // Check if target object is in range. If so, pick it up.
            if (Vector3.Distance(targetObject.position, transform.parent.position) <= objectPickupRange)
            {
                // Freeze the object instantly so that it doesn't bounce off my collider & go flying.
                Collider[] childColliders = targetObject.GetComponents<Collider>();
                foreach (Collider collider in childColliders)
                {
                    collider.enabled = false;
                }

                targetObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                targetObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
                targetObject.gameObject.GetComponent<Rigidbody>().detectCollisions = false;

                // Disable Incoherence controller && NPC AI
                if (targetObject.FindChild("Incoherence Controller") != null) targetObject.FindChild("Incoherence Controller").gameObject.SetActive(false);
                if (targetObject.FindChild("NPC AI") != null) targetObject.FindChild("NPC AI").gameObject.SetActive(false);

                // Set this item to not able to be carried so that no other NPCs try to pick it up.
                targetObject.GetComponentInChildren<InteractionSettings>().ableToBeCarried = false;

                // Tell my animator to show the picking up animation.
                if (npcAnimation != null) npcAnimation.PickupObject();

                Debug.Log(transform.parent.name + " is picking up " + targetObject);

                pickupThrowTimer = 0f;

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
                pickupThrowTimer += Time.deltaTime;

                if (pickupThrowTimer >= 1f)
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
                    pickupThrowTimer += Time.deltaTime;

                    if (pickupThrowTimer >= 1f)
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
        // Evaluate nearby objects.
        List<Transform> carriableObjects = new List<Transform>();
        List<Transform> throwTargets = new List<Transform>();

        Collider[] nearbyObjects = Physics.OverlapSphere (transform.position, evaluationRange);
        foreach (Collider collider in nearbyObjects)
        {
            // Make sure this collider does not belong to me.
            if (collider.transform != carriedObject && collider.transform != transform.parent)
            {
                // If I am not currently carrying an object, then add this object to a list of carriable objects in range.
                if (carriedObject == null)
                {
                    InteractionSettings intSet = collider.transform.GetComponentInChildren<InteractionSettings>();
                    if (intSet != null && intSet.ableToBeCarried)
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
            targetObject = carriableObjects [Random.Range (0, carriableObjects.Count)];
            currentState = BehaviorState.MoveToObject;
            writer.WriteSpecifiedString("I want that " + targetObject.name + ".");
        }

        // See if I want to throw something.
        else if (throwTargets.Count > 0 && Random.Range(0f, 1f) <= throwProbability)
        {
            throwTarget = throwTargets[Random.Range(0, throwTargets.Count)];
            if (npcAnimation != null) npcAnimation.ThrowObject();
            pickupThrowTimer = 0f;
            currentState = BehaviorState.ThrowObject;

            writer.WriteSpecifiedString("Have this " + carriedObject.name + ", " + throwTarget.name + ".");
        }

        // If I decided not to pick anything up.
        else
        {
            writer.WriteSpecifiedString("There is nothing here.");

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
        targetObject.position = handTransform.position;
        targetObject.SetParent (handTransform);
    }


    // Called at end of animation in order to reset state to wander
    void FinishedPickingUp ()
    {
        if (npcAnimation != null) npcAnimation.ObjectPickedUp ();
        Debug.Log(transform.parent.name + " Picked up " + targetObject);
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
            if (carriedObject.FindChild("Incoherence Controller") != null) carriedObject.FindChild("Incoherence Controller").gameObject.SetActive(true);
            if (carriedObject.FindChild("NPC AI") != null) carriedObject.FindChild("NPC AI").gameObject.SetActive(true);
            carriedObject.GetComponentInChildren<InteractionSettings>().ableToBeCarried = true;

            // Deparent object.
            carriedObject.transform.parent = null;

            // Throw object by adding force to it.
            carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward*throwForce, ForceMode.Impulse);

            if (npcAnimation != null) npcAnimation.ObjectThrown();

            carriedObject = null;
        }
    }
}