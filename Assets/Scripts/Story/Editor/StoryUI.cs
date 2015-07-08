using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Story))]
public class StoryUI : Editor {
	Story story;

	int selectedNodeDepth;
	int selectedNodeX;

	void OnEnable () {
		story = (Story)target;
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector();

		if (story.insertNode) {
			int newNodeIndex = story.tree.InsertNode(story.selectedNodeIndex);
			if (story.followNewNode) {
				story.selectedNodeIndex = newNodeIndex;
			}
			story.insertNode = false;
		}
		if (story.appendNode) {
			int newNodeIndex = story.tree.AppendNode(story.selectedNodeIndex);
			if (story.followNewNode) {
				story.selectedNodeIndex = newNodeIndex;
			}
			story.appendNode = false;
		}

		if (story.removeNode) {
			int newNodeIndex = story.tree.RemoveNode(story.selectedNodeIndex);
			if (story.followNewNode) {
				story.selectedNodeIndex = newNodeIndex;
			}
			story.removeNode = false;
		}


		if (story.initiateTree) {
			story.tree.InitiateTree();
			story.removeNode = false;
		}

		if (story.refreshVisualization) {
			story.tree.LocateNodes(0);
		}
	}

	void OnSceneGUI () {
		Event guiEvent = Event.current;
		
		if (guiEvent.isMouse) {
			Tools.current = Tool.View;
			Tools.viewTool = ViewTool.FPS;
		} else {
			return;
		}
		
		if (guiEvent.type == EventType.MouseMove) {
			return;
		}
		
		if (guiEvent.button == 0) {
			Vector2 mousePositionOnScreen = new Vector2 (guiEvent.mousePosition.x, guiEvent.mousePosition.y);
			Ray mouseRay = HandleUtility.GUIPointToWorldRay(mousePositionOnScreen);
			Vector3 mousePositionInScene = mouseRay.origin;
			selectedNodeDepth = (int)mousePositionInScene.y;
			selectedNodeX = (int)mousePositionInScene.x;

			if (story.tree.FindNode(selectedNodeDepth, selectedNodeX) != -1) {
				story.selectedNodeIndex = story.tree.FindNode(selectedNodeDepth, selectedNodeX);
			}

			guiEvent.Use();
		}
		
		SceneView.RepaintAll();
	}
}
