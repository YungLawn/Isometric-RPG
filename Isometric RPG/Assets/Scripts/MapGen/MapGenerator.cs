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
	public Tile waterTile;
	public Tile grassTile;
	public IsometricRuleTile sandTile;
	public bool autoUpdate;
	[HideInInspector]

	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

		for (int y = -10; y < mapHeight + 10; y++) {
			for (int x = -10; x < mapWidth + 10; x++) {
				
				terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), waterTile);

			}
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
			
				if(noiseMap[x,y] > 0.4 ) {
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), grassTile);
				}
				else if (noiseMap[x,y] < 0.4 && noiseMap[x,y] > 0.3 ) {
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0), sandTile);
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
