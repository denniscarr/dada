using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class profitParticle : MonoBehaviour {
	private ParticleSystem money;
	// Use this for initialization
	void Start () {
		money = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnParticleCollision (GameObject goods) {
		if (goods.GetComponentInChildren<InteractionSettings>() != null) {
			print (goods.name + " needs a price rise");
			goods.GetComponentInChildren<InteractionSettings> ().price += 20;
		}
	}
}
