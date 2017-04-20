using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrailSpawner : MonoBehaviour {

    [SerializeField] private GameObject grailPrefab;
    [SerializeField] private bool grailHasSpawned = false;


    private void Update()
    {
        if (GameObject.Find("QuestManager").GetComponent<QuestManager>().allQuestsCompleted)
        {
            Debug.Log("All quests completed.");
            SpawnGrail();
        }
    }


    private void SpawnGrail()
    {
        if (grailHasSpawned) return;

        GameObject grail = Instantiate(grailPrefab);
        grail.transform.position = Services.LevelGen.currentLevel.transform.position;
        grail.transform.position += Vector3.up * 100f;
    }
}
