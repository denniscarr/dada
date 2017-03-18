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
	private bool equipTrigger = false;
    private GameObject player;
    public float speed = 1;
	public float ASpeed = 10;
	public bool equipped = false;
	public GameObject refrence;

	Transform t_gun;


    // Use this for initialization
    void Start () {
        equipPrompt.SetActive(false);
		player = GameObject.Find("FPSController");
		t_gun = refrence.transform;
		t_gun.gameObject.SetActive(false);
		equipTrigger = false;
		equipped = false;
		Debug.Log (equipTrigger);
    }

    void OnTriggerExit(Collider equipCol)
    {
		
        equipPrompt.SetActive(false);
		Debug.Log ("Trigger exit");
		equipTrigger = false;
    }

    void OnTriggerEnter(Collider equipCol)
    {
		if (equipCol.gameObject.name == "FPSController") {
			equipTrigger = true;
		}
		Debug.Log ("trigger enter "+equipCol.name);
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
		Debug.Log (equipTrigger);
        if (equipTrigger == true && Input.GetKeyDown(equipKey))
        {
            MoveToCamera();
			equipped = true;
        }

		if (Input.GetKeyDown (abandonKey)) {
			abandonItem ();
			equipped = false;
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
		equippable.transform.GetChild (2).gameObject.GetComponent<Collider>().enabled = false;
        Debug.Log("why");
    }

	void abandonItem ()
	{
		GetComponent<Collider>().enabled = true;
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().AddForce(transform.forward * ASpeed);
		transform.SetParent (null);
		equippable.transform.GetChild (2).gameObject.GetComponent<Collider>().enabled = true;
	}
    
}
