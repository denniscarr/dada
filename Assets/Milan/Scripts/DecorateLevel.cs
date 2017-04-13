using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorateLevel : MonoBehaviour {

	Terrain terrainToPopulate;
	public int patchDetail, grassDensity;

		void Start()
		{
			terrainToPopulate = transform.gameObject.GetComponent<Terrain>();
			terrainToPopulate.terrainData.SetDetailResolution(grassDensity, patchDetail);
	
			int[,] newMap = new int[grassDensity, grassDensity];
	
			for (int i = 0; i < grassDensity; i++){
			
				for (int j = 0; j < grassDensity; j++){
				
					// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
					float height = terrainToPopulate.terrainData.GetHeight( i, j );
					if (height < 10.0f)
					{
						newMap[i, j] = 6;
					}
					else
					{
						newMap[i, j] = 0;
					}
				}
			}
			terrainToPopulate.terrainData.SetDetailLayer(0, 0, 0, newMap);
		}
	
	

	void Update () {
		
	}
}
