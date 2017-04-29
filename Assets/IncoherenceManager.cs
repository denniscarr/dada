using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncoherenceManager : MonoBehaviour {

    public float globalIncoherence = 0.0f;

    public float interactionIncrease = 0.05f;  // How much an object's incoherence increases when the player interacts with that object.
    public float questIncrease = 0.1f;

    [SerializeField] float inanimateObjectNPCThreshold = 0.1f;  // How high global incoherence needs to be before we start turning random objects into NPCS.
    [SerializeField] float replaceObjectThreshold = 0.3f; // How high global incoherence needs to be before we start replacing interactive objects.
    [SerializeField] float affectStaticObjectThreshold = 0.9f; // How high global incoherence needs to be before we start affecting all game objects (bad things will happen.)

    // Prefab references.
    [SerializeField] GameObject NPCAI;
    [SerializeField] GameObject equipability;
    [SerializeField] GameObject incoherenceController;

    List<GameObject> affectedObjects;

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
        float normalChance = MiscFunctions.Map(globalIncoherence, replaceObjectThreshold, 1f, 0f, 1f);
        float staticChance = MiscFunctions.Map(globalIncoherence, affectStaticObjectThreshold, 1f, 0f, 0.5f);

        for (int i = 0; i < affectedObjects.Count; i++)
        {
            if (affectedObjects[i].GetComponentInChildren<InteractionSettings>() != null && Random.value <= normalChance)
            {
                ReplaceObject(affectedObjects[i].gameObject);
            }

            else if (Random.value <= staticChance)
            {
                ReplaceObject(affectedObjects[i].gameObject);
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

        Debug.Log("Replacing " + target.name + " with " + newObject.name);

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

        if (totalIncoherence > globalIncoherence)
            globalIncoherence = totalIncoherence;

        Debug.Log("Global incoherence: " + globalIncoherence);
    }
}
