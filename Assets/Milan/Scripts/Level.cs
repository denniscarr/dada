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
	public Vector3[,] _gridPositions;

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
			
		_gridPositions = new Vector3[_width, _height];

		terrain = new Mesh ();
		vertices = new Vector3[(_width + 1) * (_height + 1)];
		uvs = new Vector2[vertices.Length];
		tris = new int[_width * _height * 6];

		GenerateChunk();
		OutputBitmap ();
	}

//	public void GeneratePieceWise(){
//
//		Vector2 playerPos = new Vector2 (Services.Player.transform.position.x, Services.Player.transform.position.z) / tileScale;
//
//		float xCoord = xOrigin + (playerPos.x * noiseScale);
//		float yCoord = yOrigin + (playerPos.y * noiseScale);
//
//		for (int y = -2; y < 3; y++) {
//
//			xCoord = xOrigin + (playerPos.x * noiseScale);
//
//			int yPos = (int)playerPos.y + y;
//
//			for (int x = -2; x < 3; x++) {
//
//				int xPos = (int)playerPos.x + x;
//
//				float luminosity = 0;
//
//				if (usePerlin) {
//					luminosity = Mathf.PerlinNoise (xCoord + (noiseScale * x), yCoord + (noiseScale * y));
//					_bitmap.SetPixel (xPos, yPos, new Color (luminosity, luminosity, luminosity));
//				} else {
//					Color c = _bitmap.GetPixel (xPos, yPos);
//					luminosity = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
//				}
//
//				if(_gridPositions[xPos, yPos] == null){
//					CreateTile (xPos, yPos, luminosity);
//				}
//			}
//		}
//	}

	void CreateTile(Vector3 pos, float luminosity){

//		if (x >= _width || x < 0 || y >= _height || y < 0) {
//			return;
//		}

		int objectType = Mathf.FloorToInt (luminosity * 10);

		GameObject tile = ObjectFactory (objectType);

		if (tile != null) {
//			_gridPositions [x, y] = (new Vector3 (x, perlinVal * 2, y) * tileScale) + transform.position;
			tile.transform.localScale *= tileScale;
			tile.transform.position = pos;
			tile.transform.position += Vector3.up * tile.GetComponent<MeshRenderer> ().bounds.extents.y;
			tile.transform.parent = transform;
			tile.name = "Tile " + pos.x /tileScale + ", " + pos.y/tileScale;
		} else {
//			_gridPositions [x, y] = new Vector3 (x, 1, y) * tileScale + transform.position;
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

				vertices [i] = (new Vector3 (x, luminosity * 3, y) * tileScale) + transform.position;
				uvs [i] = new Vector2 ((float)x / _width, (float)y / _height);

				CreateTile (vertices[i], luminosity);


			}
		}
	}

	public GameObject ObjectFactory(int x){

		GameObject newObject = null;

		switch (x) {

		case 8:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[0], Vector3.up * Random.Range(5, 20), Quaternion.identity) as GameObject;
			break;
		case 7:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[2], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case 4:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[1], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case 2:
			newObject = Instantiate (Services.Prefabs.LEVELPREFABS[3], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		default:
			return null;
			break;
		}
		newObject.transform.localScale *= Random.Range (0.25f, 2f);
		newObject.GetComponent<Renderer> ().material.mainTexture = Services.Prefabs._sprites [Random.Range (0, Services.Prefabs._sprites.Length)].texture;

		return newObject;
	}


	public bool IsTileInArray(Vector2 pos){
		if (pos.x >= _width || pos.y >= _height || pos.x < 0 || pos.y < 0) {
			return false;
		} else {
			return true;
		}
	}

//	public GameObject GetTile(Vector2 pos){
//		if (!IsTileInArray(pos)) {
//			return null;
//		}else{ 
//			return _tiles [(int)pos.x, (int)pos.y];
//		}
//	}

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
		_bitmap.filterMode = FilterMode.Point;
		floor.GetComponent<Renderer> ().material.mainTexture = _bitmap;

//		floor.transform.localScale = Vector3.forward * _height;
//		floor.transform.localScale += Vector3.right * _width;
//		floor.transform.localScale *= tileScale;

		floor.transform.position += transform.position;
		floor.transform.parent = transform;
		floor.name = "ground";
	}

	public void OnDestroyed(){

	}
}

