using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderScript : MonoBehaviour {

	//public bool clickLoader;

	public string[] levels;
	public string sceneToRun;

	public string[] questText;

	// Use this for initialization
	void Start () {
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

				//if (Input.GetKey (KeyCode.Space)){
				SceneManager.LoadScene (sceneToRun);
			}
		}
	}

	//void onOverlapEnter
}
