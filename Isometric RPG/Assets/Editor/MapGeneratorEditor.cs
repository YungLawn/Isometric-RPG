using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		MapGenerator mapGen = (MapGenerator)target;

		if (DrawDefaultInspector ()) {
			if (mapGen.autoUpdate) {
				mapGen.clearTiles();
				mapGen.GenerateMap ();
			}
		}

		if (GUILayout.Button ("Generate Map")) {
			mapGen.clearTiles();
			mapGen.GenerateMap ();
		}

		if (GUILayout.Button ("Save Map")) {
			mapGen.SaveAssetMap ();
		}
		if (GUILayout.Button ("Clear Map")) {
			mapGen.clearTiles();
		}
		if (GUILayout.Button ("Clean Up")) {
			mapGen.cleanUp();
		}
	}
}
