using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleFont : MonoBehaviour {

	public Font[] fonts;
	TextMesh text;

	void Start(){
		text = GetComponent<TextMesh> ();
		int index = Random.Range (0, fonts.Length);
		text.font = fonts [index];
		text.font.material = fonts [index].material;
		InvokeRepeating ("ChangeFont", 0, 0.1f);
	}

	void ChangeFont(){
		int index = Random.Range (0, fonts.Length);
		text.font = fonts [index];
		if (Random.Range (0, 100) < 50) {
			GetComponent<Renderer> ().material = fonts [index].material;
		}
	}
}
