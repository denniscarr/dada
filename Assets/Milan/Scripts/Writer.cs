using UnityEngine;
using System.Collections;

public class Writer : MonoBehaviour {

	public Color textColor = Color.white;
	public float lineLength = 3f;
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
	Vector3 originalPos;
	Transform pivot;
    public Transform permaText;
    public bool dontFacePlayer = false;

	Vector3 	spawnPosition;
	Vector3		spawnPosition2;
	string[][]	_script;
    public string lastWrite = "";
	int 		wordIndex, lineIndex;
	int 		stringIndex;
	public float xOffset, yOffset, zOffset;

    // Used for cooldown.
    public float cooldownTime = 0.5f;
    float timeSinceLastWrite = 0;

	void Awake () {
		wordIndex = 0;
		lineIndex = 0;
		stringIndex = 0;
		originalPos = new Vector3(xOffset, yOffset, zOffset);
		pivot = new GameObject().transform;
		pivot.transform.parent = transform;
		pivot.transform.localRotation = Quaternion.Euler (Vector3.zero);
		pivot.transform.localPosition = originalPos;
		spawnPosition = Vector3.zero;
		spawnPosition2 = Vector3.zero;
//		if (sourceText != null) {
//			SetScript (sourceText.text);
//		}
	}

    private void Update()
    {
        timeSinceLastWrite += Time.deltaTime;
		originalPos = pivot.transform.localPosition;
    }

    public void SetScript(string _text)
    {
        lastWrite = _text;

        string[] tempText = _text.Split(new char[] { '\n' });

        _script = new string[tempText.Length][];

        for (int i = 0; i < tempText.Length; i++) {
            _script [i] = tempText [i].Split (new char[] { ' ' });
        }

		stringIndex = 0;
		lineIndex = 0;
		wordIndex = 0;
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

		stringIndex = 0;
		lineIndex = 0;
		wordIndex = 0;

		foreach (string[] s in _script) {
			foreach (string w in s) {
				CreateWord (spawnPosition2);
				yield return new WaitForSeconds (delay);
			}
		}
		spawnPosition2 = Vector3.zero;

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

        CreateTextBox (transform.position, false);
    }


    public void WriteAtPoint(string _text, Vector3 position)
    {
        if (permaText != null) return;

        SetScript(_text);

        CreateTextBox(position, false);
    }


    public void DeleteTextBox()
    {
        if (permaText != null)
        {
            Destroy(permaText.gameObject);
            timeSinceLastWrite = cooldownTime;
        }
    }


    public void CreateTextBox(Vector3 basePosition, bool parentToMe)
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
        if (!dontFacePlayer)
        {
            textContainer.transform.LookAt(Services.Player.transform);
            textContainer.transform.Rotate(0f, 180f, 0f);
        }

        if (parentToMe)
        {
            textContainer.transform.SetParent(transform);
        }

        // Set all values back to zero.
        wordIndex = 0;
		spawnPosition = Vector3.zero;
        
        timeSinceLastWrite = 0f;
    }

	public void CreateWord(Vector3 pos, Vector3 rotation = default(Vector3))
    {
		GameObject newWord = (GameObject)Instantiate (textPrefab, Vector3.zero, Quaternion.identity);
        newWord.transform.parent = pivot;

		TextStyling t = newWord.GetComponent<TextStyling> ();
		t.setText (_script [stringIndex][wordIndex]);
		Font curFont = fonts [Random.Range (0, fonts.Length-1)];
		newWord.GetComponent<TextMesh>().font = curFont;
		newWord.GetComponent<TextMesh> ().color = textColor;
		newWord.GetComponent<Renderer> ().sharedMaterial = curFont.material;
		newWord.AddComponent<BoxCollider2D> ();
//		newWord.transform.LookAt(Services.Player.transform.position);
//		newWord.transform.Rotate (0, 180, 0);
		newWord.transform.localScale = Vector3.one * textSize;
		Debug.Log (newWord.GetComponent<TextMesh>().text + "is " + newWord.GetComponent<MeshRenderer> ().bounds.size.x + " wide");
		spawnPosition2.x += newWord.GetComponent<MeshRenderer> ().bounds.size.x + tracking;
		newWord.transform.localPosition = pos;
		newWord.transform.localRotation = Quaternion.Euler (Vector3.zero);
		t.fade = false;

//		if (!noRotation) {
//			t.transform.rotation = Quaternion.Euler (rotation); 
//		}

		t.delete = delete;
		t.fadeIn = false;
		t.speed = fadeSpeed;
		wordIndex++;

		if (wordIndex > _script [stringIndex].Length -1){
			wordIndex = 0;
			spawnPosition2.x = 0;
			spawnPosition2.y -= leading;
			stringIndex++;
			if (stringIndex > _script.Length -1) {
				stringIndex = 0;
//				spawnPosition.y = originalPos.y;
			}
		}
	}
}
