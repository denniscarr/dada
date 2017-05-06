using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour {

	public int SpriteIndex;

	public void OnMouseDown(){
		GetComponent<SpriteSound> ().PlaySound ();
	
	}

	public void OnMouseOver(){

		Vector3 targetPosition = Services.Player.transform.position;
		targetPosition.y = transform.position.y;
		transform.LookAt(targetPosition);
		transform.Rotate (0, 180, 0);

		if (Random.Range (0, 100) > (100 - Services.IncoherenceManager.globalIncoherence * 20)) {
			changeSprite ();
		}

	}

	public void changeSprite(){
		GetComponent<SpriteRenderer> ().sprite = Services.Prefabs.SPRITES [SpriteIndex] [Random.Range (0, Services.Prefabs.SPRITES [SpriteIndex].Length)];
	}
}
