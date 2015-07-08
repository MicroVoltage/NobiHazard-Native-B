using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {

	public int selectedNodeIndex;

	public bool insertNode;
	public bool appendNode;
	public bool removeNode;

	public bool initiateTree;
	public bool refreshVisualization;
	public bool followNewNode;


	public Tree tree = new Tree();


	void OnDrawGizmos() {
		foreach (var node in tree.nodes) {
			if (node == null) {
				continue;
			}

			Gizmos.color = Color.blue;
			foreach (var childIndex in node.childIndexes) {
				Gizmos.DrawLine(node.location.vector, tree.nodes[childIndex].location.vector);
			}

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(node.location.vector, 0.5f);
		}
		if (tree.nodes[selectedNodeIndex] == null) {
			return;
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(tree.nodes[selectedNodeIndex].location.vector, 0.6f);
	}
}
