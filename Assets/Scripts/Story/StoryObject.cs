using UnityEngine;
using System.Collections;

public class StoryObject : MonoBehaviour {

	#region Editing

	public int selectedTreeIndex;
	public int selectedNodeIndex;
	public int selectedActiveIntervalIndex;

	#endregion

	public NodeConnection[] activeIntervals;

	void Start () {
		StoryObjectManager.Register(gameObject, activeIntervals);
	}

	#region Gizmos
	
	void OnDrawGizmosSelected() {
		if (Story.GetStory().Forest.trees[selectedTreeIndex].deleted) {
			return;
		}
		
		DrawNodeConnections();
		
		DrawPaths();
		
		DrawNodes();
	}
	
	void DrawNodes () {
		// Nodes
		Gizmos.color = Color.cyan;
		if (Story.GetStory().Forest.trees[selectedTreeIndex].nodes[selectedNodeIndex].existing) {
			Node selectedNode = Story.GetStory().Forest.trees[selectedTreeIndex].nodes[selectedNodeIndex];
			//Gizmos.DrawWireSphere(selectedNode.location.vector, 0.5f);
			Gizmos.DrawWireSphere(selectedNode.location.vector, 0.3f);
			UnityEditor.Handles.Label(selectedNode.location.vector + new Vector3(-0.5f, 0.6f, 0f), selectedNode.index.ToString());
			UnityEditor.Handles.Label(selectedNode.location.vector + new Vector3(0f, 0.6f, 0f), selectedNode.name);
		}
		
		Gizmos.color = Color.red;
		foreach (var node in Story.GetStory().Forest.trees[selectedTreeIndex].nodes) {
			if (node.deleted) {
				continue;
			}
			if (node.index != selectedNodeIndex) {
				UnityEditor.Handles.Label(node.location.vector + new Vector3(-0.5f, 0.5f, 0f), node.index.ToString());
				UnityEditor.Handles.Label(node.location.vector + new Vector3(0f, 0.5f, 0f), node.name);
			}
			Gizmos.DrawWireSphere(node.location.vector, 0.4f);
		}	
	}
	
	void DrawNodeConnections () {
		// Node connections
		Gizmos.color = Color.blue;
		foreach (var node in Story.GetStory().Forest.trees[selectedTreeIndex].nodes) {
			if (node.deleted) {
				continue;
			}
			
			foreach (var childIndex in node.childIndexes) {
				Gizmos.DrawLine(node.location.vector, Story.GetStory().Forest.trees[selectedTreeIndex].nodes[childIndex].location.vector);
			}
		}
	}
	
	void DrawPaths () {
		// Alternative tree available
		Gizmos.color = Color.magenta;

		foreach (var activeInterval in activeIntervals) {
			if (activeInterval.treeIndex == selectedTreeIndex) {
				int[] path = Story.GetStory().Forest.FindPath(activeInterval);
				if (path.Length == 0) {
					Debug.LogWarning(
						"Invalide NodeConnection"
						+ " from Node " + activeInterval.startNodeIndex 
						+ " to Node " + activeInterval.endNodeIndex
						+ " in Tree " + activeInterval.treeIndex);
					continue;
				}
				
				int oldNodeIndex = path[0];
				for (int i = 1; i < path.Length; i++) {
					
					Vector3 start = Story.GetStory().Forest.trees[selectedTreeIndex].nodes[oldNodeIndex].location.vector;
					Vector3 end = Story.GetStory().Forest.trees[selectedTreeIndex].nodes[path[i]].location.vector;
					
					DrawPath(start, end);
					
					oldNodeIndex = path[i];
				}
			}
		}
	}
	
	void DrawPath (Vector3 start, Vector3 end) {
		for (float j = 0; j <= 1; j+=(0.2f / Vector3.Distance(start, end))) {
			Gizmos.DrawWireCube(Vector3.Lerp(start, end, j), Vector3.one * 0.2f);
		}
	}
	
	#endregion
}
