using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public Tilemap terrainMap;
	public IsometricRuleTile waterTile;
	public Tile grassTile;
	public IsometricRuleTile sandTile;

	void Start () {
		GenerateMap();
	}

	public void GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapWidth, mapHeight, noiseScale);

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				if(noiseMap[x,y] > 0.4 ) {
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0),grassTile);
				}
				else if (noiseMap[x,y] < 0.4 && noiseMap[x,y] > 0.3 ) {
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0),sandTile);
				}
				else if (noiseMap[x,y] < 0.3 ) {
					terrainMap.SetTile(new Vector3Int(-x + mapWidth / 2, -y + mapHeight / 2, 0),waterTile);
				}
			}
		}
		// MapDisplay display = FindObjectOfType<MapDisplay> ();
		// display.DrawNoiseMap (noiseMap);
	}
	
}
