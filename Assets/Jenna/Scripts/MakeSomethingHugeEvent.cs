using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MakeSomethingHugeEvent : IncoherenceEvent {

	new void Start()
    {
        instantaneous = true;
        threshold = 0.3f;
    }


    public override void Initiate()
    {
        base.Initiate();

        InteractionSettings chosenOne = FindObjectsOfType<InteractionSettings>()[Random.Range(0, FindObjectsOfType<InteractionSettings>().Length)];
        chosenOne.transform.parent.DOScale(chosenOne.transform.parent.localScale * 100f, Random.Range(1f, 10f));
    }
}
