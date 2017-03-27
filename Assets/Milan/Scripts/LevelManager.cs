using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SimpleManager.Manager<Level> {
	public GameObject SceneText;
	public static LevelManager levelManager;
	public Level currentLevel;
    public int levelNum = 0;
	public int width, length, height;
	public float tileScale = 1;
	public float[] NoiseRemapping;
	public float perlinFrequency = 0.02f;
	private float xOffset, yOffset;
	private Texture2D[] maps;
	private Gradient gradient;

	void Awake()
    {
        if (levelManager == null) {
			levelManager = this;
		} else if (levelManager != this) {
			Destroy (gameObject);
		}
	}

	void Start()
	{

        //SceneManager.sceneLoaded += OnSceneChange;

        maps = Resources.LoadAll<Texture2D> ("maps") as Texture2D[];

		Level.xOrigin = Random.Range (0, 10000);
		Level.yOrigin = Random.Range (0, 10000);
		Level.noiseScale = perlinFrequency;
		gradient = new Gradient ();

        Create();

    }

	void Update()
    {
		Services.Player = GameObject.Find ("Player");
		if (currentLevel != null) {
			Camera.main.backgroundColor = gradient.Evaluate (((Services.Player.transform.position.y - currentLevel.transform.position.y) / ((float)height * (float)tileScale)));
		}
		RenderSettings.fogColor = Camera.main.backgroundColor;

		//Debug.Log(SceneManager.GetActiveScene().buildIndex);

        // If the player has jumped off the level.
		if (Services.Player.transform.position.y < -20)
        {
            Services.Player = GameObject.Find ("Player");

            //Services.Player.transform.position = Vector3.zero;
            //Services.Player.transform.rotation = Quaternion.identity;

            levelNum++;

			if (currentLevel != null) currentLevel.enabled = false;

            Create();
        }
    }

	void SetGradient() {
		GradientColorKey[] gck;
		GradientAlphaKey[] gak;

		gck = new GradientColorKey[8];
		gak = new GradientAlphaKey[2];
		gak[0].alpha = 1.0F;
		gak[0].time = 0.0F;
		gak[1].alpha = 1.0F;
		gak[1].time = 1.0F;

		for (int j = 0; j < gck.Length; j++) {
			gck [j].color = currentLevel.palette [Mathf.RoundToInt(((j)/(float)gck.Length) * (float)currentLevel.palette.Length)];
			gck [j].time = (j)/(float)gck.Length;
		}
		gradient.SetKeys(gck, gak);

		Camera.main.clearFlags = CameraClearFlags.Color;
	}

	public override Level Create(){
		NoiseRemapping = new float[Random.Range (8, 15)];

		for(int i = 0; i < NoiseRemapping.Length; i++){
			NoiseRemapping[i] = Random.Range(0.00f, 1.00f);
		}

		GameObject newLevel = new GameObject();
		Level l = newLevel.AddComponent <Level> ();
		currentLevel = l;

		newLevel.transform.position = Services.Player.transform.position;
		newLevel.name = "Level " + ManagedObjects.Count;

		if (maps.Length > 0) {
			l._bitmap = maps [Random.Range (0, maps.Length)];
		}
			
		l.OnCreated ();

		GameObject t = (GameObject)Instantiate (SceneText, Vector3.zero, Quaternion.identity);
		t.transform.parent = newLevel.transform;
		t.transform.localPosition = new Vector3 (width / 2, height / 2, length / 2) * tileScale;
		t.GetComponent<TextMesh> ().text = "Abandon all hope, \n\' ye who enter here";

        Services.IncoherenceManager.HandleObjects();
		GameObject.Find ("QuestManager").SendMessage ("FindQuests");

        SetGradient();
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

    //void OnSceneChange(Scene scene, LoadSceneMode mode)
    //{
    //    GameObject.Find("Bootstrapper").GetComponent<GameManager>().Init();
    //    Create();
    //} 
}
