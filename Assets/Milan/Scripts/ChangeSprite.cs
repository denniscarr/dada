using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour {

	public int SpriteIndex;

	public void OnMouseDown(){
		GetComponent<SpriteRenderer>().sprite = Services.Prefabs.SPRITES[SpriteIndex][Random.Range(0, Services.Prefabs.SPRITES[SpriteIndex].Length)];
	}
}
