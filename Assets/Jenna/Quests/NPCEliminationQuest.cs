using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEliminationQuest : Quest {

	// Use this for initialization
	void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (targetObject != null && targetObject.GetComponentInChildren<NPC> () != null && targetObject.GetComponentInChildren<NPC> ().died == true) {
			FinishQuest();
		}
	}

	public override void makeTheQuest(){
		base.makeTheQuest ();
		rewardMoney = Mathf.RoundToInt (Random.Range(1, 10000));
		title = ("Eliminate the glowing " + targetObject.name + " ");
		progress = Quest.QuestProgress.AVAILABLE;
		description = (title + " by any means possible. Reward: $" + rewardMoney);
		CopyComponent(this, targetObject);
		spawnNote ();
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
}
