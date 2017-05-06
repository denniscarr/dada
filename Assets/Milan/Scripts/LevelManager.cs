using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enum = System.Enum;

public class LevelManager : SimpleManager.Manager<Level> {

	public Services.TYPES[] props;
	public bool showSky = false;
	public GameObject KillZone;
	public Level currentLevel;
	public Light cookieLight;
	public Light sun;
	public int maxNPCs, maxObjects, maxSprites;
    public int levelNum = 0;
	public int radius, height;
	public float tileScale = 1;
	public float[] NoiseRemapping;
	public float perlinFrequency = 0.02f;
	private float xOffset, yOffset;
	public TextAsset sourceText;
	string[] LevelDescriptions;
	Writer writer;

	public bool isTutorialCompleted = false;

	void Start()
	{
		isTutorialCompleted = false;
		LevelDescriptions = sourceText.text.Split(new char[] { '\n' });
		NoiseRemapping = new float[10];

        //SceneManager.sceneLoaded += OnSceneChange;
		writer = Services.Player.GetComponentInChildren<Writer>();
		maxNPCs = 0;
		maxObjects = 0;
		maxSprites = 0;


		radius = 25;
		height = 1;
		tileScale = 2;

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);
		Level.noiseScale = perlinFrequency;

		RenderSettings.fogEndDistance = 100;

        Create();
    }


	void Update()
    {
        // If the player has jumped off the level.
		if (isTutorialCompleted && Services.Player.transform.position.y - currentLevel.transform.position.y < - 10){
            
			Services.Player = GameObject.Find ("Player");
            //			if (currentLevel != null) currentLevel.enabled = false;
//            Services.IncoherenceManager.TallyIncoherence();
            Create();
        }
    }

	public override Level Create(){

		NoiseRemapping [0] = 0.5f;
		for(int i = 1; i < NoiseRemapping.Length; i++) {
//			NoiseRemapping [i] = NoiseRemapping [i-1] + Random.Range (-Services.IncoherenceManager.globalIncoherence, Services.IncoherenceManager.globalIncoherence);
			NoiseRemapping [i] = Random.Range (0.00f, Services.IncoherenceManager.globalIncoherence + 0.1f);
			NoiseRemapping [i] = Mathf.Clamp01 (NoiseRemapping [i]);
		}

		for (int j = 3; j < props.Length; j++) {
			props [j] = (Services.TYPES) Random.Range (0, Services.Prefabs.PREFABS.Length);
		}
		for (int m = 0; m < 10; m++) {
			props [Random.Range(0, props.Length)] = Services.TYPES.Sprite;
		}

		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
			GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject> ();
			foreach (GameObject go in allObjects) {
				if (go.GetComponentInChildren<InteractionSettings> () != null) {
					if (go.GetComponentInChildren<InteractionSettings>().carryingObject != Services.Player.transform) {
						if (go.GetComponentInChildren<InteractionSettings> ().transform.parent == go.transform) {
							Destroy (go);
						}
					}
				}
			}
		}

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);
//		NoiseRemapping = new float[20];
//
//		NoiseRemapping [0] = 0;
//		for(int i = 1; i < NoiseRemapping.Length; i++){
//			NoiseRemapping [i] = Random.Range (0.00f, 1.00f);
//			while (Mathf.Abs (NoiseRemapping [i] - NoiseRemapping [i - 1]) > 0.25f) {
//				NoiseRemapping [i] = Random.Range (0.00f, 1.00f);
//			}
//		}

		GameObject newLevel = new GameObject();
		Level l = newLevel.AddComponent <Level> ();
		currentLevel = l;

		newLevel.transform.position = Services.Player.transform.position - (Vector3.up * 100);
		cookieLight.transform.position = newLevel.transform.position;
		sun.transform.position = newLevel.transform.position;
		newLevel.name = "Level " + ManagedObjects.Count;

		l.OnCreated ();

		//writer.textSize = 0.25f;
		writer.SetScript (SetLevelText ());
		StartCoroutine (writer.WriteText ());

        Services.IncoherenceManager.HandleObjects();
	
		levelNum--;

        GetComponent<GrailSpawner>().grailHasSpawned = false;
        Services.Quests.allQuestsCompleted = false;
        Services.Quests.questsToComplete = levelNum + 2;
        GameObject.Find("QuestManager").GetComponent<QuestFinderScript>().FindQuests();

		maxNPCs += 1;
		maxObjects += 2;
		maxSprites += 50;
		radius += 10;
//		perlinFrequency += 0.020f;
		height += 5;
        if (levelNum < 0) Services.IncoherenceManager.TallyIncoherence();
		Services.IncoherenceManager.globalIncoherence += 0.05f;

        ManagedObjects.Add (l);
		return l;
	}

	public override void Destroy(Level l){
		ManagedObjects.Remove (l);
		Destroy (l);
	}

	string SetLevelText() {
		string line = "";
		line += ManagedObjects.Count + " floors down" + "\n" + "\n";
		line += LevelDescriptions[Random.Range(0, LevelDescriptions.Length)];

		return line;
	}
    
	void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneChange;
    }

    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        Create();
    } 
}
