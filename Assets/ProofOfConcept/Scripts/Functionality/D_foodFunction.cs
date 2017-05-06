using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class D_foodFunction : D_Function
{
    private Vector3 zero;
    private GameObject eater;
    public float restoreValue = 20f;
	// Use this for initialization
	new void Start () {
        base.Start();
        zero = new Vector3(0, 0, 0);
        //eater = gameObject.transform.parent.parent.gameObject;
	}

    // Update is called once per frame
    public override void Use()
    {
        base.Use();
        /*if (eater.GetComponent<NPC>() != null)
        {
            eater.GetComponent<NPC>().SendMessage("RestoreHealth", restoreValue);
        }*/
        gameObject.transform.parent.transform.DOScale(zero, 1f);
        Destroy(gameObject.transform.parent.gameObject, 1.5f);
	}
}
