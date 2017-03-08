using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipA2 : MonoBehaviour {
    public GameObject equippable;
    public GameObject equipPrompt; //this is the UI text displayed when player enters the trigger
    public Collider equipCol; //drag the child box trigger collider of an equippable item here
    public Vector3 cameraPos;
    public KeyCode equipKey = KeyCode.E;
    public KeyCode abandonKey = KeyCode.G;
    private bool activatePrompt = false;

    // Use this for initialization
    void Start () {
        equipPrompt.SetActive(false);

    }

    void OnTriggerExit(Collider equipCol)
    {
        equipPrompt.SetActive(false);
    }

    void OnTriggerEnter(Collider equipCol)
    {
        equipPrompt.SetActive(true);
        if (Input.GetKeyDown(equipKey))
        {
            activatePrompt = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (activatePrompt && Input.GetKeyDown(equipKey))
        {
            equipPrompt.SetActive(true);
        }
    }

    void MoveToCamera ()
    {


    }

    
}
