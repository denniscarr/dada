using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myFractalTexture : MonoBehaviour
{
	//Variables

	Texture2D heightMap;
	int width;
	int height;


	public void Start(){
	}

	#region OtherCalculateFunction

	#endregion

	#region HeightmapFromTexture
	public static TerrainData ConvertTexToHeight(Texture2D tex)
	{
		Texture2D heightmap = tex;
		if (heightmap == null)
		{
			//EditorUtility.DisplayDialog("No texture selected", "Please select a texture.", "Cancel");
			return null;
		}
		//Undo.RegisterUndo(Terrain.activeTerrain.terrainData, "Heightmap From Texture");


		TerrainData terrain = new TerrainData();
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
		return terrain;

	}

	#endregion
}
