using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;


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

				Vector3Int position = new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0);

				if(noiseMap[x,y] > 0.4 ) {
					// Debug.Log("grass");
					terrainMap.SetTile(position, grassTile);
				}
				else if (noiseMap[x,y] < 0.4 && noiseMap[x,y] > 0.3 ) {
					// Debug.Log("sand");
					terrainMap.SetTile(position, sandTile);
				}
				else if (noiseMap[x,y] < 0.3) {
					// Debug.Log("sand");
					terrainMap.SetTile(position, waterTile);
				}

			}
		}

		cleanUp();
		cleanUp();

	}

	public void cleanUp() {

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {

				Vector3Int position = new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0);

				// TileBase centerTile = terrainMap.GetTile(position);
				if(terrainMap.GetTile(position) == waterTile) {
					int cnt = 0;
					// Debug.Log(x + " | " + y);
					if(terrainMap.GetTile(position + new Vector3Int(0,1,0)) != waterTile) {
						// Debug.Log("up");
						cnt ++;
					}
					if(terrainMap.GetTile(position - new Vector3Int(0,1,0)) != waterTile) {
						// Debug.Log("down");
						cnt ++;
					}
					if(terrainMap.GetTile(position - new Vector3Int(1,0,0)) != waterTile) {
						// Debug.Log("left");
						cnt ++;
					}
					if(terrainMap.GetTile(position + new Vector3Int(1,0,0)) != waterTile) {
						// Debug.Log("right");
						cnt ++;
					}
					
					if(cnt >= 3) {
						// Debug.Log("Water" + x + " | " + y);
						terrainMap.SetTile(position, sandTile);
					}

				}

				if(terrainMap.GetTile(position) == grassTile) {
					int cnt = 0;
					// Debug.Log(x + " | " + y);
					if(terrainMap.GetTile(position + new Vector3Int(0,1,0)) != grassTile) {
						// Debug.Log("up");
						cnt ++;
					}
					if(terrainMap.GetTile(position - new Vector3Int(0,1,0)) != grassTile) {
						// Debug.Log("down");
						cnt ++;
					}
					if(terrainMap.GetTile(position - new Vector3Int(1,0,0)) != grassTile) {
						// Debug.Log("left");
						cnt ++;
					}
					if(terrainMap.GetTile(position + new Vector3Int(1,0,0)) != grassTile) {
						// Debug.Log("right");
						cnt ++;
					}
					
					if(cnt >= 3) {
						// Debug.Log("Grass" + x + " | " + y);
						terrainMap.SetTile(position, sandTile);
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
