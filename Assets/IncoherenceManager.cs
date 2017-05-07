using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncoherenceManager : MonoBehaviour {

    public float globalIncoherence = 0.0f;

    public float interactionIncrease = 0.05f;  // How much an object's incoherence increases when the player interacts with that object.
    public float questIncrease = 0.1f;

    // Thresholds.
    [SerializeField] float inanimateObjectNPCThreshold = 0.1f;  // How high global incoherence needs to be before we start turning random objects into NPCS.
    [SerializeField] float replaceObjectThreshold = 0.5f; // How high global incoherence needs to be before we start replacing interactive objects.
    [SerializeField] float affectStaticObjectThreshold = 0.9f; // How high global incoherence needs to be before we start affecting all game objects (bad things will happen.)

    // Prefab references.
    [SerializeField] GameObject NPCAI;
    [SerializeField] GameObject equipability;
    [SerializeField] GameObject incoherenceController;

    // Objects which have InteractionSettings
    List<GameObject> affectedObjects;

    // Incoherence events.
    GravityShiftEvent gravityShiftEvent;

    // A list to hold incoherence events whose threshold has not been passed & whose have been passed.
    List<IncoherenceEvent> dormantEvents;
    List<IncoherenceEvent> activeEvents;
    IncoherenceEvent nextEvent;

    float timeSinceLastEvent = 0f;
    float timeUntilNextEvent = 0f;


    private void Start()
    {
        // Instantiate event components.
        dormantEvents = new List<IncoherenceEvent>();
        dormantEvents.Add(gameObject.AddComponent<GravityShiftEvent>());

        activeEvents = new List<IncoherenceEvent>();
    }


    private void Update()
    {
        // See if I should apply an incoherence event on the next audio shuffle.
        if (activeEvents.Count > 0 && nextEvent == null)
        {
            float eventChance = MyMath.Map(globalIncoherence, 0f, 1f, 0f, 1f);

            if (Random.value > eventChance)
            {
                QueueEvent();
            }
        }

        // Wait for the next audio shuffle and apply the event.
        else if (nextEvent != null)
        {
            timeSinceLastEvent += Time.deltaTime;
            if (timeSinceLastEvent >= timeUntilNextEvent)
            {
                nextEvent.Initiate();
                nextEvent = null;
            }
        }
    }


    void QueueEvent()
    {
        nextEvent = activeEvents[Random.Range(0, activeEvents.Count)];

        timeUntilNextEvent = MyMath.Map(globalIncoherence, 0f, 1f, 60f, 2f);
        Debug.Log("Queuing: " + nextEvent + ". " + timeUntilNextEvent + " seconds.");
        timeSinceLastEvent = 0.0f;
    }


    /// <summary>
    /// Handles assigning incoherence to all objects in scene.
    /// </summary>
    public void HandleObjects()
    {
        // Get a list of all objects to be affected.
        affectedObjects = new List<GameObject>();

        // Get a list of interactive objects in the scene.
        foreach (InteractionSettings intSet in FindObjectsOfType<InteractionSettings>())
        {
            affectedObjects.Add(intSet.transform.parent.gameObject);
        }

        // If the game is ready to break completely, add non interactive game objects to this list.
        if (globalIncoherence >= affectStaticObjectThreshold)
        {
            foreach (GameObject oops in FindObjectsOfType<GameObject>())
            {
                affectedObjects.Add(oops);
            }
        }

        ReplaceObjects();
        ApplyIncoherence();
    }


    void ApplyIncoherence()
    {
        float staticChance = MiscFunctions.Map(globalIncoherence, affectStaticObjectThreshold, 1f, 0f, 0.5f);

        foreach (GameObject gameObject in affectedObjects)
        {
            if (gameObject.GetComponentInChildren<InteractionSettings>() != null || (gameObject.GetComponentInChildren<InteractionSettings>() == null && Random.value <= staticChance))
            {
                // If this game object does not already have an incoherence controller, give it one.
                if (gameObject.GetComponentInChildren<IncoherenceController>() == null)
                {
                    Instantiate(incoherenceController, gameObject.transform);
                }

                // Set incoherence of this object based on global incoherence.
                gameObject.GetComponentInChildren<IncoherenceController>().incoherenceProbability = Random.Range(0f, globalIncoherence);
                gameObject.GetComponentInChildren<IncoherenceController>().incoherenceMagnitude = Random.Range(0f, globalIncoherence);
            }
        }
    }


    void ReplaceObjects()
    {
        // Make sure we're above the threshold.
        if (globalIncoherence < replaceObjectThreshold) return;

        // Get the probability that any item will be replaced.
        float npcChance = MiscFunctions.Map(globalIncoherence, inanimateObjectNPCThreshold, 1f, 0f, 0.7f);
        float normalChance = MiscFunctions.Map(globalIncoherence, replaceObjectThreshold, 1f, 0f, 0.5f);
        //Debug.Log("global incoherence: " + globalIncoherence + ". chance of replacement: " + normalChance);
        float staticChance = MiscFunctions.Map(globalIncoherence, affectStaticObjectThreshold, 1f, 0f, 0.5f);

        for (int i = 0; i < affectedObjects.Count; i++)
        {
            if (affectedObjects[i].GetComponentInChildren<InteractionSettings>() != null && Random.value <= normalChance)
            {
                ReplaceObject(affectedObjects[i].gameObject);
            }

            else if (globalIncoherence >= affectStaticObjectThreshold && Random.value <= staticChance)
            {
                //ReplaceObject(affectedObjects[i].gameObject);
            }

            if (affectedObjects[i].GetComponentInChildren<InteractionSettings>() != null && Random.value <= npcChance)
            {
                TurnObjectIntoNPC(affectedObjects[i].gameObject);
            }
        }
    }


    void TurnObjectIntoNPC(GameObject target)
    {
        if (target.GetComponentInChildren<NPC>() != null) return;

        Instantiate(NPCAI, target.transform);
    }


    void ReplaceObject(GameObject target)
    {
        // Create a new object.
        GameObject newObject = (GameObject) Instantiate(
			Services.Prefabs.PREFABS[(int)Services.TYPES.NPCs][Random.Range(0, Services.Prefabs.PREFABS[(int)Services.TYPES.NPCs].Length)],
            target.transform.position,
            target.transform.rotation);

        //Debug.Log("Replacing " + target.name + " with " + newObject.name);

        // Destroy old object.
        Destroy(target);
    }


	public void TallyIncoherence()
    {
        float totalIncoherence = 0f;
        int incoherenceNum = 0;

        // If the average incoherence of the level has increased, apply that to the global incoherence.
        foreach(IncoherenceController inCon in FindObjectsOfType<IncoherenceController>())
        {
            totalIncoherence += inCon.incoherenceMagnitude;
            incoherenceNum++;
        }

        totalIncoherence /= incoherenceNum;
        //Debug.Log("Average incoherence in this level: " + totalIncoherence);

        if (totalIncoherence > globalIncoherence)
            globalIncoherence = totalIncoherence;

        //Debug.Log("Global incoherence set to: " + globalIncoherence);

        // See if the threshold for any incoherence events has been added.
        if (dormantEvents.Count > 0)
        {
            for (int i = 0; i < dormantEvents.Count; i++)
            {
                if (globalIncoherence >= dormantEvents[i].threshold)
                {
                    Debug.Log("adding event.");
                    activeEvents.Add(dormantEvents[i]);
                    dormantEvents.Remove(dormantEvents[i]);
                }
            }
        }
    }
}
