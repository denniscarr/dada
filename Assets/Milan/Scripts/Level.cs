using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum = System.Enum;

public class Level : MonoBehaviour, SimpleManager.IManaged {

	public static float noiseScale;
	public static float xOrigin, yOrigin;

	Terrain levelMesh;

	float tileScale;
	public int _width, _length, _height;
	string directory; 
	public Texture2D _bitmap;
	float colorRange = 0.1f;
	public float highestPoint = 1;
	public float mapHeight;

	bool playerPlaced;
	bool usePerlin = false;

	List<int> highestPointIndices;
	int highestPointIndex;
	public int NPCs, Pickups, NonPickups, Sprites;
	int spriteType;

	GameObject[,] children;

	GameObject ground, sky;
	Vector3[] vertices;
	Vector3[] normals;
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
	float normalizedPerlinHeight;

	public void OnCreated(){

		_width = Random.Range (10, 25);
		_length = _width;
		_height = Random.Range (1, 4);

		tileScale = Services.LevelGen.tileScale;

		children = new GameObject[_width, _height];

		ground = Instantiate (Services.Prefabs.TILE, new Vector3(_width/2, 0, _length/2) * tileScale, Quaternion.identity) as GameObject;
		ground.transform.parent = transform;
		ground.layer = 2;
		ground.transform.localPosition = Vector3.zero;
		ground.name = "GROUND";
		ground.isStatic = true;

		sky = Instantiate (Services.Prefabs.TILE, new Vector3(_width/2, 0, _length/2) * tileScale, Quaternion.identity) as GameObject;
		sky.transform.parent = transform;
		sky.transform.localPosition = Vector3.zero;
		sky.name = "SKY";
	
		gradient = new Gradient ();

		palette = new Color[10];
		for (int i = 0; i < palette.Length; i++) {
			float upper = (1 / ((float)palette.Length*1.1f)) * (float)i + 0.1f;
			float lower = Mathf.Clamp(upper - 1/(float)palette.Length, 0, upper - 1/(float)palette.Length);
			float rand = Random.Range (lower, upper);
			palette [i] = new Color (Random.Range (lower, upper),Random.Range (lower, upper),Random.Range (lower, upper));
//			palette [i] = new Color (rand, rand, rand);
//			palette [i] = Random.ColorHSV(lower, upper, (1- lower) * 0.5f, (1-upper) * 0.5f, lower, upper);
		}
			
		SetGradient ();

		usePerlin = true;
		_bitmap = new Texture2D (33, 33);

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

		sky.GetComponent<MeshFilter> ().mesh = clouds;
		sky.GetComponent<Renderer> ().receiveShadows = false;
		sky.GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		sky.GetComponent<Renderer> ().material.mainTexture = skyColor;
		if (!Services.LevelGen.showSky) {
			sky.GetComponent<Renderer> ().enabled = false;
		}

		centre = Vector3.zero;

		hypotenuse = Vector3.Distance(centre, (new Vector3(_width/2, 0, _length/2) * tileScale));
		distanceOutsideCircle = hypotenuse - ((_width/2) * tileScale);

		mapHeight = _height * tileScale;
		spriteType = Random.Range(0, Services.Prefabs.SPRITES.Length);

		NPCs = 0;
		Pickups = 0;
		NonPickups = 0;
		Sprites = 0;

		highestPointIndices = new List<int> ();

		foreach (Camera c in Services.Player.transform.parent.GetComponentsInChildren<Camera>()) {
			if (c.name != "UpperCamera") {
				c.backgroundColor = Color.black;
				c.clearFlags = CameraClearFlags.Color;
			}
		}

//		levelMesh = Instantiate (Resources.Load<Terrain> ("Terrain"), Vector3.zero, Quaternion.identity).GetComponent<Terrain>();

		GenerateChunk();
	}

	void Update(){

		float playerHeightNormalized = ((Services.Player.transform.position.y - transform.position.y) / (highestPoint * 2.5f));
		float NormalisedToHighestPoint = ((Services.Player.transform.position.y - transform.position.y) / highestPoint);
		if (playerHeightNormalized < 0) {
			playerHeightNormalized = Mathf.Abs(playerHeightNormalized);
		}

//		ground.GetComponent<Renderer> ().material.color = Color.Lerp(Color.white, Color.black, playerHeightNormalized);
		foreach (Camera c in Services.Player.transform.parent.GetComponentsInChildren<Camera>()) {
			if(c.name != "UpperCamera" && c){
				c.backgroundColor = Color.Lerp (c.backgroundColor, Color.Lerp (Color.white, Color.black, playerHeightNormalized), Time.deltaTime * 3);
			}
		}


		RenderSettings.fogColor = Services.Player.GetComponentInChildren<Camera> ().backgroundColor;
		RenderSettings.fogEndDistance = Mathf.Lerp (100, 100, 1- playerHeightNormalized);
		Services.LevelGen.sun.transform.eulerAngles += Vector3.up;
//		Services.LevelGen.sun.intensity = Mathf.Lerp (0, 1, 1- playerHeightNormalized);


		float xCoord = xOrigin;
		float yCoord = yOrigin;

		Mesh mesh = sky.GetComponent<MeshFilter>().mesh;
		Vector3[] verts = mesh.vertices;

		for (int i = 0, y = 0; y <= _length; y++, yCoord++) {

			xCoord = xOrigin;

			for (int x = 0; x <= _width; x++, i++, xCoord++) {
				float perlinVal = OctavePerlin (xCoord * noiseScale, yCoord * noiseScale, 3, 0.5f);
				perlinVal = Mathf.Pow (perlinVal, 0.75f);
				verts [i] = new Vector3 (x, perlinVal * _height, y) * tileScale;
				verts[i] -= new Vector3(_width/2, 0, _length/2) * tileScale;

				float skyCoefficient = Mathf.Pow(perlinVal, 3);
//				skyColor.SetPixel (x, y, Color.Lerp(Color.Lerp(gradient.Evaluate (playerHeightNormalized), Color.black, playerHeightNormalized), Color.Lerp(gradient.Evaluate (playerHeightNormalized), Color.white, playerHeightNormalized), skyCoefficient));
				skyColor.SetPixel (x, y, Color.Lerp(gradient.Evaluate (playerHeightNormalized), Color.black, skyCoefficient));
				Color Grayscale = new Color (skyCoefficient, skyCoefficient, skyCoefficient);
				groundLerpedColour.SetPixel (x, y, _bitmap.GetPixel(x, y) + Grayscale);
	
				verts [i] -= (Vector3.up * Vector3.Distance (verts [i], centre)) * Mathf.Pow(Vector3.Distance (verts [i], centre)/hypotenuse, 0.5f);
			}
		}


		skyColor.Apply();
		groundLerpedColour.Apply();
		mesh.vertices = verts;

		xOrigin += Time.deltaTime;
		yOrigin += Time.deltaTime;

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

	}

	void GenerateChunk(){
		 

		highestPoint = -Mathf.Infinity;
		highestPointIndex = 0;

		float xCoord = xOrigin;
		float yCoord = yOrigin;


		for (int i = 0, y = 0; y <= _length; y++, yCoord++) {

			xCoord = xOrigin;

			for (int x = 0; x <= _width; x++, i++, xCoord++){

				float perlinVal = 0;

				if (usePerlin) {
					perlinVal = OctavePerlin (xCoord * noiseScale, yCoord * noiseScale, 3, 0.5f);
				} else {	
					Color c = _bitmap.GetPixel (x, y);
					perlinVal = ((0.21f * (float)c.r) + (0.72f * (float)c.g) + (0.07f * (float)c.b));
				}
					
				int remapIndex = Mathf.RoundToInt(perlinVal * (Services.LevelGen.NoiseRemapping.Length-1));
				float difference = (perlinVal * (Services.LevelGen.NoiseRemapping.Length-1)) - (float)remapIndex;


                perlinVal = Services.LevelGen.NoiseRemapping[remapIndex];

                if (remapIndex < Services.LevelGen.NoiseRemapping.Length - 1 && difference > 0)
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

				perlinVal = Mathf.Pow (perlinVal, 1f);

				vertices [i] -= new Vector3(_width/2, 0, _length/2) * tileScale;
				vertices [i] += (new Vector3 (x, 0, y) * tileScale) + new Vector3(Random.Range(0, tileScale), 0, Random.Range(0, tileScale));
				//drag height to 0 around edge of circle
				perlinVal = Mathf.Lerp(perlinVal, 0f, Mathf.Pow(Vector3.Distance(centre, vertices[i])/((_width/2) * tileScale), 4));

				vertices [i] += Vector3.up * _height * perlinVal * tileScale;

				if (Vector3.Distance (vertices [i], centre) > ((_width * tileScale) / 2)) {
					float spillover = Vector3.Distance (centre, vertices [i]) - ((_width / 2) * tileScale);
					vertices [i] = Vector3.Lerp (vertices [i], centre, spillover / hypotenuse);
					vertices [i] -= (Vector3.up * spillover)/10;
				} else {
					if (vertices [i].y > highestPoint) {
						highestPoint = vertices [i].y;
						highestPointIndex = i;
					}
				}

				uvs [i] = new Vector2 ((float)x / _width, (float)y / _length);

			}
		}
		normalizedPerlinHeight = mapHeight / highestPoint;
		OutputBitmap ();
		PopulateMap ();
//		ConvertTexToHeight (_bitmap);
		transform.position -= vertices [highestPointIndex];

		GameObject killzone = (GameObject)Instantiate(Services.LevelGen.KillZone, Vector3.zero, Quaternion.identity);
		killzone.transform.parent = transform;
		killzone.transform.localScale += Vector3.right * _width * tileScale * 2;
		killzone.transform.localScale += Vector3.forward * _length * tileScale * 2;
		killzone.transform.localScale += Vector3.up * _height;
		killzone.transform.localPosition = Vector3.zero;
	}

	void PopulateMap(){
		float localMaximum = float.NegativeInfinity;
		int localMaxIndex = 0;

		for (int i = 0, y = 0; y <= _length; y++) {


			for (int x = 0; x <= _width; x++, i++){
				
				float perlinVal = (vertices [i].y / _height)/tileScale;
				perlinVal = Mathf.Clamp01(perlinVal * normalizedPerlinHeight);

					
				_bitmap.SetPixel (x, y, palette[Mathf.RoundToInt(perlinVal * (palette.Length-1))]);

				LevelObjectFactory (perlinVal, vertices [i], new Vector2 (x, y));


				if (Vector3.Distance (vertices [i], vertices [localMaxIndex]) < 25) {
					if (perlinVal > localMaximum) {
						localMaximum = perlinVal;
						localMaxIndex = i;
					}
				} else {
					if (localMaximum > 0.9f) {

						bool nearExistingMax = false;

						foreach (int indice in highestPointIndices) {
							if (Vector3.Distance (vertices [indice], vertices [localMaxIndex]) < 50) {
								nearExistingMax = true;
							}
						}
						if (!nearExistingMax){
							highestPointIndices.Add (i);
						}
					}

					localMaximum = float.NegativeInfinity;
					localMaxIndex = i;
				}

			}
		}

		int length = Random.Range (10, 32);
		foreach (int indice in highestPointIndices) {
			for (int j = 0; j < length; j++) {
				GameObject newObject = Instantiate (Services.Prefabs.PREFABS [(int)Services.TYPES.Sprite] [0], Vector3.zero, Quaternion.identity) as GameObject;
				newObject.GetComponent<SpriteRenderer> ().sprite = Services.Prefabs.SPRITES [(int)Services.SPRITES.image] [Random.Range (0, Services.Prefabs.SPRITES [(int)Services.SPRITES.image].Length)];
				newObject.GetComponent<ChangeSprite> ().SpriteIndex = (int)Services.SPRITES.image;
				newObject.transform.localScale /= newObject.GetComponent<SpriteRenderer> ().bounds.size.y;
				newObject.transform.localScale *= 5;
				newObject.AddComponent<BoxCollider> ().isTrigger = true;

				newObject.transform.parent = transform;
				newObject.transform.localPosition = vertices[indice] + (Vector3.up * j * 2) - (Vector3.up * 2);

				newObject.transform.Rotate (0, Random.Range(0, 360), 0);
			}
		}
		_bitmap.Apply ();
	}
		
		
	public GameObject LevelObjectFactory(float x, Vector3 pos, Vector2 index){

		GameObject newObject = null;
		Color floorColor = _bitmap.GetPixel ((int)index.x, (int)index.y);

		int propIndex = Mathf.RoundToInt (x * (Services.LevelGen.props.Length-1));
		int objectType = (int)Services.LevelGen.props [propIndex];
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
			
		int spriteIndex;

		switch (propIndex) {
		case 0:
			spriteIndex = (int)Services.SPRITES.tall;
			break;

		case 1:
			spriteIndex = (int)Services.SPRITES.tall;
			break;

		case 2:
			spriteIndex = (int)Services.SPRITES.tall;
			break;

		case 3:
			spriteIndex = (int)Services.SPRITES.foliage;
			break;

		default:
			spriteIndex = Random.Range(1, Services.Prefabs.SPRITES.Length);
			break;
		}

		string tag = "Untagged";

		switch (objectType) {
			
		case (int)Services.TYPES.Emptiness:
			return null;
			break;

		case (int)Services.TYPES.NPCs:
			if (NPCs >= Services.LevelGen.maxNPCs) {
				return null;
				break;
			} else {
				NPCs++;
			}
			break;

		case (int)Services.TYPES.Sprite:
			if (Sprites >= Services.LevelGen.maxSprites) {
				return null;
				break;
			} else {
				Sprites++;
			}

			if (spriteIndex == (int)Services.SPRITES.image) {
				tag = "ImageSprite";
			} else {
				tag = "InkSprite";
			}

			break;

		case (int)Services.TYPES.Pickups:
			if (Pickups >= Services.LevelGen.maxObjects) {
				return null;
				break;
			} else {
				Pickups++;
			}
			break;

		case (int)Services.TYPES.NonPickups:
			if (NonPickups >= Services.LevelGen.maxObjects) {
				return null;
				break;
			} else {
				NonPickups++;
			}
			break;

		default:
			return null;
			break;
		}


		newObject = Instantiate (Services.Prefabs.PREFABS[objectType][Random.Range(0, Services.Prefabs.PREFABS[objectType].Length)], Vector3.zero, Quaternion.identity) as GameObject;

		if (newObject.GetComponentInChildren<SpriteRenderer> () != null) {
			
			newObject.GetComponent<SpriteRenderer> ().sprite = Services.Prefabs.SPRITES [spriteIndex] [Random.Range (0, Services.Prefabs.SPRITES [spriteIndex].Length)];
			newObject.GetComponent<SpriteRenderer> ().material.color = floorColor;
			newObject.GetComponent<ChangeSprite> ().SpriteIndex = spriteIndex;


		} else {
//			foreach (Renderer r in newObject.GetComponentsInChildren<Renderer>()) {
//				r.material.shader = Services.Prefabs.FlatShading;
//				r.material.SetColor ("_Color", floorColor);
//			}
//
			if (newObject.GetComponentInChildren<Renderer> ().bounds.size.x > newObject.GetComponentInChildren<Renderer> ().bounds.size.z && newObject.GetComponentInChildren<Renderer> ().bounds.size.x > 1) {
				newObject.transform.localScale /= newObject.GetComponentInChildren<Renderer> ().bounds.size.x;
			} else if(newObject.GetComponentInChildren<Renderer> ().bounds.size.z > 1){
				newObject.transform.localScale /= newObject.GetComponentInChildren<Renderer> ().bounds.size.z;
			}
		}

		newObject.transform.parent = transform;
		newObject.transform.localPosition = pos;

		if (newObject.GetComponentInChildren<SpriteRenderer> () == null) {
			newObject.transform.localScale *= Random.Range (3.0f, 5.0f);
			newObject.transform.localPosition += newObject.GetComponentInChildren<Renderer> ().bounds.extents.y * Vector3.up;
		} else {
//			newObject.transform.localScale *= Random.Range (1.0f, 2.0f);
			newObject.transform.localPosition -= Vector3.up * (newObject.GetComponent<SpriteRenderer> ().bounds.extents.y / 4);
			newObject.AddComponent<BoxCollider> ().isTrigger = true;
		}
			
		Vector3 targetPosition = normals [(int)index.x * (int)index.y];
		targetPosition.y = newObject.transform.position.y;
		newObject.transform.LookAt(targetPosition);
		newObject.transform.Rotate (0, 180, 0);

        if (newObject.name.Contains("(Clone)"))
        {
            newObject.name = newObject.name.Remove(newObject.name.Length - 7, 7);
        }

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
		normals = terrain.normals;
		ground.GetComponent<MeshFilter> ().mesh = terrain;
		ground.GetComponent<MeshCollider> ().sharedMesh = terrain;
//		ground.GetComponent<Renderer> ().enabled = false;

		_bitmap.Apply ();

		groundLerpedColour.SetPixels (_bitmap.GetPixels());
		groundLerpedColour.Apply ();
//		groundLerpedColour.filterMode = FilterMode.Point;

		clouds.vertices = vertices;
		clouds.uv = uvs;
		clouds.triangles = groundTriangles;

		sky.GetComponent<MeshFilter> ().mesh = clouds;
		sky.transform.position += Vector3.up * (highestPoint + 100);
		sky.transform.localScale *= 2;

		Services.LevelGen.cookieLight.cookie = _bitmap;
	}

	public TerrainData ConvertTexToHeight(Texture2D tex)
	{
		Texture2D heightmap = tex;

		TerrainData terrain = levelMesh.terrainData;
		terrain.size = new Vector3 (_width, _height, _length) * tileScale;
		terrain.heightmapResolution = 32;

		float w = heightmap.width;
		float h = heightmap.height;
		int w2 = terrain.heightmapWidth;
		float[,] heightmapData = terrain.GetHeights(0, 0, w2, w2);
		Color[] mapColors = heightmap.GetPixels();
		Color[] map = new Color[w2 * w2];

		if (w2 != w || h != w)
		{
			// Resize using nearest-neighbor scaling if texture has no filtering
			if (heightmap.filterMode == FilterMode.Point)
			{
				float dx = (float)w / w2;
				float dy = (float)h / w2;

				for (int y = 0; y < w2; y++)
				{
					//if (y % 20 == 0)
					//{
					// EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, (float)w2, y));
					//}

					int thisY = (int)((dy * y) * w);
					int yw = y * w2;

					for (int x = 0; x < w2; x++)
					{
						map[yw + x] = mapColors[thisY + (int)dx * x];
					}
				}
			}

			// Otherwise resize using bilinear filtering
			else
			{
				float ratioX = (float)(1.0 / (w2) / (w - 1));
				float ratioY = (float)(1.0 / (w2) / (h - 1));

				for (int y = 0; y < w2; y++)
				{
					//if (y % 20 == 0)
					//{
					// EditorUtility.DisplayProgressBar("Resize", "Calculating texture", Mathf.InverseLerp(0.0f, w2, y));
					//}

					float yy = Mathf.Floor(y * ratioY);
					float y1 = yy * w;
					float y2 = (yy + 1) * w;
					float yw = y * w2;

					for (int x = 0; x < w2; x++)
					{
						float xx = Mathf.Floor(x * ratioX);
						Color bl = mapColors[(int)y1 + (int)xx];
						Color br = mapColors[(int)y1 + (int)xx + 1];
						Color tl = mapColors[(int)y2 + (int)xx];
						Color tr = mapColors[(int)y2 + (int)xx + 1];

						float xLerp = x * ratioX - xx;
						map[(int)yw + x] = Color.Lerp(Color.Lerp(bl, br, xLerp), Color.Lerp(tl, tr, xLerp), y * ratioY - yy);
					}
				}
			}
			//EditorUtility.ClearProgressBar();
		}
		else
		{
			// Use original if no resize is needed
			map = mapColors;
		}

		//Assign texture data to heightmap
		for (int y = 0; y < w2; y++)
		{
			for (int x = 0; x < w2; x++)
			{
				heightmapData[y, x] = map[y * w2 + x].grayscale;
			}
		}
		terrain.SetHeights(0, 0, heightmapData);
		levelMesh.terrainData = terrain;
		levelMesh.Flush ();

//		levelMesh.terrainData.SetDetailLayer


		return terrain;

	}


	public void OnDestroyed(){

	}
}

