using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncoherenceManager : MonoBehaviour {

    public float globalIncoherence = 0.0f;
    [SerializeField] float replaceObjectThreshold = 0.1f; // How high global incoherence needs to be before we start replacing objects.

    // Prefab references.
    [SerializeField] GameObject NPCAI;
    [SerializeField] GameObject equipability;

    /// <summary>
    /// Handles assigning incoherence to all objects in scene.
    /// </summary>
    public void HandleObjects()
    {
        ReplaceObjects();
    }


    void ReplaceObjects()
    {
        // Make sure we're above the threshold.
        if (globalIncoherence < replaceObjectThreshold) return;

        // Get the probability that any item will be replaced.
        float chance = MiscFunctions.Map(globalIncoherence, replaceObjectThreshold, 1f, 0f, 1f);

        // Get all incoherence managers in current scene.
        IncoherenceManager[] incoherencesInScene = FindObjectsOfType<IncoherenceManager>();

        for (int i = 0; i < incoherencesInScene.Length; i++)
        {
            if (Random.value <= chance)
            {
                ReplaceObject(incoherencesInScene[i].gameObject);
            }
        }

    }


    void ReplaceObject(GameObject target)
    {
        // Create a new object.
        GameObject newObject = (GameObject) Instantiate(
            Services.Prefabs.NPCPREFABS[Random.Range(0, Services.Prefabs.NPCPREFABS.Length)],
            target.transform.position,
            target.transform.rotation);

        // Destroy old object.
    }


	public void TallyIncoherence()
    {

    }
}
