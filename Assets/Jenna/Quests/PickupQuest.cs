using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// LOCATION: QUEST MANAGER
// LOCATION: QUEST GAMEOBJECT (description serves as "giving of quest")

public class PickupQuest : Quest {

	// for finishing quest
	public int requiredPickups;
	public int numberofPickups;
	public bool pickedUp;


	void Start () {

		base.Start ();

	}

    public void FixedUpdate()
    {
        //	 check to see if the thing has been picked up
        //	 if so YAY FINISH

        if (targetObject != null && targetObject.GetComponentInChildren<InteractionSettings>() != null)
        {
            //if (parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject != null &&
            //    parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject.name == "Player" &&
            //    !pickedUp)
            //         {
            //	numberofPickups++;
            //	text.text = ("Picked up " + numberofPickups.ToString () + " " + "times");

            //	if (numberofPickups >= requiredPickups) {
            //		FinishQuest ();
            //	}

            //	pickedUp = true;

            //} else if (parentObject.GetComponentInChildren<InteractionSettings> ().carryingObject == null) {
            //	pickedUp = false;
            //}

			if (Input.GetMouseButtonUp(0) && targetObject.transform.parent && targetObject.transform.parent.name.Equals("INROOMOBJECTS"))
            {
                FinishQuest();
            }

            //if (targetObject.GetComponentInChildren<InteractionSettings>().IsInVisor)
            //{
            //    if (targetObject.GetComponent<CollisionReporter>() == null) targetObject.AddComponent<CollisionReporter>();

            //    if (targetObject.GetComponent<CollisionReporter>() != null && targetObject.GetComponentInChildren<InteractionSettings>().carryingObject != Services.Player && targetObject.GetComponent<CollisionReporter>().collidedWithSomethingAtLeastOnce)
            //    {
            //        Destroy(targetObject.GetComponent<CollisionReporter>());
            //        FinishQuest();
            //    }
            //}
        }
    }


	public override void makeTheQuest(GameObject _targetObject){
		base.makeTheQuest (_targetObject);
		requiredPickups = Random.Range (2, 6);
        rewardMoney = Mathf.RoundToInt(targetObject.GetComponentInChildren<InteractionSettings>().price * Random.Range(2f, 3f));
        //Debug.Log("Required pickups: " + requiredPickups + ", Reward money: " + rewardMoney);

		// create title to appear. THIS IS THE QUEST OBJECTIVE.
		title = ("Pick up the glowing" + " " + targetObject.name + " "); 

		//manager.CheckAvailableQuests (objectScript);
		progress = Quest.QuestProgress.AVAILABLE;

		// give it a description eh
		// can make this more interesting later during tweaking/juicing stages
		description = (title + " " + "and place it in your room. Reward: $" + rewardMoney);


        // put it on the parent object
        CopyComponent(this, targetObject);

        spawnNote();

        //for (int i = 0; i < 20; i++) {
        //	NoteSpawnerScript noteSpawn = GameObject.Find("NoteSpawner(Clone)").GetComponent<NoteSpawnerScript>();
        //	noteSpawn.MakeItRain (id);
        //}
    }

    // method to copy alla this shit on the pickupquest on the quest object generated
    // in questbuilderscript
    Component CopyComponent (Component original, GameObject destination)
    {
		System.Type type = original.GetType ();
		Component copy = destination.AddComponent(type);

		System.Reflection.FieldInfo[] fields = type.GetFields ();
		foreach (System.Reflection.FieldInfo field in fields) {
			field.SetValue (copy, field.GetValue(original));
		}
		return copy;
	}


		

	new void FinishQuest(){
		if (completed) return;
		base.FinishQuest ();

    }


    //public void DestroyNotes()
    //{
    //    Debug.Log("notes belonging to me: " + myNotes.Count);
    //    // Destroy all notes related to this quest.
    //    for (int i = 0; i < myNotes.Count; i++)
    //    {
    //        Destroy(myNotes[i]);
    //    }
    //}
}
