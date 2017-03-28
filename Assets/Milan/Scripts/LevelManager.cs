using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SimpleManager.Manager<Level> {

	public int[] ObjectTypes;

	public GameObject SceneText;
	public Level currentLevel;
    public int levelNum = 0;
	public int width, length, height;
	public float tileScale = 1;
	public float[] NoiseRemapping;
	public float perlinFrequency = 0.02f;
	private float xOffset, yOffset;
	private Texture2D[] maps;
	private Gradient gradient;


	void Start()
	{

        //SceneManager.sceneLoaded += OnSceneChange;
        maps = Resources.LoadAll<Texture2D> ("maps") as Texture2D[];

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);
		Level.noiseScale = perlinFrequency;

        Create();

    }

	void Update()
    {
        // If the player has jumped off the level.
		if (Services.Player.transform.position.y - currentLevel.transform.position.y < -10){
            
			Services.Player = GameObject.Find ("Player");
//			if (currentLevel != null) currentLevel.enabled = false;
            Create();
        }
    }

	public override Level Create(){

		if (currentLevel != null) {
			Destroy (currentLevel.gameObject);
		}

		NoiseRemapping = new float[20];

		NoiseRemapping [0] = 0;
		for(int i = 1; i < NoiseRemapping.Length; i++){
			NoiseRemapping [i] = Random.Range (0.00f, 1.00f);
			while (Mathf.Abs (NoiseRemapping [i] - NoiseRemapping [i - 1]) > 0.25f) {
				NoiseRemapping [i] = Random.Range (0.00f, 1.00f);
			}
		}

		GameObject newLevel = new GameObject();
		Level l = newLevel.AddComponent <Level> ();
		currentLevel = l;

		newLevel.transform.position = Services.Player.transform.position - (Vector3.up * 100);
		newLevel.name = "Level " + ManagedObjects.Count;

		if (maps.Length > 0) {
			l._bitmap = maps [Random.Range (0, maps.Length)];
		}
			
		l.OnCreated ();

		GameObject t = (GameObject)Instantiate (SceneText, Vector3.zero, Quaternion.identity);
		t.transform.parent = newLevel.transform;
		t.transform.localPosition = new Vector3 (width / 2, height, length / 2) * tileScale;
		t.GetComponent<TextMesh> ().text = "Abandon all hope, \n\' ye who enter here";

        Services.IncoherenceManager.HandleObjects();
		GameObject.Find ("QuestManager").SendMessage ("FindQuests");

		Level.xOrigin += width / Level.noiseScale;
		Level.yOrigin += height / Level.noiseScale;

		xOffset += width;
		yOffset += height;

        ManagedObjects.Add (l);
		return l;
	}

	public override void Destroy(Level l){
		ManagedObjects.Remove (l);
		Destroy (l);
	}

    void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneChange;
    }

    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        GameObject.Find("Bootstrapper").GetComponent<GameManager>().Init();
        Create();
    } 
}
