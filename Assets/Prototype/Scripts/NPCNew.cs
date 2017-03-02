using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNew : MonoBehaviour {

    // BEHAVIOR STATES
    enum BehaviorState {NormalMovement};
    BehaviorState currentState = BehaviorState.NormalMovement;

    // USED FOR MOVEMENT
    Vector3 baseDirection;  // The general direction that I want to wander in.
    float meanderRange = 1f;  // How far I meander from the base direction.
    [SerializeField] float moveForce = 5f;
    [SerializeField] float maxVelocity = 10f;

    // USED FOR CHECKING ENVIRONMENT
    [SerializeField] float evaluateSurroundingsFreqMin = 2f;  // How often I stop to check my surroundings.
    [SerializeField] float evaluateSurroundingsFreqMax = 10f;
    float evaluateSurroundingsFreqCurrent;
    float timeSinceLastEvaluation;

    // FOR PERLIN NOISE
    float noiseSpeed = 1f;
    float noiseX = 0.0f;
    float noiseY = 0.0f;
    float noiseXOffset = 100f;
    float noiseYOffset = 1000f;

    // COMPONENT REFERENCES
    Rigidbody rb;
    NPCAnimation npcAnimation;

    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();

        // See if I have an animator before I try to use NPCAnimation.
        if (transform.parent.GetComponentInChildren<Animator>() != null)
        {
            npcAnimation = GetComponent<NPCAnimation>();
        }

        EvaluateSurroundings();
    }


    void Update()
    {
        if (currentState == BehaviorState.NormalMovement)
        {
            // See if it's time to check my surroundings.
            timeSinceLastEvaluation += Time.deltaTime;
            if (timeSinceLastEvaluation >= evaluateSurroundingsFreqCurrent)
            {
                EvaluateSurroundings();
            }

            // See if I'm about to hit something. If so, get a new direction.
            if (Physics.Raycast(transform.parent.position, baseDirection, 3f))
            {
                RandomizeBaseDirection();
            }

            Debug.DrawRay(transform.parent.position, baseDirection, Color.cyan);

            // Get a meandering path towards my base direction with perlin noise.
            Vector3 meanderDirection = baseDirection + new Vector3 (
                MyMath.Map (Mathf.PerlinNoise (noiseX, 0.0f), 0f, 1f, -meanderRange, meanderRange),
                0f,
                MyMath.Map (Mathf.PerlinNoise(0.0f, noiseY), 0f, 1f, -meanderRange, meanderRange)
            );

            Debug.DrawRay(transform.parent.position, meanderDirection, Color.red);

            noiseX += noiseSpeed * Time.deltaTime;
            noiseY += noiseSpeed * Time.deltaTime;

            // Turn me to face the meander direction.
            transform.parent.rotation = Quaternion.RotateTowards(
                Quaternion.LookRotation(transform.parent.forward),
                Quaternion.LookRotation(meanderDirection),
                150f * Time.deltaTime
            );

            // Move me forwards
            rb.AddForce(transform.parent.forward*moveForce*Time.deltaTime, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }


        // Send information to the animation script.
        if (npcAnimation != null) npcAnimation.Move(rb.velocity);
    }


    // Check the surrounding area and see if there is anything that interests me.
    void EvaluateSurroundings()
    {
        // (Looking for interactable items).
 
        RandomizeBaseDirection();

        // Decide how long until I next check my surroundings.
        evaluateSurroundingsFreqCurrent = Random.Range(evaluateSurroundingsFreqMin, evaluateSurroundingsFreqMax);
        timeSinceLastEvaluation = 0.0f;
    }


    // Gets a new base direction.
    void RandomizeBaseDirection()
    {
        Vector2 randomInCircle = Random.insideUnitCircle;
        baseDirection = new Vector3(randomInCircle.x, 0f, randomInCircle.y);
        baseDirection.Normalize();
    }
}