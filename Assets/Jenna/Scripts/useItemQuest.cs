using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useItemQuest : Quest {
	public int timeToUse;
	// Use this for initialization
	void Start () {
		base.Start ();
		timeToUse = Random.Range (1, 20);
	}
	
	// Update is called once per frame
	public void FixedUpdate () {
		if (targetObject != null && targetObject.GetComponentInChildren<D_Function> () != null && targetObject.GetComponentInChildren<D_Function> ().timeUsed >= timeToUse) {
			FinishQuest ();
		}
	}

	public override void makeTheQuest(GameObject _targetObject){
		base.makeTheQuest (_targetObject);
		rewardMoney = Mathf.RoundToInt (Random.Range (1, 10000));
		title = ("Use the glowing " + targetObject.name + " several times. ");
		progress = Quest.QuestProgress.AVAILABLE;
		description = (title + "Reward: $" + rewardMoney);
		CopyComponent(this, targetObject);
		spawnNote ();
	}

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
