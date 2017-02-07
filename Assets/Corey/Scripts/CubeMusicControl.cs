using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMusicControl : MonoBehaviour {



	public GameObject[] SoundCubes;

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			// rotate cube 1
			// switch start time to orig. start time + time/4

			SoundCubes [0].transform.Rotate (new Vector3 (0, 90f, 0));

		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			// rotate cube 2
			SoundCubes [1].transform.Rotate (new Vector3 (0, 90f, 0));
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			// rotate cube 3
			SoundCubes [2].transform.Rotate (new Vector3 (0, 90f, 0));
		}  

		if ( Input.GetKeyDown(KeyCode.S)) {
			for (int i = 0; i < SoundCubes.Length; i ++){
				SoundCubes[i].GetComponent<CS_MusicRotate>().PlayCubeClip();
			}
		}

	}
}
