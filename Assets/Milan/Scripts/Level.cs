using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum = System.Enum;

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

	GameObject[,] children;
	ParticleSystem p;

	GameObject ground, sky;
	Vector3[] vertices;
	Mesh terrain, clouds;
	Vector2[] uvs;
	Vector3 centre;

	int[] groundTriangles;
	Color[] palette;
	Texture2D skyColor;
	Texture2D groundLerpedColour;
	Gradient gradient;
	float hypotenuse;
	float distanceOutsideCircle;

	public void OnCreated(){

		_width = Services.LevelGen.width;
		_height = Services.LevelGen.height;
		_length = Services.LevelGen.length;
		tileScale = Services.LevelGen.tileScale;

		children = new GameObject[_width, _height];
		p = Instantiate (Services.Prefabs.PARTICLESYSTEM, Vector3.zero, Quaternion.identity).GetComponent<ParticleSystem>();
		ParticleSystem tempP = p;
		ParticleSystem.ShapeModule s = tempP.shape;
		s.box = new Vector3 (_width, _height, _length) * tileScale;
		p = tempP;
		p.gameObject.transform.parent = transform;
		p.transform.localPosition = Vector3.zero + (Vector3.up * _height * tileScale);

		ground = Instantiate (Services.Prefabs.TILE, new Vector3(_width/2, 0, _length/2) * tileScale, Quaternion.identity) as GameObject;
		ground.transform.parent = transform;
		ground.transform.localPosition = Vector3.zero;
		ground.name = "GROUND";

//		sky = Instantiate (Services.Prefabs.TILE, new Vector3(_width/2, 0, _length/2) * tileScale, Quaternion.identity) as GameObject;
//		sky.transform.parent = transform;
//		sky.transform.localPosition = Vector3.zero;
//		sky.name = "SKY";
	
		gradient = new Gradient ();

		palette = new Color[15];
		for (int i = 0; i < palette.Length; i++) {
			float upper = (1 / ((float)palette.Length*1.1f)) * (float)i + 0.1f;
			float lower = Mathf.Clamp(upper - 1/(float)palette.Length, 0, upper - 1/(float)palette.Length);
			float rand = Random.Range (lower, upper);
//			palette [i] = new Color (Random.Range (lower, upper),Random.Range (lower, upper),Random.Range (lower, upper));
//			palette [i] = new Color (rand, rand, rand);
			palette [i] = Random.ColorHSV(lower, upper, (1- lower) * 0.5f, (1-upper) * 0.5f, lower, upper);
		}
			
		SetGradient ();

		if (_bitmap == null) {
			usePerlin = true;
			_bitmap = new Texture2D (_width + 1, _length + 1);
		} else {
			_width = _bitmap.width;
			_length = _bitmap.height;
		}

		skyColor = new Texture2D (_width + 1, _length + 1);
		groundLerpedColour = new Texture2D (_width + 1, _length + 1);

		terrain = new Mesh ();
		clouds = new Mesh ();

		vertices = new Vector3[(_width + 1) * (_length + 1)];
		uvs = new Vector2[vertices.Length];
		groundTriangles = new int[_width * _length * 6];

		ground.GetComponent<MeshFilter> ().mesh = terrain;
		ground.GetComponent<Renderer> ().material.mainTexture = groundLerpedColour;
		ground.AddComponent<MeshCollider> ();

//		sky.GetComponent<MeshFilter> ().mesh = clouds;
//		sky.GetComponent<Renderer> ().receiveShadows = false;
//		sky.GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
//		sky.GetComponent<Renderer> ().material.mainTexture = skyColor;

		centre = Vector3.zero;

		hypotenuse = Vector3.Distance(centre, (new Vector3(_width/2, 0, _length/2) * tileScale));
		distanceOutsideCircle = hypotenuse - ((_width/2) * tileScale);

		GenerateChunk();
	}

	void Update(){

		float playerHeightNormalized = ((Services.Player.transform.position.y - transform.position.y) / (_height * tileScale));
		float NormalisedToHightestPoint = ((Services.Player.transform.position.y - transform.position.y) / highestPoint);
		ground.GetComponent<Renderer> ().material.color = Color.Lerp(Color.white, Color.black, playerHeightNormalized/2);
		Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, Color.Lerp(Color.white, Color.black, playerHeightNormalized/2), Time.deltaTime * 3);
		RenderSettings.fogColor = Camera.main.backgroundColor;
		RenderSettings.fogEndDistance = Mathf.Lerp (100, 100, NormalisedToHightestPoint);

//		float xCoord = xOrigin;
//		float yCoord = yOrigin;
//
////		Mesh mesh = sky.GetComponent<MeshFilter>().mesh;
////		Vector3[] verts = mesh.vertices;
//
//		for (int i = 0, y = 0; y <= _length; y++, yCoord++) {
//
//			xCoord = xOrigin;
//
//			for (int x = 0; x <= _width; x++, i++, xCoord++) {
//				float perlinVal = OctavePerlin (xCoord * noiseScale, yCoord * noiseScale, 3, 0.5f);
//				perlinVal = Mathf.Pow (perlinVal, 0.75f);
////				verts [i] = new Vector3 (x, perlinVal * _height, y) * tileScale;
////				verts[i] -= new Vector3(_width/2, 0, _length/2) * tileScale;
//
//				float skyCoefficient = Mathf.Pow(perlinVal, 3);
//				skyColor.SetPixel (x, y, Color.Lerp(Color.Lerp(Color.black, gradient.Evaluate (playerHeightNormalized), NormalisedToHightestPoint), Color.Lerp(gradient.Evaluate (playerHeightNormalized), Color.white, NormalisedToHightestPoint), 1-skyCoefficient));
//				Color Grayscale = new Color (skyCoefficient, skyCoefficient, skyCoefficient);
//				groundLerpedColour.SetPixel (x, y, Color.black + Grayscale);
//	
////				verts [i] -= (Vector3.up * Vector3.Distance (verts [i], centre)) * Mathf.Pow(Vector3.Distance (verts [i], centre)/hypotenuse, 0.5f);
//			}
//		}
//
//		skyColor.Apply();
//		groundLerpedColour.Apply();
////		mesh.vertices = verts;
//
//		xOrigin += Time.deltaTime * 5;
//		yOrigin += Time.deltaTime * 5;

	}

	void SetGradient(){
		GradientColorKey[] gck;
		GradientAlphaKey[] gak;

		gck = new GradientColorKey[8];
		gak = new GradientAlphaKey[2];
		gak[0].alpha = 1.0F;
		gak[0].time = 0.0F;
		gak[1].alpha = 1.0F;
		gak[1].time = 1.0F;

		for (int j = 0; j < gck.Length; j++) {
			gck [j].color = palette [Mathf.RoundToInt(((j)/(float)gck.Length) * (float)palette.Length)];
			gck [j].time = (j)/(float)gck.Length;
		}
		gradient.SetKeys(gck, gak);

		Camera.main.clearFlags = CameraClearFlags.Color;
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
					perlinVal = OctavePerlin (xCoord * noiseScale, yCoord * noiseScale, 1, 0.5f);
				} else {	
					Color c = _bitmap.GetPixel (x, y);
					perlinVal = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
				}
//					
//				int remapIndex = Mathf.RoundToInt(perlinVal * (Services.LevelGen.NoiseRemapping.Length-1));
//				float difference = (perlinVal * (Services.LevelGen.NoiseRemapping.Length-1)) - (float)remapIndex;
//
//
//                perlinVal = Services.LevelGen.NoiseRemapping[remapIndex];
//
//                if (remapIndex < Services.LevelGen.NoiseRemapping.Length - 1 && difference > 0)
//                {
//                    perlinVal = Mathf.Lerp(perlinVal, Services.LevelGen.NoiseRemapping[remapIndex + 1], difference);
//                }
//                else
//                {
//                    if (remapIndex > 0 && difference < 0)
//                    {
//                        perlinVal = Mathf.Lerp(perlinVal, Services.LevelGen.NoiseRemapping[remapIndex - 1], Mathf.Abs(difference));
//                    }
//                }

				perlinVal = Mathf.Pow (perlinVal, 1f);

				vertices [i] -= new Vector3(_width/2, 0, _length/2) * tileScale;
				vertices [i] += new Vector3 (x, 0, y) * tileScale;

				//drag height to 0 around edge of circle
				perlinVal = Mathf.Lerp(perlinVal, 0f, Mathf.Pow(Vector3.Distance(centre, vertices[i])/((_width/2) * tileScale), 6));

				vertices [i] += Vector3.up * _height * perlinVal * tileScale;
			
				_bitmap.SetPixel (x, y, palette[Mathf.RoundToInt(perlinVal * (palette.Length-1))]);

				if (Vector3.Distance (vertices [i], centre) > ((_width * tileScale) / 2)) {
					float spillover = Vector3.Distance (centre, vertices [i]) - ((_width / 2) * tileScale);
					vertices [i] = Vector3.Lerp (vertices [i], centre, spillover / hypotenuse);
					vertices [i] -= (Vector3.up * spillover);
				} else {
					if (vertices [i].y > highestPoint) {
						highestPoint = vertices [i].y;
						highestPointIndex = i;
					}
					LevelObjectFactory (perlinVal, vertices [i], new Vector2 (x, y));
				}

				uvs [i] = new Vector2 ((float)x / _width, (float)y / _length);

			}
		}
			

		OutputBitmap ();

		transform.position -= vertices [highestPointIndex];
	}
		
	public GameObject LevelObjectFactory(float x, Vector3 pos, Vector2 index){

		GameObject newObject = null;
		Color floorColor = _bitmap.GetPixel ((int)index.x, (int)index.y);

		int objectType = Mathf.RoundToInt (x * (Services.LevelGen.props.Length-1));
		int spriteType = Mathf.RoundToInt (x * (Services.Prefabs.SPRITES.Length-1));
		if (index.x == 0 || index.x == _width || index.y == 0 || index.y == _length) {

		//newObject = Instantiate(Services.Prefabs.TILE, Vector3.zero, Quaternion.identity) as GameObject;
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

			return null;
		}

		if (Services.LevelGen.props [objectType] == 0) {
			return null;
		}

		newObject = Instantiate (Services.Prefabs.PREFABS[(int)Services.LevelGen.props[objectType]][Random.Range(0, Services.Prefabs.PREFABS[(int)Services.LevelGen.props[objectType]].Length)], Vector3.zero, Quaternion.identity) as GameObject;

		if (newObject.GetComponentInChildren<SpriteRenderer> () != null) {
			newObject.GetComponent<SpriteRenderer> ().sprite = Services.Prefabs.SPRITES [spriteType] [Random.Range (0, Services.Prefabs.SPRITES [spriteType].Length)];
		} else {
			foreach (Renderer r in newObject.GetComponentsInChildren<Renderer>()) {
				r.material.shader = Services.Prefabs.FlatShading;
				r.material.SetColor ("_Color", floorColor);
			}

			if (newObject.GetComponentInChildren<Renderer> ().bounds.size.x > newObject.GetComponentInChildren<Renderer> ().bounds.size.z && newObject.GetComponentInChildren<Renderer> ().bounds.size.x > 1) {
				newObject.transform.localScale /= newObject.GetComponentInChildren<Renderer> ().bounds.size.x;
			} else if(newObject.GetComponentInChildren<Renderer> ().bounds.size.z > 1){
				newObject.transform.localScale /= newObject.GetComponentInChildren<Renderer> ().bounds.size.z;
			}
			newObject.transform.localScale *= Random.Range (0.5f, tileScale);
		}

		newObject.transform.parent = transform;
		newObject.transform.localPosition = pos;
		newObject.transform.position += newObject.GetComponentInChildren<Renderer> ().bounds.extents.y * Vector3.up;

		Vector3 targetPosition = transform.position;
		targetPosition.y = newObject.transform.position.y;
		newObject.transform.LookAt(targetPosition);
		newObject.transform.Rotate (0, 180, 0);
		//FADING IN GROUND TEXTURE WITH BOTTOM OF THE SPRITE TOO INTENSE FOR THE PROCESSOR!!!!!
		//		Texture2D newTexture = SpriteBlending(Services.Prefabs._sprites [Random.Range (0, Services.Prefabs._sprites.Length)].texture, floorColor);

		//		ADDING SPRITE
//		GameObject Sprite = Instantiate (Services.AudioManager.tonePillowObject, Vector3.zero, Quaternion.identity) as GameObject;
//		Sprite newSprite = Services.Prefabs._sprites [Random.Range (0, Services.Prefabs._sprites.Length)];
//		Sprite.transform.localScale /= Sprite.GetComponentInChildren<Renderer> ().bounds.size.x;
//		Sprite.GetComponentInChildren<Renderer> ().material.SetColor ("_Color", floorColor);
//		Sprite.transform.parent = transform;
//		Sprite.transform.localPosition = pos + (Vector3.up * newObject.GetComponentInChildren<Renderer> ().bounds.extents.y);

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
				groundTriangles[ti] = vi;
				groundTriangles[ti + 3] = groundTriangles[ti + 2] = vi + 1;
				groundTriangles[ti + 4] = groundTriangles[ti + 1] = vi + _width + 1;
				groundTriangles[ti + 5] = vi + _width + 2;
			}
		}

		terrain.triangles = groundTriangles;
		terrain.uv = uvs;

		terrain.RecalculateNormals ();
		ground.GetComponent<MeshFilter> ().mesh = terrain;
		ground.GetComponent<MeshCollider> ().sharedMesh = terrain;

		_bitmap.Apply ();
		_bitmap.filterMode = FilterMode.Point;
		groundLerpedColour.filterMode = FilterMode.Point;

		groundLerpedColour.SetPixels (_bitmap.GetPixels());
		groundLerpedColour.Apply ();

		clouds.vertices = vertices;
		clouds.uv = uvs;
		clouds.triangles = groundTriangles;

//		sky.GetComponent<MeshFilter> ().mesh = clouds;
//		sky.transform.position += Vector3.up * (highestPoint + 100);
//		sky.transform.localScale *= 2;
	}

	public void OnDestroyed(){

	}

//	public void GeneratePieceWise(){
//
//		Vector3 playerPos = Services.Player.transform.position;
//
//		int yIndex = Mathf.RoundToInt ((playerPos.z / tileScale));
//		int xIndex = Mathf.RoundToInt ((playerPos.x / tileScale));
//
//		for (int y = -1, ti = -6, vi = -1; y < 2; y++, vi++) {
//
//			int yPos = yIndex + y;
//
//			for (int x = -1; x < 2; x++, ti += 6, vi++) {
//
//				int xPos = xIndex + x;
//				int vertIndex = xPos * yPos;
//				float perlinVal = 0;
//				int t = ti + vertIndex;
//				int v = vi + vertIndex;
//
//				if(xPos >= 0 && xPos < _width && yPos > 0 && yPos < _length){
//
//					if (usePerlin) {
//						perlinVal = OctavePerlin (((xOrigin + xPos) * noiseScale), ((yOrigin + yPos) * noiseScale), 5, 0.5f);
//					} else {
//						Color c = _bitmap.GetPixel (xPos, yPos);
//						perlinVal = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
//					}
//
//
//					int remapIndex = Mathf.RoundToInt (perlinVal * Services.LevelGen.NoiseRemapping.Length);
//					float difference = (perlinVal * Services.LevelGen.NoiseRemapping.Length) - (float)remapIndex;
//					perlinVal = Services.LevelGen.NoiseRemapping [remapIndex];
//
//					if (remapIndex < Services.LevelGen.NoiseRemapping.Length-1 && difference > 0) {
//						perlinVal = Mathf.Lerp (perlinVal, Services.LevelGen.NoiseRemapping [remapIndex + 1], difference);
//					} else {
//						if (remapIndex > 0 && difference < 0) {
//							perlinVal = Mathf.Lerp (perlinVal, Services.LevelGen.NoiseRemapping [remapIndex - 1], Mathf.Abs(difference));
//						}
//					}
//					_bitmap.SetPixel (x, y, palette[Mathf.RoundToInt(perlinVal * (palette.Length-1))]);
//
//					vertices [vertIndex] = new Vector3 (xPos, (perlinVal * _height), yPos);
//					vertices [vertIndex] *= tileScale;
//
//					uvs [vertIndex] = new Vector2 ((float)xPos / _width, (float)yPos / _length);
//
//					LevelObjectFactory (perlinVal, vertices [vertIndex], new Vector2 (x, y));
//
//					groundTriangles[t] = v;
//					groundTriangles[t + 3] = groundTriangles[t + 2] = v + 1;
//					groundTriangles[t + 4] = groundTriangles[t + 1] = v + _width + 1;
//					groundTriangles[t + 5] = v + _width + 2;
//				}
//			}
//		}
//
//		terrain.vertices = vertices;
//		terrain.uv = uvs;
//
//		ground.GetComponent<MeshFilter> ().mesh = terrain;
//		ground.GetComponent<Renderer> ().material.mainTexture = _bitmap;
//		ground.GetComponent<MeshCollider> ().sharedMesh = terrain;
//	}
}

