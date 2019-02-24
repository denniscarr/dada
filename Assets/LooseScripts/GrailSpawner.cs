using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrailSpawner : MonoBehaviour {

    [SerializeField] private GameObject grailPrefab;
    [SerializeField] public bool grailHasSpawned = false;


    private void Update()
    {
        //if (GameObject.Find("QuestManager").GetComponent<QuestManager>().allQuestsCompleted)
        //{
        //    SpawnGrail();
        //}
    }


    public void SpawnGrail()
    {
        if (grailHasSpawned) return;

        GameObject.Find("Sun").GetComponent<Light>().DOIntensity(0.4f, 1f);
        Services.Player.GetComponentInChildren<ColorfulFog>().coloringMode = ColorfulFog.ColoringMode.Solid;
        Services.Player.GetComponentInChildren<ColorfulFog>().solidColor = Color.black;

        GameObject grail = Instantiate(grailPrefab);
        grail.transform.position = Services.LevelGen.currentLevel.transform.position;
        grail.transform.position += Vector3.up * 100f;

        grailHasSpawned = true;
    }


    public void TurnOnTheLights()
    {
        GameObject.Find("Sun").GetComponent<Light>().DOIntensity(1f, 1f);
        Services.Player.GetComponentInChildren<ColorfulFog>().coloringMode = ColorfulFog.ColoringMode.Gradient;
    }
}
