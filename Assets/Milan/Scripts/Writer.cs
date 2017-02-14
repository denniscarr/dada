using UnityEngine;
using System.Collections;

public class Writer : MonoBehaviour {

	public float lineLength = 50;
	public float tracking = 0.25f;
	public float leading = 1;
	public Font[] fonts;
	public GameObject textPrefab;
	public bool fade, noRotation, delete, WordbyWord;
	public float delay; 
	public float fadeSpeed;
	public TextAsset sourceText;
	Vector3 originalPos;

	Vector3 	spawnPosition;
	string[][]	_script;
	int 		wordIndex, lineIndex;
	int 		stringIndex;

	void Start () {
		originalPos = transform.position;
		wordIndex = 0;
		lineIndex = 0;
		stringIndex = 0;
		spawnPosition = transform.position;

		string[] tempText = sourceText.text.Split(new char[] { '\n' });
		_script = new string[tempText.Length][];

		for (int i = 0; i < tempText.Length; i++) {
				_script [i] = tempText [i].Split (new char[] { ' ' });
		}
	}

	public string[]GetCurrentString(){
		return _script[stringIndex % _script.Length];
	}

	public void SetAndSplitString(string input){
		if (input != null) {
			_script[stringIndex] = input.Split (new char[] { ' ' });
		}
	}

	public void SetString(string[] input){
		_script[stringIndex] = input;
	}

	public IEnumerator WriteText(){
		Debug.Log ("Write line");

		string line = "";

		foreach (string s in _script [stringIndex]) {
			line += s;
		}
		Debug.Log (line);

		foreach (string s in _script [stringIndex]) {
			CreateWord (spawnPosition);
			yield return new WaitForSeconds (delay);
		}
	}

	void UsedByPlayer() {
		if (WordbyWord) {
			CreateWord (spawnPosition);
		} else {
			StartCoroutine (WriteText ());
		}
	}

	public void CreateWord(Vector3 pos, Vector3 rotation = default(Vector3)){
		GameObject newWord = (GameObject)Instantiate (textPrefab, pos, Quaternion.identity);
		TextStyling t = newWord.GetComponent<TextStyling> ();
		t.setText (_script [stringIndex][wordIndex]);
		Font curFont = fonts [Random.Range (0, fonts.Length)];
		newWord.GetComponent<TextMesh>().font = curFont;
//		newWord.GetComponent<Renderer> ().sharedMaterial = curFont.material;
		newWord.AddComponent<BoxCollider2D> ();
		spawnPosition.x += newWord.GetComponent<BoxCollider2D> ().bounds.size.x + tracking;

		t.fade = fade;
		if (!noRotation) {
			t.transform.rotation = Quaternion.Euler (rotation); 
		}
		t.delete = delete;
		t.fadeIn = fade;
		t.speed = fadeSpeed;
		wordIndex++;

		if (wordIndex > _script [stringIndex].Length -1) {
			wordIndex = 0;
			spawnPosition.x = originalPos.x;
			spawnPosition.y -= leading;
			stringIndex++;
			if (stringIndex > _script.Length -1) {
				stringIndex = 0;
				spawnPosition.y = originalPos.y;
			}
		}
	}
}
