using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour, SimpleManager.IManaged {

	public static float noiseScale;
	public static float xOrigin, yOrigin;

	float tileScale;
	int _width, _length, _height;
	string directory; 
	public Texture2D _bitmap;
	float colorRange = 0.1f;
	public float highestPoint = 1;

	bool playerPlaced;
	bool usePerlin = false;

	ParticleSystem p;
	GameObject ground;
	Vector3[] vertices;
	Mesh terrain;
	Vector2[] uvs;
	int[] tris;
	Color hue;
	public Color[] palette;


	public void OnCreated(){

		_width = Services.LevelGen.width;
		_height = Services.LevelGen.height;
		_length = Services.LevelGen.length;
		tileScale = Services.LevelGen.tileScale;

		p = Instantiate (Services.Prefabs.PARTICLESYSTEM, Vector3.zero, Quaternion.identity).GetComponent<ParticleSystem>();
		ParticleSystem tempP = p;
		ParticleSystem.ShapeModule s = tempP.shape;
		s.box = new Vector3 (_width, _height, _length) * tileScale;
		p = tempP;
		p.gameObject.transform.parent = transform;
		p.transform.localPosition = new Vector3 (_width / 2, _height, _length / 2) * tileScale;

		ground = Instantiate (Services.Prefabs.TILE, Vector3.zero, Quaternion.identity) as GameObject;
		ground.transform.parent = transform;
		ground.transform.localPosition = Vector3.zero;
		ground.name = "ground";
	
		palette = new Color[Services.LevelGen.NoiseRemapping.Length];
		for (int i = 0; i < palette.Length; i++) {
			float upper = (1 / (float)palette.Length) * (float)i;
			float lower = Mathf.Clamp(upper - 1/(float)palette.Length, 0, upper - 1/(float)palette.Length);
			palette [i] = new Color (Random.Range (lower, upper), Random.Range (lower, upper), Random.Range (lower, upper));
		}
			
		if (_bitmap == null) {
			usePerlin = true;
			_bitmap = new Texture2D (_width + 1, _length + 1);
		} else {
			_width = _bitmap.width;
			_length = _bitmap.height;
		}

		hue = Random.ColorHSV ();
		terrain = new Mesh ();
		vertices = new Vector3[(_width + 1) * (_length + 1)];
		uvs = new Vector2[vertices.Length];
		tris = new int[_width * _length * 6];

		ground.GetComponent<MeshFilter> ().mesh = terrain;
		ground.GetComponent<Renderer> ().material.mainTexture = _bitmap;
		ground.AddComponent<MeshCollider> ();

		GenerateChunk();
	}

	void GenerateChunk(){
		 
		int highestPointIndex = 0;
		highestPoint = -Mathf.Infinity;

		float xCoord = xOrigin;
		float yCoord = yOrigin;

		for (int i = 0, y = 0; y <= _length; y++, yCoord++) {

			xCoord = xOrigin;

			for (int x = 0; x <= _width; x++, i++, xCoord++){

				float perlinVal = 0;

				if (usePerlin) {
					perlinVal = OctavePerlin (xCoord * noiseScale, yCoord * noiseScale, 5, 0.5f);
				} else {
					Color c = _bitmap.GetPixel (x, y);
					perlinVal = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
				}

                int remapIndex = Mathf.RoundToInt((perlinVal * Services.LevelGen.NoiseRemapping.Length-1));
                float difference = ((perlinVal * (Services.LevelGen.NoiseRemapping.Length-1)) - (float)remapIndex);
                perlinVal = Services.LevelGen.NoiseRemapping[remapIndex];

                if (remapIndex < Services.LevelGen.NoiseRemapping.Length - 2 && difference > 0)
                {
                    perlinVal = Mathf.Lerp(perlinVal, Services.LevelGen.NoiseRemapping[remapIndex + 1], difference);
                }
                else
                {
                    if (remapIndex > 0 && difference < 0)
                    {
                        perlinVal = Mathf.Lerp(perlinVal, Services.LevelGen.NoiseRemapping[remapIndex - 1], Mathf.Abs(difference));
                    }
                }
                _bitmap.SetPixel (x, y, palette[Mathf.RoundToInt(perlinVal * (palette.Length-1))]);

				vertices [i] = new Vector3 (x, (perlinVal * _height), y);
				vertices [i] *= tileScale;

				//Generate Tone Prefab between 0.25 _height and 0.5 _height
				if ((perlinVal * _height) > 0f && (perlinVal * _height) < _height * 0.5f) {

					GameObject newObject = Instantiate (Services.AudioManager.tonePillowObject, vertices[i], Quaternion.identity);
					newObject.transform.parent = transform;
					newObject.transform.localPosition = vertices [i];

				}

				uvs [i] = new Vector2 ((float)x / _width, (float)y / _length);

				//		Generate wall if at edge of map
				if (x == 0 || x == _width || y == 0 || y == _length) {
					
				} else {
					if (vertices [i].y > highestPoint) {
						highestPoint = vertices [i].y;
						highestPointIndex = i;
					}
				}

				LevelObjectFactory (perlinVal, vertices [i], new Vector2 (x, y));
			}
		}
			
		_bitmap.filterMode = FilterMode.Point;
		_bitmap.Apply ();

		OutputBitmap ();

		transform.position -= vertices [highestPointIndex] + Vector3.up;
//		Services.Player.transform.position = transform.position + vertices[highestPointIndex];
	}
		
	public GameObject LevelObjectFactory(float x, Vector3 pos, Vector2 index){

		GameObject newObject = null;

		Color floorColor = _bitmap.GetPixel ((int)index.x, (int)index.y);
		int objectType = Mathf.RoundToInt (x * (Services.Prefabs.NPCPREFABS.Length-1));

		//if (index.x == 0 || index.x == _width || index.y == 0 || index.y == _length) {

		//newObject = Instantiate(Services.Prefabs.STATICPREFABS[1], Vector3.zero, Quaternion.identity) as GameObject;
		//newObject.transform.localScale += transform.up * 10;
		//newObject.name = "wall";
		//newObject.transform.parent = transform;
		//newObject.transform.localPosition = pos;

		//if (index.x == 0 || index.x == _width) {
		//	newObject.transform.localScale += (transform.forward * tileScale) - transform.forward;
		//} 
		//if (index.y == 0 || index.y == _length){
		//	newObject.transform.localScale += (transform.right * tileScale) - transform.right;
		//}
		//newObject.GetComponent<Renderer> ().material.color = floorColor;
		//return newObject;
		//}

		if (objectType >= Services.Prefabs.NPCPREFABS.Length || !Services.Prefabs.STATICPREFABS [Mathf.RoundToInt (x * (Services.Prefabs.STATICPREFABS.Length-1))]) {
			return null;
		}

		newObject = Instantiate (Services.Prefabs.NPCPREFABS [Random.Range(0, Services.Prefabs.NPCPREFABS.Length)], Vector3.zero, Quaternion.identity);
//		if (newObject.GetComponentInChildren<InteractionSettings> () == null) {
//			newObject.AddComponent<InteractionSettings> ();
//		}

		newObject.AddComponent<AudioSource> ().playOnAwake = false;
		newObject.AddComponent<SphereCollider> ().isTrigger = true;
		//newObject.tag = "ToneTrigger";
		newObject.GetComponent<AudioSource> ().clip = Services.AudioManager.tonesClipPool [Mathf.RoundToInt(x * (Services.AudioManager.tonesClipPool.Length - 1))];

		if (newObject.GetComponentInChildren<Renderer> ().bounds.size.x > newObject.GetComponentInChildren<Renderer> ().bounds.size.z && newObject.GetComponentInChildren<Renderer> ().bounds.size.x > 1) {
			newObject.transform.localScale /= newObject.GetComponentInChildren<Renderer> ().bounds.size.x;
		} else if(newObject.GetComponentInChildren<Renderer> ().bounds.size.z > 1){
			newObject.transform.localScale /= newObject.GetComponentInChildren<Renderer> ().bounds.size.z;
		}
			
		newObject.GetComponentInChildren<Renderer> ().material.SetColor ("_Color", floorColor);
		//		newObject.GetComponentInChildren<Renderer> ().material.mainTexture = newSprite.texture;

		// Objects scale up with map size?
		//		newObject.transform.localScale *= tileScale;

		newObject.transform.localScale *= Random.Range (0.50f, 1.50f);
		newObject.transform.parent = transform;
		newObject.transform.localPosition = pos;
		newObject.transform.position += Vector3.up * newObject.GetComponentInChildren<Renderer>().bounds.extents.y;

//		ADDING SPRITE
//		GameObject Sprite = Instantiate (Services.Prefabs.SPRITE, Vector3.zero, Quaternion.identity);
//
//		//FADING IN GROUND TEXTURE WITH BOTTOM OF THE SPRITE TOO INTENSE FOR THE PROCESSOR!!!!!
//		//		Texture2D newTexture = SpriteBlending(Services.Prefabs._sprites [Random.Range (0, Services.Prefabs._sprites.Length)].texture, floorColor);
//
//		Sprite newSprite = Services.Prefabs._sprites [Random.Range (0, Services.Prefabs._sprites.Length)];
//		Sprite.GetComponent<SpriteRenderer> ().sprite = newSprite;
//		Sprite.transform.localScale /= Sprite.GetComponentInChildren<Renderer> ().bounds.size.x;
//		Sprite.GetComponentInChildren<Renderer> ().material.SetColor ("_Color", floorColor);
//		Sprite.transform.position = newObject.transform.position + (Vector3.up * newObject.GetComponentInChildren<Renderer> ().bounds.extents.y);
//		Sprite.transform.parent = newObject.transform;

		return newObject;
	}

	Texture2D SpriteBlending(Texture2D sprite, Color floorC){
		Texture2D t =  new Texture2D (sprite.width, sprite.height);
	 	t.SetPixels32(sprite.GetPixels32());

		int maxHeight = t.height/2;
		for (int y = 0; y < maxHeight; y++) {
			for (int x = 0; x < t.width; x++) {
				Color c = t.GetPixel (x, y);
				if (c.a > 0.1f) {
					float progress = (float)y / (float)maxHeight;
					float lerpVal = Mathf.Cos (progress * Mathf.PI);
					c = Color.Lerp (c, floorC, lerpVal);
					c = floorC;
					t.SetPixel (x, y, c);
				}
			}
		}
		t.Apply ();
		return t;
	}

	public bool IsTileInArray(Vector2 pos){
		if (pos.x >= _width || pos.y >= _length || pos.x < 0 || pos.y < 0) {
			return false;
		} else {
			return true;
		}
	}

	float OctavePerlin(float x, float y, int octaves, float persistence) {
		float total = 0;
		float frequency = 1;
		float amplitude = 1;
		float maxValue = 0;  // Used for normalizing result to 0.0 - 1.0

		for(int i=0;i<octaves;i++) {
			total += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;

			maxValue += amplitude;

			amplitude *= persistence;
			frequency *= 2;
		}

		return total/maxValue;
	}

	public void OutputBitmap(){

		terrain.vertices = vertices;

		for (int ti = 0, vi = 0, y = 0; y < _length; y++, vi++) {
			for (int x = 0; x < _width; x++, ti += 6, vi++) {
				tris[ti] = vi;
				tris[ti + 3] = tris[ti + 2] = vi + 1;
				tris[ti + 4] = tris[ti + 1] = vi + _width + 1;
				tris[ti + 5] = vi + _width + 2;
			}
		}

		terrain.triangles = tris;
		terrain.uv = uvs;

		ground.GetComponent<MeshFilter> ().mesh = terrain;
		ground.GetComponent<Renderer> ().material.mainTexture = _bitmap;
		ground.GetComponent<MeshCollider> ().sharedMesh = terrain;
	}

	public void OnDestroyed(){

	}

	public void GeneratePieceWise(){

		Vector3 playerPos = Services.Player.transform.position;

		int yIndex = Mathf.RoundToInt ((playerPos.z / tileScale));
		int xIndex = Mathf.RoundToInt ((playerPos.x / tileScale));

		for (int y = -1, ti = -6, vi = -1; y < 2; y++, vi++) {

			int yPos = yIndex + y;

			for (int x = -1; x < 2; x++, ti += 6, vi++) {

				int xPos = xIndex + x;
				int vertIndex = xPos * yPos;
				float perlinVal = 0;
				int t = ti + vertIndex;
				int v = vi + vertIndex;

				if(xPos >= 0 && xPos < _width && yPos > 0 && yPos < _length){

					if (usePerlin) {
						perlinVal = OctavePerlin (((xOrigin + xPos) * noiseScale), ((yOrigin + yPos) * noiseScale), 5, 0.5f);
					} else {
						Color c = _bitmap.GetPixel (xPos, yPos);
						perlinVal = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
					}

					int tileIndex = Mathf.FloorToInt (perlinVal * 10);

					int remapIndex = Mathf.RoundToInt (perlinVal * Services.LevelGen.NoiseRemapping.Length);
					float difference = (perlinVal * Services.LevelGen.NoiseRemapping.Length) - (float)remapIndex;
					perlinVal = Services.LevelGen.NoiseRemapping [remapIndex];

					if (remapIndex < Services.LevelGen.NoiseRemapping.Length-1 && difference > 0) {
						perlinVal = Mathf.Lerp (perlinVal, Services.LevelGen.NoiseRemapping [remapIndex + 1], difference);
					} else {
						if (remapIndex > 0 && difference < 0) {
							perlinVal = Mathf.Lerp (perlinVal, Services.LevelGen.NoiseRemapping [remapIndex - 1], Mathf.Abs(difference));
						}
					}
					_bitmap.SetPixel (x, y, palette[Mathf.RoundToInt(perlinVal * (palette.Length-1))]);

					vertices [vertIndex] = new Vector3 (xPos, (perlinVal * _height), yPos);
					vertices [vertIndex] *= tileScale;

					uvs [vertIndex] = new Vector2 ((float)xPos / _width, (float)yPos / _length);

					LevelObjectFactory (perlinVal, vertices [vertIndex], new Vector2 (x, y));

					tris[t] = v;
					tris[t + 3] = tris[t + 2] = v + 1;
					tris[t + 4] = tris[t + 1] = v + _width + 1;
					tris[t + 5] = v + _width + 2;
				}
			}
		}

		terrain.vertices = vertices;
		terrain.uv = uvs;

		ground.GetComponent<MeshFilter> ().mesh = terrain;
		ground.GetComponent<Renderer> ().material.mainTexture = _bitmap;
		ground.GetComponent<MeshCollider> ().sharedMesh = terrain;
	}
}

