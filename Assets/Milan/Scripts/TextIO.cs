using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TextIO : MonoBehaviour {

	public TextAsset sourceText;
	public string[][] _script;


	// Use this for initialization
	void Start () {
		_script = new string[100][];
		string[] tempText = sourceText.text.Split("\r"[0]);
		for (int i = 0; i < tempText.Length; i++) {
			if (tempText[i].Length > 1) {
				_script [i] = tempText [i].Split (new char[] { ' ' });
				GetComponent<Writer> ().SetString (_script [i]);
			}
		}
	}
}
