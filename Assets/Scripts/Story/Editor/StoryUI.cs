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
		if (story.Forest == null) {
			story.Forest = new Forest();
		}
	}

	public override void OnInspectorGUI () {
		GUILayout.Label("Tree : " + story.selectedTreeIndex + " : " + story.Forest.trees.Length);
		int newTreeIndex = EditorGUILayout.IntSlider(story.selectedTreeIndex, 0, story.Forest.trees.Length - 1);
		if (newTreeIndex != story.selectedTreeIndex && story.Forest.trees[story.selectedTreeIndex].existing) {
			story.selectedTreeIndex = newTreeIndex;
			story.selectedNodeIndex = Forest.entryTreeIndex;
		}
		if (GUILayout.Button("New Tree", GUILayout.Height(20))) {
			story.NewTree();
		}
		if (GUILayout.Button("Initiate Tree", GUILayout.Height(20))) {
			story.InitiateTree();
		}
		if (GUILayout.Button("Delete Tree", GUILayout.Height(20))) {
			story.DeleteTree();
			story.selectedTreeIndex = 0;
		}
		story.treeName = EditorGUILayout.TextField("Name: ", story.treeName);
		story.SetTreeName();
//		EditorGUILayout.PropertyField(new SerializedObject(story).FindProperty("treeIntervals"));
	
		GUILayout.Space(20);

		GUILayout.Label("Node : " + story.selectedNodeIndex + " : " + story.Forest.trees[story.selectedTreeIndex].nodes.Length);
		if (GUILayout.Button("Insert Node", GUILayout.Height(20))) {
			story.InsertNode();
		}
		if (GUILayout.Button("Append Node", GUILayout.Height(20))) {
			story.AppendNode();
		}
		if (GUILayout.Button("Remove Node", GUILayout.Height(20))) {
			story.RemoveNode();
		}
		story.nodeName = EditorGUILayout.TextField("Name: ", story.nodeName);
		story.SetNodeName();


		if (story.refreshVisualization) {
			story.Forest.trees[story.selectedTreeIndex].LocateNodes(0);
		}

		GUILayout.Space(20);

		DrawDefaultInspector();

		SceneView.RepaintAll();
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
			selectedNodeDepth = (int)(mousePositionInScene.y + 0.5);
			selectedNodeX = (int)(mousePositionInScene.x + 0.5);

			if (story.Forest.trees[story.selectedTreeIndex].FindNode(selectedNodeDepth, selectedNodeX) != -1) {
				story.selectedNodeIndex = story.Forest.trees[story.selectedTreeIndex].FindNode(selectedNodeDepth, selectedNodeX);
				story.GetNodeName();
			}

			guiEvent.Use();
		}
		
		SceneView.RepaintAll();
	}
}
