using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderScript : MonoBehaviour {

	//	Ray ray;
	//	RaycastHit hit;

	// Use this for initialization
	void Start () {
		string[] sceneArray = {"Main Scene", "Blue", "Green", "Red"};
		string sceneToRun = sceneArray[Random.Range(0, sceneArray.Length-1)];

	}

	// Update is called once per frame
	void Update () {

		//if (Input.GetMouseButtonDown) {
		//load

		//right now I'm just going to see if it loads a random scene when space is pressed
		if (Input.GetKey (KeyCode.Space)){
			//load that level!
			//SceneManager.LoadScene(1);
			SceneManager.LoadScene("sceneToRun");


		}

	}

	//void onOverlapEnter
}
