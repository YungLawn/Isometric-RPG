using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;


public class MapGenerator : MonoBehaviour {

	public int mapSize;
	public float noiseScale;

	[Range(0,1)]
	public float grassLimit;
	[Range(0,1)]
	public float sandLimit;
	[Range(0,1)]
	public float vegetationZone;
	[Range(1,50)]
	public int treeChance;
	[Range(1,20)]
	public int grassChance;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public Tilemap terrainMap;
	public Tilemap obstacleMap;
	public TileBase waterTile;
	public TileBase grassTile;
	public TileBase sandTile;
	public TileBase tree;
	public TileBase grass;
	public bool autoUpdate;
	[Range(0,4)]
	public int cleanUpIterations;
	[Range(2,20)]
	public int beachSize;

	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapSize, mapSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

		float centerX = mapSize / 2f; // X-coordinate of the center of the circular area
		float centerY = mapSize / 2f; // Y-coordinate of the center of the circular area
		float radius = Mathf.Min(mapSize, mapSize) / 2f; // Radius of the circular area

		for (int y = -mapSize; y < mapSize * 2; y++) {
			for (int x = -mapSize; x < mapSize * 2; x++) {
				float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
				Vector3Int position = new Vector3Int(-x + mapSize / 2, -y + mapSize / 2, 0);

				if (distance <= radius + beachSize + 20) {
					terrainMap.SetTile(position, waterTile);
				}

			}
		}

		for (int y = 0; y < mapSize; y++) {
			for (int x = 0; x < mapSize; x++) {
				float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
				Vector3Int position = new Vector3Int(-x + mapSize / 2, -y + mapSize / 2, 0);


				if (distance <= radius) {
					if (noiseMap[x, y] >= sandLimit)
						terrainMap.SetTile(position, sandTile);
				}

				if (distance <= radius - beachSize) {
					// Vector3Int position = new Vector3Int(-x + mapSize / 2, -y + mapSize / 2, 0);

					if (noiseMap[x, y] >= grassLimit) {
						terrainMap.SetTile(position, grassTile);
					} else if (noiseMap[x, y] <= grassLimit && noiseMap[x, y] > sandLimit) {
						terrainMap.SetTile(position, sandTile);
					} else if (noiseMap[x, y] <= sandLimit) {
						terrainMap.SetTile(position, waterTile);
					}
				}
			}
		}

		placeObstacles(noiseMap, centerX, centerY, radius);

		for(int i = 0; i < cleanUpIterations; i++) {
			cleanUp();
		}

	}

	void placeObstacles(float[,] noiseMap, float centerX, float centerY, float radius) {

		for (int y = 0; y < mapSize; y++) {
			for (int x = 0; x < mapSize; x++) {
				float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));

				if (distance <= (radius - beachSize) * 0.95f) {
					Vector3Int position = new Vector3Int(-x + mapSize / 2, -y + mapSize / 2, 0);

					int treeValue = Random.Range(0,treeChance);
					int grassValue = Random.Range(0,grassChance);

					if (noiseMap[x,y] > vegetationZone) {
						// Debug.Log("tree");
						if(treeValue == 1){
							obstacleMap.SetTile(position, tree);
						}
						if(grassValue == 1 || grassValue == 2){
							obstacleMap.SetTile(position, grass);
						}
					}
				}
			}
		}

	}

	public void cleanUp() {

		for (int y = 0; y < mapSize; y++) {
			for (int x = 0; x < mapSize; x++) {

				Vector3Int position = new Vector3Int(-x + mapSize / 2, -y + mapSize / 2, 0);

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
		obstacleMap.ClearAllTiles();
	}

	void OnValidate() {
		if (mapSize < 1) {
			mapSize = 1;
		}
		if (mapSize < 1) {
			mapSize = 1;
		}
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}

}
