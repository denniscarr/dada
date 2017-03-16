using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour, SimpleManager.IManaged {

	public float tileScale;
	public bool usePerlin = false;
	public static float noiseScale = 0.1f;
	public static float xOrigin, yOrigin;
	public int _width, _height;
	public string directory; 

	public Texture2D bitmap;

	private bool playerPlaced;

	private Texture2D _bitmap;

	public Vector3[] vertices;
	private Mesh terrain;
	private Vector2[] uvs;
	private int[] tris;

	public void OnCreated(){

		if (_bitmap == null) {
			usePerlin = true;
			_bitmap = new Texture2D (_width, _height);
		} else {
			_width = _bitmap.width;
			_height = _bitmap.height;
		}

		terrain = new Mesh ();
		vertices = new Vector3[(_width + 1) * (_height + 1)];
		uvs = new Vector2[vertices.Length];
		tris = new int[_width * _height * 6];

		GenerateChunk();
		OutputBitmap ();
	}

	void CreateTile(Vector3 pos, float luminosity){

		GameObject tile = ObjectFactory (luminosity);

		if (tile != null) {
			tile.transform.localScale *= tileScale;
			tile.transform.position = pos;
			tile.transform.position += Vector3.up * (tile.transform.localScale.y);
			tile.transform.parent = transform;
			tile.name = "Tile " + pos.x /tileScale + ", " + pos.y/tileScale;
		} else {
		}
	}
		
	void GenerateChunk(){
		 

		float xCoord = xOrigin;
		float yCoord = yOrigin;

		for (int i = 0, y = 0; y <= _height; y++) {

			xCoord = xOrigin;
			yCoord += noiseScale;

			for (int x = 0; x <= _width; x++, i++){

				xCoord += noiseScale;

				float luminosity = 0;

				if (usePerlin) {
					luminosity = Mathf.PerlinNoise (xCoord, yCoord);
					_bitmap.SetPixel (x, y, new Color (luminosity, luminosity, luminosity));

				} else {
					Color c = _bitmap.GetPixel (x, y);
					luminosity = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
				}

				if (luminosity > 0f && luminosity < 0.4) {
					vertices [i] = (new Vector3 (x, 3, y) * tileScale) + transform.position;
				} else if (luminosity > 0.5f) {
					vertices [i] = (new Vector3 (x, 7, y) * tileScale) + transform.position;
				} else {
					vertices [i] = (new Vector3 (x, luminosity * 10, y) * tileScale) + transform.position;
				}
				uvs [i] = new Vector2 ((float)x / _width, (float)y / _height);

				if(x == 0 || x == _width || y == 0 || y == _height){
					GameObject wall = Instantiate(Services.Prefabs.LEVELPREFABS[1], vertices [i] + (transform.up * 5), Quaternion.identity) as GameObject;
					wall.transform.localScale += transform.up * 10;
					wall.name = "wall";
					wall.transform.parent = gameObject.transform;

					if (x == 0 || x == _width) {
						wall.transform.localScale += (transform.forward * tileScale) - transform.forward;
					} 
					if (y == 0 || y == _height){
						wall.transform.localScale += (transform.right * tileScale) - transform.right;
					}
					wall.GetComponent<Renderer>().material.SetColor("_Color", _bitmap.GetPixel(x, y));
				}else{
					CreateTile (vertices [i], luminosity);
				}
			}
		}
	}

	public GameObject ObjectFactory(float x){

		//MusicProcGen -- load tones from folder
		//this functionality will probably be migrated to the tone object prefab
		AudioClip[] Tones = Resources.LoadAll<AudioClip> ("Tones");

		GameObject newObject = null;

		int objectType = Mathf.FloorToInt (x * 10);

		switch (objectType) {

		case 8:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[3], Vector3.up * Random.Range(5, 20), Quaternion.identity) as GameObject;
			break;
		case 7:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[2], Vector3.zero, Quaternion.identity) as GameObject;
			break;

		////////////////////
		//  Music Object  //
		////////////////////
		case 6: 
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS [4], Vector3.zero, Quaternion.identity) as GameObject;
			newObject.GetComponent<AudioSource> ().clip = Tones [Random.Range (0, Tones.Length - 1)];
			break;

		case 4:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[1], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		default:
			return null;
			break;
		}

		Texture2D t = Services.Prefabs._sprites [Random.Range (0, Services.Prefabs._sprites.Length)].texture;
		Texture2D newTexture =  new Texture2D (t.width, t.height);
		Color floorColour = new Color (x, x, x);
		newTexture.SetPixels32(t.GetPixels32());
		newTexture = SpriteBlending (newTexture, floorColour);
		newTexture.Apply ();

		if (newObject.GetComponent<SpriteRenderer> () != null) {
			Sprite newSprite = Sprite.Create (newTexture, new Rect (0, 0, newTexture.width, newTexture.height), new Vector2 (0.5f, 0.5f), 100);
			newObject.GetComponent<SpriteRenderer> ().sprite = newSprite;
			newObject.transform.localScale = new Vector3 (1 / newSprite.bounds.size.x, 1 / newSprite.bounds.size.y, 0);
		} else {
			newObject.GetComponent<Renderer> ().material.SetColor ("_Color", floorColour);
		}

		newObject.transform.localScale *= Random.Range (0.25f, 2f);
		return newObject;
	}

	Texture2D SpriteBlending(Texture2D t, Color floorC){
		int maxHeight = t.height/2;
		for (int y = 0; y < maxHeight; y++) {
			for (int x = 0; x < t.width; x++) {
				Color c = t.GetPixel (x, y);
				if (c.a > 0.1f) {
					float progress = (float)y / (float)maxHeight;
					float lerpVal = Mathf.Cos (progress * Mathf.PI);
//					c = Color.Lerp (c, floorC, lerpVal);
					c = floorC;
					t.SetPixel (x, y, c);
				}
			}
		}
		return t;
	}

	public bool IsTileInArray(Vector2 pos){
		if (pos.x >= _width || pos.y >= _height || pos.x < 0 || pos.y < 0) {
			return false;
		} else {
			return true;
		}
	}

	public void OutputBitmap(){

		terrain.vertices = vertices;

		for (int ti = 0, vi = 0, y = 0; y < _height; y++, vi++) {
			for (int x = 0; x < _width; x++, ti += 6, vi++) {
				tris[ti] = vi;
				tris[ti + 3] = tris[ti + 2] = vi + 1;
				tris[ti + 4] = tris[ti + 1] = vi + _width + 1;
				tris[ti + 5] = vi + _width + 2;
			}
		}

		terrain.triangles = tris;
		terrain.uv = uvs;


		GameObject floor = Instantiate (Services.Prefabs.TILE, Vector3.zero, Quaternion.identity) as GameObject;
		floor.GetComponent<MeshFilter>().mesh = terrain;

		_bitmap.Apply ();
		floor.GetComponent<Renderer> ().material.mainTexture = _bitmap;
		floor.transform.parent = transform;
		floor.name = "ground";
	}

	public void OnDestroyed(){

	}
}

