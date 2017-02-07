using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderScript : MonoBehaviour {

	//public bool clickLoader;

	public string[] levels;
	public string sceneToRun;

	// Use this for initialization
	void Start () {
		//the example string for storing levels
		//it turns out that things have to be, um, exactly the name of the scene,
		//which makes sense. So, if you're looking at this later, be super precise.
		string[] levels = {"QuestSceneTest", "Blue", "Green", "Red"};
		sceneToRun = levels[Random.Range(0, (levels.Length -1 ) )];

	}

	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		transform.position = ray.origin + ray.direction * 2f;
		if (Physics.Raycast (ray, out hit)) {

			if (hit.collider.name.Contains ("Blue")) {
				SceneManager.LoadScene (sceneToRun);
			}
		}
	}

	//void onOverlapEnter
}
