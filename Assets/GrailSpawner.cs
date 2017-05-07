using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrailSpawner : MonoBehaviour {

    [SerializeField] private GameObject grailPrefab;
    [SerializeField] public bool grailHasSpawned = false;


    private void Update()
    {
        if (GameObject.Find("QuestManager").GetComponent<QuestManager>().allQuestsCompleted)
        {
            SpawnGrail();
        }
    }


    public void SpawnGrail()
    {
        if (grailHasSpawned) return;

        GameObject grail = Instantiate(grailPrefab);
        grail.transform.position = Services.LevelGen.currentLevel.transform.position;
        grail.transform.position += Vector3.up * 100f;
        grailHasSpawned = true;
    }
}
