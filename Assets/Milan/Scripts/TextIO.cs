using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TextIO : MonoBehaviour {

	public float speed;
	public Font[] fonts;
	public TextAsset sourceText;

	public static string[][] _script;

	float time;
	public float interval = 0.25f; //interval to change words at
	int wordCount; //how far the words are going
	int lineCount;
	float letterSpacing = 0.25f;
	Vector3 lastPosition;
	/*
	 * box of 600x400 fits 1254 chara at 14pt 2 in Space Mono
	 * 66 chara per line, 19 lines
	 * 
	 */

	// Use this for initialization
	void Start () {
		string[] tempText = sourceText.text.Split("\r"[0]);
		for (int i = 0; i < tempText.Length; i++) {
			_script [i] = tempText[i].Split (new char[] { ' ' });
		}
	}
}
