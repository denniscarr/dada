﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpawner: MonoBehaviour {

	public float delay;
	public float fadeSpeed;

	public bool createGrid;
	public float xDist, yDist, zDist;
	public string directory;
	public int height, width;
	public GameObject spritePrefab;
	public Vector3 rotation;
	public static GameObject[,] grid;

	protected List<Sprite> sprites;
	protected List<GameObject> children;
	protected Sprite[] spriteArray;

	private int xIndex, yIndex, index;
	// Use this for initialization

	public SpriteSpawner(){
		sprites = new List<Sprite> ();
		children =  new List<GameObject> ();
	}

	protected void LoadSprites(){
		spriteArray =  (Resources.LoadAll<Sprite> (directory));
		sprites.AddRange (spriteArray);
	}

	void Start () {
		LoadSprites ();
		if (createGrid) {
			StartCoroutine(SpawnGrid ());
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public virtual void Spawn (){
		
		Vector3 newObjectPos = transform.position;
		newObjectPos.x += (xIndex * xDist) - (width/2 * xDist);
		newObjectPos.z += (yIndex * zDist);
		newObjectPos.y += (yIndex * yDist);

		GameObject newObject = (GameObject)Instantiate (spritePrefab, newObjectPos, Quaternion.identity);
		newObject.transform.rotation = Quaternion.Euler (rotation);
		newObject.transform.parent = transform;
		if (newObject.GetComponent<SpriteRenderer> () == null) {
			newObject.AddComponent<SpriteRenderer>();
		}
		newObject.AddComponent <FadeSprite>().time = fadeSpeed;

		SpriteRenderer r = newObject.GetComponent<SpriteRenderer> ();
		if (sprites.Count == 0) {
			sprites.AddRange (spriteArray);
		}
		Sprite s = sprites [Random.Range (0, sprites.Count)];
		r.sprite = s;
		r.sortingOrder = index;
		newObject.transform.position += (transform.up * r.bounds.size.y)/2;
//		newObject.transform.localScale /= r.bounds.size.y/xDist;
		sprites.Remove (s);
		children.Add (newObject);
	}

  public  IEnumerator SpawnGrid(){

		for (xIndex = 0; xIndex < width; xIndex++) {
			for (yIndex = 0; yIndex < height; yIndex++) {
				Spawn ();
				index++;
			}
			yield return new WaitForSeconds(delay);
		}
		StartCoroutine (SpawnGrid ());
	}
}
