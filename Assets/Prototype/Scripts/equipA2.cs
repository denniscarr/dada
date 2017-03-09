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
    private GameObject player;
    public float speed = 1;

	Transform t_gun;


    // Use this for initialization
    void Start () {
        equipPrompt.SetActive(false);
		player = GameObject.Find("FPSController");
		t_gun = GameObject.Find("Modern Russian AR (1)").transform;
		t_gun.gameObject.SetActive(false);
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

        if (Input.GetKeyDown(equipKey))
        {
            MoveToCamera();
        }
    }

    void MoveToCamera ()
    {
        
        float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = false;
		transform.position = t_gun.position;
		transform.rotation = t_gun.rotation;
		transform.SetParent(t_gun.parent,true);

        Debug.Log("why");
    }

    
}
