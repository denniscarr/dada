using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
			GameObject.FindObjectOfType<MouseControllerNew>().GetComponent<Image>().DOFade(0.5f,0f);
			Debug.Log("mouse over sprite");
		}

	}

	public void changeSprite(){
		GetComponent<SpriteRenderer> ().sprite = Services.Prefabs.SPRITES [SpriteIndex] [Random.Range (0, Services.Prefabs.SPRITES [SpriteIndex].Length)];
		Services.LevelGen.cookieLight.cookie = GetComponent<SpriteRenderer> ().sprite.texture;
		Services.LevelGen.cookieLight.intensity = 0.2f;
	}
}
