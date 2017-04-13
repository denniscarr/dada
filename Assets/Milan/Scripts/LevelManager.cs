using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SimpleManager.Manager<Level> {

	public Services.TYPES[] props;
	public bool showSky = false;
	public GameObject SceneText;
	public Level currentLevel;
	public Light cookieLight;
	public Light sun;
	public int maxNPCs, maxPickups, maxObjects;
    public int levelNum = 0;
	public int width, length, height;
	public float tileScale = 1;
	public float[] NoiseRemapping;
	public float perlinFrequency = 0.02f;
	private float xOffset, yOffset;
	private Texture2D[] maps;
	private Gradient gradient;
	Writer writer;
	void Start()
	{

        //SceneManager.sceneLoaded += OnSceneChange;
		writer = Services.Player.GetComponentInChildren<Writer>();
		writer.spawnPosition += Vector3.forward * 2;
		writer.spawnPosition += Vector3.right * 5;
		writer.originalPos = writer.spawnPosition;


        maps = Resources.LoadAll<Texture2D> ("maps") as Texture2D[];

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);
		Level.noiseScale = perlinFrequency;

		RenderSettings.fogEndDistance = 100;

        Create();

    }

	void Update()
    {
        // If the player has jumped off the level.
		if (Services.Player.transform.position.y - currentLevel.transform.position.y < -currentLevel.mapHeight * 2){
            
			Services.Player = GameObject.Find ("Player");
//			if (currentLevel != null) currentLevel.enabled = false;
            Create();
        }
    }

	public override Level Create(){

		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
			GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject> ();
			foreach (GameObject go in allObjects) {
				if (go.GetComponentInChildren<InteractionSettings> () != null) {
					if (go.GetComponentInChildren<InteractionSettings>().carryingObject != Services.Player.transform) {
						if (go.GetComponentInChildren<InteractionSettings> ().transform.parent == go.transform) {
							Debug.Log (go.name);
							Destroy (go);
						}
					}
				}
			}
		}

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

		newLevel.transform.position = Services.Player.transform.position - (Vector3.up * 200);
		cookieLight.transform.position = newLevel.transform.position;
		sun.transform.position = newLevel.transform.position;
		newLevel.name = "Level " + ManagedObjects.Count;

		if (maps.Length > 0) {
			l._bitmap = maps [Random.Range (0, maps.Length)];
		}
			
		l.OnCreated ();

		writer.textSize = 1;
		writer.SetScript (SetLevelText ());
		StartCoroutine (writer.WriteText ());

        Services.IncoherenceManager.HandleObjects();

		Level.xOrigin += width / Level.noiseScale;
		Level.yOrigin += height / Level.noiseScale;

		xOffset += width;
		yOffset += height;

		SetLevelText ();

		levelNum--;
        ManagedObjects.Add (l);

        GameObject.Find("QuestManager").SendMessage("FindQuests");

        return l;
	}

	public override void Destroy(Level l){
		ManagedObjects.Remove (l);
		Destroy (l);
	}

	string SetLevelText(){
		return "Act " + ManagedObjects.Count + ": A Forest \n" + width + " metres by " + height + " metres\n";
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
