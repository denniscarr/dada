using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prompt : MonoBehaviour {
    public GameObject equipPrompt;
    public KeyCode equipKey = KeyCode.E;
    public bool activatePrompt = false;
    // Use this for initialization
    void Start () {
        equipPrompt.SetActive(false); //disable the equip prompt when it should not appear
	}

	void OnTriggerExit()
    {
        equipPrompt.SetActive(false);
    }

    void OnTriggerEnter()
    {
        equipPrompt.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            activatePrompt = true;
        }
    }
	// Update is called once per frame
	void Update () {
		if(activatePrompt && Input.GetKeyDown(equipKey))
        {
            equipPrompt.SetActive(true);
        }
	}
}
