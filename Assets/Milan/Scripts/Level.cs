using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour, SimpleManager.IManaged {

	public float noiseScale = 0.1f;
	public int minSize, maxSize;
	public float tileSize = 1f;
	public int _width, _height;
	public float xOrigin, yOrigin;
	public string directory; 

	private Texture2D _bitmap;
	private GameObject[,] _tiles;
	public GameObject _tile;


	public void OnCreated(){

		_width = Random.Range (minSize, maxSize);
		_height = Random.Range (minSize, maxSize);
		_bitmap = new Texture2D(_width, _height);
		_tiles = new GameObject[_width, _height];

		float xCoord = xOrigin;
		float yCoord = yOrigin;

		for (int x = 0; x < _width; x++) {

			yCoord = yOrigin;
			xCoord += noiseScale;

			for (int y = 0; y < _height; y++){

				yCoord += noiseScale;

				float perlin = Mathf.PerlinNoise (xCoord, yCoord);

				Color c = new Color (perlin, perlin, perlin);
				_bitmap.SetPixel (x, y, c);

				int objectType = Mathf.FloorToInt(perlin * 10);

				GameObject tile = GetComponent<LevelObjectManager>().ObjectFactory(objectType);

				if (tile != null) {
					tile.transform.localScale += transform.up * perlin * 2;
					tile.transform.position = new Vector3 (x * tileSize, 0, y * tileSize) + transform.position;
					tile.transform.parent = transform;
					tile.name = "Tile " + x + ", " + y;
					_tiles [x, y] = tile;
				}
			}
		}

		OutputBitmap ();
	}

	public void OutputBitmap(){
		GameObject floor = Instantiate (PrefabManager.Instance.TILE, Vector3.zero, Quaternion.identity) as GameObject;
		_bitmap.Apply ();
		_bitmap.filterMode = FilterMode.Point;
		floor.transform.eulerAngles = new Vector3 (90, 0, 0);
		floor.GetComponent<Renderer> ().material.mainTexture = _bitmap;
		floor.transform.localScale += ((Vector3.right) * _width);
		floor.transform.localScale += ((Vector3.up) * _height);
		floor.transform.position += new Vector3 (_width / 2, -transform.localScale.z, _height / 2) + transform.position;
		floor.transform.parent = transform;
		floor.name = "ground";
	}

	public void OnDestroyed(){

	}
}

