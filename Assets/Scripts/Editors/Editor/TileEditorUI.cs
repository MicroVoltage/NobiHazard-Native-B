using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TileEditor))]
public class TileEditorUI : Editor {
	// Reference to the component
	TileEditor tileEditor;
	
	// Scrollbar position
	Vector2 scrollbarPosition;

	Vector3 mousePositionInScene;
	Vector3 mousePositionOnMap;
	
	void OnEnable () {
		tileEditor = (TileEditor)target;
	}
	
	void OnSceneGUI () {
		Event guiEvent = Event.current;
		
		if (guiEvent.isMouse) {
			Tools.current = Tool.View;
			Tools.viewTool = ViewTool.FPS;
		} else {
			return;
		}

		Vector2 mousePositionOnScreen = new Vector2 (guiEvent.mousePosition.x, guiEvent.mousePosition.y);
		CalculateMousePositionOnMap(mousePositionOnScreen);

		if (guiEvent.type == EventType.MouseMove) {
			return;
		}
		
		if (guiEvent.button == 0) {
			Draw((int)mousePositionOnMap.x, (int)mousePositionOnMap.y);
			guiEvent.Use();
		} else if (guiEvent.button == 1) {
			Delete((int)mousePositionOnMap.x, (int)mousePositionOnMap.y);
			guiEvent.Use();
		}

		SceneView.RepaintAll();
	}

	Vector3 CalculateMousePositionOnMap (Vector2 mousePositionOnScreen) {
		Ray mouseRay = HandleUtility.GUIPointToWorldRay(mousePositionOnScreen);
		mousePositionInScene = mouseRay.origin;
		mousePositionInScene.z = 0;

		mousePositionOnMap = new Vector3(
			((int)(mousePositionInScene.x / tileEditor.tileSize)) * tileEditor.tileSize + tileEditor.tileSize * 0.5f,
			((int)(mousePositionInScene.y / tileEditor.tileSize)) * tileEditor.tileSize + tileEditor.tileSize * 0.5f);
		
		return mousePositionOnMap;
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		
		if (tileEditor.generateTiles) {
			tileEditor.GenerateTiles();
		}

		//Show scroll bar For next layout
		tileEditor.tileTextureID = GUILayout.SelectionGrid(
			tileEditor.tileTextureID,
			tileEditor.tileTextures,
			6,
			tileEditor.guiSkin.GetStyle("Button"));
	}
	
	void Draw (int x, int y) {
		string tileName = GetTileName(x, y);
		Sprite tileSprite = tileEditor.tiles[tileEditor.tileTextureIDtoTileNo[tileEditor.tileTextureID]];

		if (!tileEditor.transform.Find(tileName)) {
			//lets you undo editor changes
			Undo.IncrementCurrentGroup();
			
			GameObject tile = new GameObject(tileName);
			SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
			renderer.sprite = tileSprite;

			tile.transform.position = new Vector3(x, y);
			tile.transform.parent = tileEditor.transform;

			Undo.RegisterCreatedObjectUndo(tile, "Create Tile");

		} else if (tileEditor.transform.Find(tileName).GetComponent<SpriteRenderer>().sprite != tileSprite) {
			Delete(x, y);
			Draw(x, y);
		}
	}

	
	void Delete (int x, int y) {
		string tileName = GetTileName(x, y);
		if (tileEditor.transform.Find(tileName)) {
			Undo.IncrementCurrentGroup();
			Undo.DestroyObjectImmediate(tileEditor.transform.Find(tileName).gameObject);
		}
	}

	string GetTileName (int x, int y) {
		return x + ", " + y;
	}
}
