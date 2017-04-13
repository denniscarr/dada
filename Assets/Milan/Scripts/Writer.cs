﻿using UnityEngine;
using System.Collections;

public class Writer : MonoBehaviour {

	public Color textColor = Color.white;
	float lineLength = 3f;
    float lineSpacing = 1f;
    public float textSize = 0.1f;
	public float tracking = 0.1f;
	public float leading = 1;
	public Font[] fonts;
	public GameObject textPrefab;
	public bool fade, noRotation, delete, WordbyWord;
	public float delay; 
	public float fadeSpeed;
	public TextAsset sourceText;
	public Vector3 originalPos;

    public Transform permaText;

	public Vector3 	spawnPosition;
	string[][]	_script;
	int 		wordIndex, lineIndex;
	int 		stringIndex;
	public float xOffset, yOffset, zOffset;

    // Used for cooldown.
    public float cooldownTime = 0.5f;
    float timeSinceLastWrite = 0;

	void Start () {
		wordIndex = 0;
		lineIndex = 0;
		stringIndex = 0;
		originalPos = new Vector3(xOffset, yOffset, zOffset);
		spawnPosition = originalPos;
//		if (sourceText != null) {
//			SetScript (sourceText.text);
//		}
	}

    private void Update()
    {
        timeSinceLastWrite += Time.deltaTime;
    }

    public void SetScript(string _text)
    {
        string[] tempText = _text.Split(new char[] { '\n' });

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

		string line = "";

		foreach (string[] s in _script) {
			foreach (string w in s) {
				CreateWord (spawnPosition);
				yield return new WaitForSeconds (delay);
			}
		}
			
	}

	void UsedByPlayer() {
		if (WordbyWord) {
//			CreateWord (spawnPosition);
		} else {
			StartCoroutine (WriteText ());
		}
	}

    public void WriteSpecifiedString(string _text)
    {
        SetScript(_text);

        CreateTextBox (transform.position);
    }

    public void WriteAtPoint(string _text, Vector3 position)
    {
        SetScript(_text);

        CreateTextBox(position);
    }


    public void DeleteTextBox()
    {
        if (permaText != null)
        {
            Destroy(permaText.gameObject);
            timeSinceLastWrite = cooldownTime;
        }
    }


    public void CreateTextBox(Vector3 basePosition)
    {
        if (timeSinceLastWrite < cooldownTime) return;

        basePosition.y += 2f;

		spawnPosition = Vector3.zero;

        GameObject textContainer = new GameObject("Text Container");
        permaText = textContainer.transform;
        textContainer.transform.position = basePosition;

        while (wordIndex < _script [stringIndex].Length)
        {
            // Instantiate the text object.
            GameObject newWord = (GameObject) Instantiate(textPrefab, basePosition, Quaternion.identity);
            newWord.transform.parent = textContainer.transform;

            // Set styling & text for the next word.
            TextStyling textStyling = newWord.GetComponent<TextStyling>();
            textStyling.setText(_script[stringIndex][wordIndex]);
            Font currentFont = fonts[Random.Range(0, fonts.Length - 1)];

            newWord.GetComponent<TextMesh>().font = currentFont;
            newWord.GetComponent<TextMesh>().color = textColor;
            newWord.GetComponent<Renderer>().sharedMaterial = currentFont.material;

            // Set this word's local position.
            newWord.transform.localPosition = spawnPosition;
            newWord.transform.localScale = new Vector3(textSize, textSize, textSize);

            // Text styling stuff.
            textStyling.fade = fade;
            textStyling.delete = delete;
            textStyling.fadeIn = fade;
            textStyling.speed = fadeSpeed;

            // Get the position of the next word.
            spawnPosition.x += (newWord.GetComponent<Renderer>().bounds.size.x + tracking);

            // If the next word would appear outside the space set aside per line, go to the next line.
            if (spawnPosition.x > lineLength)
            {
                spawnPosition.y -= lineSpacing;
                spawnPosition.x = 0;
            }

            wordIndex += 1;
        }

        // Rotate the text containter towards the player.
        textContainer.transform.LookAt(Services.Player.transform);
        textContainer.transform.Rotate(0f, 180f, 0f);

        // Set all values back to zero.
        wordIndex = 0;
		spawnPosition = Vector3.zero;

        timeSinceLastWrite = 0f;
    }

	public void CreateWord(Vector3 pos, Vector3 rotation = default(Vector3))
    {
		GameObject newWord = (GameObject)Instantiate (textPrefab, Vector3.zero, Quaternion.identity);
        newWord.transform.parent = transform;

		TextStyling t = newWord.GetComponent<TextStyling> ();
		t.setText (_script [stringIndex][wordIndex]);
		Font curFont = fonts [Random.Range (0, fonts.Length-1)];
		newWord.GetComponent<TextMesh>().font = curFont;
		newWord.GetComponent<TextMesh> ().color = textColor;
		newWord.GetComponent<Renderer> ().sharedMaterial = curFont.material;
		newWord.AddComponent<BoxCollider2D> ();
		newWord.transform.localPosition = pos;
		newWord.transform.localScale = new Vector3 (textSize, textSize, textSize);
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
