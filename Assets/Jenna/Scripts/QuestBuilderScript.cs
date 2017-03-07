using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// ******* I AM NOT CERTAIN IF THIS SHOULD GO IN THE QUEST MANAGER, OR IF IT SHOULD BE ADDED
// TO QUESTABLE OBJECTS VIA THE QUESTFINDER SCRIPT **********

public class QuestBuilderScript : MonoBehaviour {

	// quest finder
	public QuestFinderScript finder;

	// quest manager
	public QuestManager manager;

	// quest object
	public QuestObject objeto;

	// stuff for words
	public string partsOfSpeech = "speech.txt"; 		// public var for the file to read/write
	public List<string> verbs = new List<string>(); 	// list of verbs
	public List<string> nouns = new List<string>();		// list of nouns
	const char DELIMITER = '|';

	// Use this for initialization
	void Start () {

		// get scripts
		finder = this.gameObject.GetComponent<QuestFinderScript> ();
		manager = this.gameObject.GetComponent<QuestManager> ();


	// *********** STREAM READER/STREAM WRITER ***********
		// print dataPath
		Debug.Log ("Path: " + Application.dataPath);

		// create a string variable to the path of the file we want to use
		string finalFilePath = Application.dataPath + "/" + partsOfSpeech;

		// make a streamwriter and then write to it
		// it will be dependent on the length of the verbs string, so we should take note of
		// that and make the verbs and nouns lists the same length because i don't know what
		// would happen otherwise. the "true" means it constantly adds to one file -- which
		// might be cool for having carryover in multiple play sessions. Otherwise, make false.
		StreamWriter sw = new StreamWriter(finalFilePath, true);

		for (int i = 0; i < verbs.Count; i++) {
			sw.WriteLine (verbs[i] + "|" + nouns[i]);
		}

		sw.Close ();

		// read from the file that exists. In the near future, I will use this to at least pop-
		// ulate to the quest manager, even if it's not functional, but this is a good skeleton.
		// will also have to make this work looking for objects that have can be used for quests
		// true as well as with the quest object script
		StreamReader sr = new StreamReader(finalFilePath);

		while (!sr.EndOfStream){
			// read a line
			string line = sr.ReadLine ();

			// split line based on what's before the split and what's after
			string[] splitLine = line.Split(DELIMITER);

			// get the value out of the first part of the array
			string verb = splitLine[0];

			// get the second part of the array
			string noun = splitLine[1];
		}

		sr.Close ();
	// *********** END STREAMS ***********

		// FIND EACH GAME OBJECT IN INTERACTABLES AND ADD ONE QUEST ID FOR EACH ITEM
		// this is where I randomize based on the streamreader whoooooa
//		foreach (GameObject go in finder.interactables) {
//			manager.questList.
//		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
