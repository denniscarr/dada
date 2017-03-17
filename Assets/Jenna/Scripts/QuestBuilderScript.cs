using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// ******* I AM NOT CERTAIN IF THIS SHOULD GO IN THE QUEST MANAGER, OR IF IT SHOULD BE ADDED
// TO QUESTABLE OBJECTS VIA THE QUESTFINDER SCRIPT; RIGHT NOW ON MANAGER**********

public class QuestBuilderScript : MonoBehaviour {

	// quest finder
	public QuestFinderScript finder;

	// quest manager
	public QuestManager manager;

	// pickup quest
	public PickupQuest pickup;

	// quest object script
	public QuestObject objectScript;

	// stuff for words
	public string partsOfSpeech = "speech.txt"; 					// public var for the file to read/write
	public List<string> verbs = new List<string>(); 				// list of verbs
	public List<string> nouns = new List<string>();					// list of nouns
	const char DELIMITER = '|';										// split here
	public int length, ranger, currentVerb, currentNoun;			// length/current word variables

	// stuff for picking objects
	public int questThing;
	public GameObject objeto;

	// Use this for initialization
	void Start () {

		// get scripts
		finder = this.gameObject.GetComponent<QuestFinderScript> ();
		manager = this.gameObject.GetComponent<QuestManager> ();
		pickup = this.gameObject.GetComponent<PickupQuest> ();


	// *********** STREAM READER/STREAM WRITER ***********
//		// print dataPath
//		Debug.Log ("Path: " + Application.dataPath);
//
//		// create a string variable to the path of the file we want to use
//		string finalFilePath = Application.dataPath + "/" + partsOfSpeech;
//
//		// make a streamwriter and then write to it
//		// it will be dependent on the length of the verbs string. the "true" means it
//		// constantly adds to one file -- which might be cool for having bleed.
//		StreamWriter sw = new StreamWriter(finalFilePath, true);
//
//		for (int i = 0; i < verbs.Count; i++) {
//			sw.WriteLine (verbs[i] + "|" + nouns[i]);
//		}
//
//		sw.Close ();
//
		// read from the file that exists. In the near future, I will use this to at least pop-
		// ulate to the quest manager, even if it's not functional, but this is a good skeleton.
		// will also have to make this work looking for objects that have can be used for quests
		// true as well as with the quest object script
//		StreamReader sr = new StreamReader(finalFilePath);
//
//		while (!sr.EndOfStream){
//			// read a line
//			string line = sr.ReadLine ();
//
//			// split line based on what's before the split and what's after
//			string[] splitLine = line.Split(DELIMITER);
//
//			// get the value out of the first part of the array
//			string verb = splitLine[0];
//
//			// get the second part of the array
//			string noun = splitLine[1];
//		}
//
//		sr.Close ();
	// *********** END STREAMS ***********

		// FIND EACH GAME OBJECT IN INTERACTABLES AND ADD ONE QUEST ID FOR EACH ITEM
		// this is where I randomize based on the streamreader whoooooa
//		foreach (GameObject go in finder.interactables) {
//			
//		}

		// setting up a clean slate for words
		currentVerb = 0;
		currentNoun = 0;

	}
	
	// Update is called once per frame
	void Update () {

		ranger = Random.Range (0, finder.questItems.Count);
		length = finder.questItems.Count;

		if (Input.GetKeyDown(KeyCode.Q)) {
			Generate ();
		}
	}

	// will fix this to generate at runtime and not, like, on a button press
	// but this will do for the prototype -- maybe instead of "q" make it a button that
	// a user is likely to press anyway
	public void Generate() {
		// MAKE NEW PICKUP QUEST AND ADD TO LIST
		// pick an object for it
		questThing = ranger % length;
		objeto = finder.questItems [Random.Range (0, finder.questItems.Count) % finder.questItems.Count];
		objectScript = objeto.GetComponent<QuestObject> ();

		// add the script yay
		PickupQuest newQuest = objeto.AddComponent<PickupQuest>();

		// first set up some multipliers
		length = verbs.Count - 1;
		//make sure not to divide by zero
		if (length == 0){
			length = 1;
		}

		ranger = Random.Range (-verbs.Count, verbs.Count);
		currentVerb = ((currentVerb + ranger) % length);
		currentNoun = ((currentNoun + ranger) % length);

		pickup.makeTheQuest (newQuest);
		manager.questList.Add (newQuest);
	}
}
