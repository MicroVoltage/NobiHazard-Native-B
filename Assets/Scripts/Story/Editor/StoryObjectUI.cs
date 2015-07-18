using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StoryObject))]
public class StoryObjectUI : Editor {
	StoryObject storyObject;

	void OnEnable () {
		storyObject = (StoryObject)target;
	}

	public override void OnInspectorGUI () {
		GUILayout.Label("Tree : " + storyObject.selectedTreeIndex + " : " + (Story.GetStory().Forest.trees.Length-1));
		int newTreeIndex = EditorGUILayout.IntSlider(storyObject.selectedTreeIndex, 0, Story.GetStory().Forest.trees.Length - 1);
		if (newTreeIndex != storyObject.selectedTreeIndex && Story.GetStory().Forest.trees[storyObject.selectedTreeIndex].existing) {
			storyObject.selectedTreeIndex = newTreeIndex;
			storyObject.selectedNodeIndex = Forest.entryTreeIndex;
		}

		GUILayout.Label("Node : " + storyObject.selectedNodeIndex + " : " + (Story.GetStory().Forest.trees[storyObject.selectedTreeIndex].nodes.Length-1));

		GUILayout.Label("Active Interval : " + storyObject.selectedActiveIntervalIndex + " : " + (storyObject.activeIntervals.Length-1));

		storyObject.selectedActiveIntervalIndex = EditorGUILayout.IntSlider(storyObject.selectedActiveIntervalIndex, 0, storyObject.activeIntervals.Length - 1);

		if (GUILayout.Button("Set Start", GUILayout.Height(20))) {
			storyObject.activeIntervals[storyObject.selectedActiveIntervalIndex].treeIndex = storyObject.selectedTreeIndex;
			storyObject.activeIntervals[storyObject.selectedActiveIntervalIndex].startNodeIndex = storyObject.selectedNodeIndex;
		}
		if (GUILayout.Button("Set End", GUILayout.Height(20))) {
			storyObject.activeIntervals[storyObject.selectedActiveIntervalIndex].treeIndex = storyObject.selectedTreeIndex;
			storyObject.activeIntervals[storyObject.selectedActiveIntervalIndex].endNodeIndex = storyObject.selectedNodeIndex;
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
			int selectedNodeDepth = (int)(mousePositionInScene.y + 0.5);
			int selectedNodeX = (int)(mousePositionInScene.x + 0.5);
			
			if (Story.GetStory().Forest.trees[storyObject.selectedTreeIndex].FindNode(selectedNodeDepth, selectedNodeX) != -1) {
				storyObject.selectedNodeIndex = Story.GetStory().Forest.trees[storyObject.selectedTreeIndex].FindNode(selectedNodeDepth, selectedNodeX);
			}
			
			guiEvent.Use();
		}
		
		SceneView.RepaintAll();
	}
}
