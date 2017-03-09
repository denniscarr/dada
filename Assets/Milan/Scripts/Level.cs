using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour, SimpleManager.IManaged {

	public float tileScale;
	public bool usePerlin = false;
	public static float noiseScale = 0.05f;
	public static float xOrigin, yOrigin;
	public int _width, _height;
	public string directory; 

	public Texture2D bitmap;
	public GameObject[,] _tiles;

	private bool playerPlaced;
	private Texture2D _bitmap;

	public void OnCreated(){

		if (_bitmap == null) {
			usePerlin = true;
			_bitmap = new Texture2D (_width, _height);
			_tiles = new GameObject[_width, _height];
		} else {
			_tiles = new GameObject[_bitmap.width, _bitmap.height];
		}

		GenerateChunk();
		OutputBitmap ();
	}

	public void GeneratePieceWise(){

		Vector2 playerPos = new Vector2 (Services.Player.transform.position.x, Services.Player.transform.position.z) / tileScale;

		float xCoord = xOrigin + (playerPos.x * noiseScale);
		float yCoord = yOrigin + (playerPos.y * noiseScale);

		for (int y = -2; y < 3; y++) {

			xCoord = xOrigin + (playerPos.x * noiseScale);

			int yPos = (int)playerPos.y + y;

			for (int x = -2; x < 3; x++) {

				int xPos = (int)playerPos.x + x;

				float luminosity = 0;

				if (usePerlin) {
					luminosity = Mathf.PerlinNoise (xCoord + (noiseScale * x), yCoord + (noiseScale * y));
					_bitmap.SetPixel (xPos, yPos, new Color (luminosity, luminosity, luminosity));
				} else {
					Color c = _bitmap.GetPixel (xPos, yPos);
					luminosity = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
				}

				if(GetTile( new Vector2(xPos, yPos)) == null){
					CreateTile (xPos, yPos, luminosity);
				}
			}
		}
	}

	void CreateTile(int x, int y, float perlinVal){

		if (x >= _width || x < 0 || y >= _height || y < 0) {
			return;
		}

		int objectType = Mathf.FloorToInt (perlinVal * 10);

		GameObject tile = Services.Prefabs.ObjectFactory (objectType);

		if (tile != null) {
			tile.transform.position = new Vector3 (x, 0, y) * tileScale;
			tile.transform.localScale *= tileScale;
			tile.transform.position += transform.position;
			tile.transform.position += Vector3.up * tile.GetComponent<MeshRenderer>().bounds.extents.y;
			tile.transform.parent = transform;
			tile.name = "Tile " + x + ", " + y;
			_tiles [x, y] = tile;
		}
	}

	void GenerateChunk(){

		float xCoord = xOrigin;
		float yCoord = yOrigin;

		for (int y = 0; y < _height; y++) {

			xCoord = xOrigin;
			yCoord += noiseScale;

			for (int x = 0; x < _width; x++){

				xCoord += noiseScale;

				float luminosity = 0;

				if (usePerlin) {
					luminosity = Mathf.PerlinNoise (xCoord, yCoord);
					_bitmap.SetPixel (x, y, new Color (luminosity, luminosity, luminosity));
				} else {
					Color c = _bitmap.GetPixel (x, y);
					luminosity = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
				}

				CreateTile (x, y, luminosity);
			}
		}
	}

	public bool IsTileInArray(Vector2 pos){
		if (pos.x >= _width || pos.y >= _height || pos.x < 0 || pos.y < 0) {
			return false;
		} else {
			return true;
		}
	}

	public GameObject GetTile(Vector2 pos){
		if (!IsTileInArray(pos)) {
			return null;
		}else{ 
			return _tiles [(int)pos.x, (int)pos.y];
		}
	}

	public void OutputBitmap(){
		GameObject floor = Instantiate (Services.Prefabs.TILE, Vector3.zero, Quaternion.identity) as GameObject;
		_bitmap.Apply ();
		_bitmap.filterMode = FilterMode.Point;
		floor.transform.eulerAngles = new Vector3 (0, 180, 0);
		floor.GetComponent<Renderer> ().material.mainTexture = _bitmap;
		floor.transform.localScale = Vector3.forward * _height;
		floor.transform.localScale += Vector3.right * _width;
		floor.transform.localScale *= tileScale;
		floor.transform.position = new Vector3 (_width/2 * tileScale, -floor.GetComponent<Renderer>().bounds.extents.y,  _height/2 * tileScale) + transform.position;
		floor.transform.parent = transform;
		floor.name = "ground";
	}

	public void OnDestroyed(){

	}
}

