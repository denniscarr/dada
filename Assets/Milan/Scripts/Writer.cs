using UnityEngine;
using System.Collections;

public class Writer : MonoBehaviour {

	public float lineLength = 50;
	public float tracking = 0.25f;
	public float leading = 1;
	public Font[] fonts;
	public GameObject textPrefab;
	public bool fade, noRotation, delete;
	public float speed; 

	Vector3 	spawnPosition;
	string[][]	_script;
	int 		wordIndex, lineIndex;
	int 		stringIndex;

	void Awake () {
		wordIndex = 0;
		lineIndex = 0;
		stringIndex = 0;
		spawnPosition = transform.position;
		_script = new string [100][];
	}

	public string[]GetCurrentString(){
		return _script[stringIndex % _script.Length];
	}

	public void SetAndSplitString(string input){
		if (input != null) {
			_script[stringIndex] = input.Split (new char[] { ' ' });
			stringIndex++;
		}
		wordIndex = 0;
	}

	public void SetString(string[] input){
		_script[stringIndex] = input;
		wordIndex = 0;
	}

	public IEnumerator WriteText(){
		int wordCount = 0;
		int lineCount = 0;
		while(wordIndex < _script[lineIndex %_script.Length].Length){
			if (wordIndex > lineLength) {
				lineIndex++;
				wordIndex = 0;
				spawnPosition.x = transform.position.x;
				spawnPosition.y -= (float)lineIndex * leading;
			}else{
				GameObject newWord = CreateWord (spawnPosition);
				newWord.AddComponent<BoxCollider2D> ();
				spawnPosition.x += newWord.GetComponent<BoxCollider2D>().bounds.size.x + tracking;
			}	
			yield return new WaitForSeconds(speed);
		}
		stringIndex++;
	}

	void UsedByPlayer() {
		StartCoroutine (WriteText ());
	}

	public GameObject CreateWord(Vector3 pos, Vector3 rotation = default(Vector3)){
		GameObject newWord = (GameObject)Instantiate (textPrefab, pos, Quaternion.identity);
		TextStyling t = newWord.GetComponent<TextStyling> ();
		t.setText (_script [lineIndex][wordIndex % _script[lineIndex].Length]);
		Font curFont = fonts [Random.Range (0, fonts.Length)];
		newWord.GetComponent<TextMesh>().font = curFont;
		newWord.GetComponent<Renderer> ().sharedMaterial = curFont.material;
		newWord.transform.localScale *= Random.Range (0.75f, 1.2f);
		t.fade = fade;
		if (!noRotation) {
			t.transform.rotation = Quaternion.Euler (rotation); 
		}
		t.delete = delete;
		t.fadeIn = fade;
		t.speed = speed;
		wordIndex++;

		return newWord;
	}
}
