using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// LOCATION: QUEST OBJECT

public class QuestObject : MonoBehaviour {

	private bool inTrigger = false;

	public List<int> availableQuestIDs = new List<int>();
	public List<int> receivableQuestIDs = new List<int>();

	// in this example, this is the stupid square canvas, but this is a layout
	public GameObject QuestMarker;
	public Image theImage;

	public Sprite questAvailableSprite;
	public Sprite questReceivableSprite;

	// has it been assigned?
	public bool assigned = false;

	// Use this for initialization
	void Start () {

		//SetQuestMarker (); //do the thing automatically if necessary
		
	}

	// SETS THE GRAPHIC TO INDICATE QUESTS
	// not sure if necessary but hey you know could be useful for something
	// IT IS STILL BUGGY AND NOT CHANGING, but that can be easily fixed in a real future version
	// if we even deem this to be necessary, which I'm not convinced it is.
	public void SetQuestMarker() {

		if (QuestManager.questManager.CheckCompletedQuests (this)) { //check to see if there are any completed quests on this objct + set sprite to available
			QuestMarker.SetActive (true);
			theImage.sprite = questReceivableSprite;
			theImage.color = Color.yellow;
		} else if (QuestManager.questManager.CheckAvailableQuests (this)) { //set quest marker to active/available
			QuestMarker.SetActive (true);
			theImage.sprite = questAvailableSprite;
			theImage.color = Color.magenta;
			assigned = true;
		} else if (QuestManager.questManager.CheckAcceptedQuests (this)) {
			QuestMarker.SetActive (true);
			theImage.sprite = questReceivableSprite;
			theImage.color = Color.gray;
		} else { //disable
			QuestMarker.SetActive (false);

		}
	}
	
	// Update is called once per frame
	public void Update () {

        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name.Contains("Blue"))
                {
                    inTrigger = true;
                }
                else
                {
                    inTrigger = false;
                }
            }

            if (inTrigger == true)
            {
                if (Input.GetMouseButton(0))
                {
                    //quest user interface manager to check shit
                    Debug.Log("it's seeing you have a quest");
                    QuestManager.questManager.QuestRequest(this);
                }
            }
        }

	}

	// these can all be changed, i guess, like the tag and stuff, once we have a uniform
	// nomenclature scheme. again, this is just a frame I'm learning from some guy on the
	// internet so we can change everything when it comes to that point.
	// the ontrigger functions are exclusively for flagging to see if something is interactable.
	public void OnTriggerEnter(Collider col){
		if (col.tag == "Player") {
			inTrigger = true;
		}
	}

	public void OnTriggerExit(Collider col){
		if (col.tag == "Player") {
			inTrigger = false;
		}
	}
}
