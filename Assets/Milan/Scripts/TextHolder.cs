using UnityEngine;
using System.Collections;

public class TextHolder : MonoBehaviour {

	public float lineLength = 50;
	public float tracking = 0.25f;
	public float leading = 1;
	public Font[] fonts;
	public GameObject textPrefab;
	public bool fade, noRotation, delete;
	public float speed; 

	Vector3 	lastWordPosition;
	string[]	words;
	int 		wordCount, lineCount;

	void Start () {
		wordCount = 0;
		lineCount = 0;
		lastWordPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public string GetText(){
		return words[wordCount % words.Length];
	}

	public void SetText(string input){
		if (input != null) {
			words = input.Split (new char[] { ' ' });
			lineCount++;
		}
		wordCount = 0;
	}

	public void SetWords(string[] input){
		words = input;
		wordCount = 0;
	}

	public IEnumerator WriteText(){
		int wordIndex = 0;
		while(wordIndex < words.Length){
			if (wordCount > lineLength) {
				wordCount = 0;
				lastWordPosition.x = transform.position.x;
				lastWordPosition.y -= (float)lineCount * leading;
			}else{
				GameObject newWord = CreateWord (lastWordPosition);
				newWord.AddComponent<BoxCollider2D> ();
				lastWordPosition.x += newWord.GetComponent<BoxCollider2D>().bounds.size.x + tracking;
			}	
			yield return new WaitForSeconds(speed);
		}
	}

	public GameObject CreateWord(Vector3 pos){
		GameObject newWord = (GameObject)Instantiate (textPrefab, pos, Quaternion.identity);
		newWord.GetComponent<TextFormatting> ().setText (words [wordCount % words.Length]);
		Font curFont = fonts [Random.Range (0, fonts.Length)];
		newWord.GetComponent<TextMesh> ().font = curFont;
		newWord.transform.localScale *= Random.Range (0.75f, 1.2f);
		newWord.GetComponent<Renderer> ().sharedMaterial = curFont.material;
		TextFormatting t = newWord.GetComponent<TextFormatting> ();
		t.fade = fade;
		t.rotFixed = noRotation;
		t.delete = delete;
		t.fadeIn = fade;
		t.speed = speed;
		wordCount++;

		return newWord;
	}

	public void CreateParagraph(){
		float letterSpacing = 0.25f;
		wordCount = 0;
		Vector3 lastWord = transform.position;
		lastWord.y -= (float)lineCount/1.33f;
		for (int i = 0; i < words.Length; i++) {
			GameObject newWord = CreateWord (lastWord);
			newWord.AddComponent<BoxCollider2D> ();
			lastWord.x += newWord.GetComponent<BoxCollider2D>().bounds.size.x + letterSpacing;
		}
	}
}
