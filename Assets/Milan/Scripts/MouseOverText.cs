using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverText : MonoBehaviour {
	public string[] text;
	public Color[] color;

	public TextMesh textM;
	// Use this for initialization
	void Start () {


		foreach (Transform t in gameObject.GetComponentsInChildren<Transform>()) {
			if (t.gameObject.GetComponent<MouseOverText> () == null) {
				t.gameObject.AddComponent<MouseOverText> ();
				t.gameObject.GetComponent<MouseOverText> ().text = text;
				t.gameObject.GetComponent<MouseOverText> ().color = color;
				t.gameObject.GetComponent<MouseOverText> ().textM = textM;
			}
		}

	}
	public void OnMouseEnter(){
		textM.text = text[Random.Range (0, text.Length)];
		textM.color = color[Random.Range(0, color.Length)];
		textM.transform.position = transform.position;
		textM.transform.LookAt (Services.Player.transform.position);
		textM.transform.Rotate (0, 180, 0);
	}

	public void OnMouseExit(){
		textM.text = "";
	}
}
