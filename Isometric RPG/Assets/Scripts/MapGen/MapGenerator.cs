using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEditor;


public class MapGenerator : MonoBehaviour {

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public Tilemap terrainMap;
	public TileBase waterTile;
	public TileBase grassTile;
	public TileBase sandTile;
	public bool autoUpdate;

	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				// terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), waterTile);

				if(noiseMap[x,y] > 0.4 ) {
					// Debug.Log("grass");
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), grassTile);
				}
				else if (noiseMap[x,y] < 0.4 && noiseMap[x,y] > 0.3 ) {
					// Debug.Log("sand");
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), sandTile);
				}
				else if (noiseMap[x,y] < 0.3) {
					// Debug.Log("sand");
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), waterTile);
				}

			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				TileBase centerTile = terrainMap.GetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0));

				TileBase topTile = terrainMap.GetTile(new Vector3Int((-x + mapWidth / 2), (-y + mapHeight / 2) + 1, 0));
				TileBase bottomTile = terrainMap.GetTile(new Vector3Int((-x + mapWidth / 2), (-y + mapHeight / 2) - 1, 0));
				TileBase rightTile = terrainMap.GetTile(new Vector3Int((-x + mapWidth / 2) + 1, (-y + mapHeight / 2), 0));
				TileBase leftTile = terrainMap.GetTile(new Vector3Int((-x + mapWidth / 2) - 1, (-y + mapHeight / 2), 0));

				if(centerTile.name == "Water") {
					// Debug.Log("WaterTile @ " +  x + "/" +  y);

					if(topTile != null && topTile.name =="Water") {
						Debug.Log("above " +  x + "/" +  y);
					}
					else if(bottomTile != null && bottomTile.name =="Water") {
						Debug.Log("below " +  x + "/" +  y);
					}
					else if(rightTile != null && rightTile.name =="Water") {
						Debug.Log("right of " +  x + "/" +  y);
					}
					else if(leftTile != null && leftTile.name =="Water") {
						Debug.Log("left of " +  x + "/" +  y);
					}
				}
			}
		}
	}

	public void SaveAssetMap() {
        string saveName = "Map " + seed;
        var mf = GameObject.Find("Grid");

        if (mf)
        {
            var savePath = "Assets/" + saveName + ".prefab";
            if (PrefabUtility.CreatePrefab(savePath,mf))
            {
                EditorUtility.DisplayDialog("Tilemap saved", "Your Tilemap was saved under" + savePath, "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Tilemap NOT saved", "An ERROR occured while trying to saveTilemap under" + savePath, "Continue");
            }
        }
    }

	public void clearTiles() {
		terrainMap.ClearAllTiles();
	}

	void OnValidate() {
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapHeight < 1) {
			mapHeight = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}

}
