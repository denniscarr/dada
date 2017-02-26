using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equip : MonoBehaviour {
    public GameObject equipped;
    public GameObject itemOnTheGround;
    public GameObject equipPrompt;
    public KeyCode equipKey = KeyCode.E;
    public bool activateTrigger = false;
    public bool moveTrigger = false;
    //public KeyCode dismissKey = KeyCode.G;

	// Use this for initialization
	void Start () {
        equipPrompt.SetActive(false); //disable the text prompt that tells you to equip the object
        equipped.SetActive(false); //hide the item when it is not equipped
        itemOnTheGround.SetActive(true); //when the object is waiting to be equipped
        activateTrigger = true; //activates the proximity detection
	}

    void OnTriggerExit()
    {
        activateTrigger = false;
    }

    void OnTriggerEnter() {
        activateTrigger = true;
        if (Input.GetKeyDown(equipKey))
        {
            activateTrigger = true;
            moveTrigger = true;
        }
    }

    // Update is called once per frame
    void Update () {
		if(activateTrigger && Input.GetKeyDown(equipKey))
        {
            equipped.SetActive(true);
            itemOnTheGround.SetActive(false);
            activateTrigger = false;
            moveTrigger = true;
        }

        if (moveTrigger) {
            transform.Translate(new Vector3(0, 0, 1));
        }


	}

    
}
