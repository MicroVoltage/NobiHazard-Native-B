using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Story : MonoBehaviour {
	public static Forest forest;

	// Editing ultilities
	int selectedTreeIndexValue;
	public int selectedTreeIndex {
		set {
			selectedTreeIndexValue = value;
			GetTreeName();
			GetTreeIntervals();
		}
		get { return selectedTreeIndexValue; }
	}
	public string treeName;
	public NodeConnection[] treeIntervals;

	int selectedNodeIndexValue;
	public int selectedNodeIndex {
		set {
			selectedNodeIndexValue = value;
			GetNodeName();
		}
		get { return selectedNodeIndexValue; }
	}
	public string nodeName;
	
	public bool refreshVisualization;
	public bool followNewNode;


	public Forest Forest;

	// Runtime states
	public static CompoundIndex nodePointer;
	public static Stack<CompoundIndex> nodePointerStack;

	// Constants
	public const string initializedFlagKey = "StoryInitialized";
	public const string nodePointerTreeIndexKey = "nodePointerTreeIndex";
	public const string nodePointerNodeIndexKey = "nodePointerNodeIndex";
	public const string nodePointerStackTreeIndexesKey = "nodePointerStackTreeIndexes";
	public const string nodePointerStackNodeIndexesKey = "nodePointerStackNodeIndexes";


	// Editing methods ----------------------------------------------------------------------------------------------
	public void ResetForset () {
		Forest = new Forest();
		selectedTreeIndex = 0;
		selectedNodeIndex = 0;

	}

	public void NewTree () {
		int newTreeIndex = Forest.NewTree();
		if (followNewNode) {
			selectedTreeIndex = newTreeIndex;
			selectedNodeIndex = 0;
		}
	}

	public void InitiateTree () {
		Forest.trees[selectedTreeIndex].InitiateTree(selectedTreeIndex);
		selectedNodeIndex = Forest.entryNodeIndex;
	}

	public void DeleteTree () {
		Forest.DeleteTree(selectedTreeIndex);
		selectedTreeIndex = 0;
		selectedNodeIndex = 0;
	}

	public void GetTreeName () {
		treeName = Forest.trees[selectedTreeIndex].name;
	}
	
	public void SetTreeName () {
		Forest.trees[selectedTreeIndex].name = treeName;
	}

	public void GetTreeIntervals () {
		treeIntervals = Forest.trees[selectedTreeIndex].treeIntervals;
	}

	public void SetTreeIntervals () {
		Forest.trees[selectedTreeIndex].treeIntervals = treeIntervals;
	}

	// --------------------------------------------------------------------------------------------------

	public void InsertNode () {
		int newNodeIndex = Forest.trees[selectedTreeIndex].InsertNode(selectedNodeIndex);
		if (followNewNode) {
			selectedNodeIndex = newNodeIndex;
		}
	}

	public void AppendNode () {
		int newNodeIndex = Forest.trees[selectedTreeIndex].AppendNode(selectedNodeIndex);
		if (followNewNode) {
			selectedNodeIndex = newNodeIndex;
		}
	}

	public void RemoveNode () {
		int parentNodeIndex = Forest.trees[selectedTreeIndex].RemoveNode(selectedNodeIndex);
		selectedNodeIndex = parentNodeIndex;
	}

	public void GetNodeName () {
		nodeName = Forest.GetNode(selectedTreeIndex, selectedNodeIndex).name;
	}

	public void SetNodeName () {
		Forest.GetNode(selectedTreeIndex, selectedNodeIndex).name = nodeName;
	}

	// Runtime methods ----------------------------------------------------------------------------------------------
	void Awake () {
		forest = Forest;

		LoadStory();
	}

	public static void InitializeStory () {
		nodePointer = new CompoundIndex(0, 0);
		nodePointerStack = new Stack<CompoundIndex>();

		SaveStory();
		
		DataSL.SaveData<bool>(initializedFlagKey, true);
	}

	public static void SaveStory () {
		DataSL.SaveData<int>(nodePointerTreeIndexKey, nodePointer.treeIndex);
		DataSL.SaveData<int>(nodePointerNodeIndexKey, nodePointer.nodeIndex);

		CompoundIndex[] nodePointerStackArray = nodePointerStack.ToArray();
		int[] nodePointerStackTreeIndexes = new int[nodePointerStackArray.Length];
		int[] nodePointerStackNodeIndexes = new int[nodePointerStackArray.Length];
		for (int i = 0; i < nodePointerStackArray.Length; i++) {
			nodePointerStackTreeIndexes[i] = nodePointerStackArray[i].treeIndex;
			nodePointerStackNodeIndexes[i] = nodePointerStackArray[i].nodeIndex;
		}
		DataSL.SaveArray<int>(nodePointerStackTreeIndexesKey, nodePointerStackTreeIndexes);
		DataSL.SaveArray<int>(nodePointerStackNodeIndexesKey, nodePointerStackNodeIndexes);
	}

	public static void LoadStory () {
		if (!DataSL.LoadData<bool>(initializedFlagKey)) {
			InitializeStory();
			return;
		}

		nodePointer = new CompoundIndex(DataSL.LoadData<int>(nodePointerTreeIndexKey), DataSL.LoadData<int>(nodePointerNodeIndexKey));

		nodePointerStack = new Stack<CompoundIndex>();
		int[] nodePointerStackTreeIndexes = DataSL.LoadArray<int>(nodePointerStackTreeIndexesKey);
		int[] nodePointerStackNodeIndexes = DataSL.LoadArray<int>(nodePointerStackNodeIndexesKey);
		for (int i = 0; i < nodePointerStackTreeIndexes.Length; i++) {
			nodePointerStack.Push(new CompoundIndex(nodePointerStackTreeIndexes[i], nodePointerStackNodeIndexes[i]));
		}
	}

	public static CompoundIndex GetNodePointer () {
		return nodePointer;
	}

	public static Node GetNode (int nodeIndex) {
		return forest.GetNode(nodePointer.treeIndex, nodeIndex);
	}

	public static int[] GetNodeChildIndexes () {
		return forest.GetNode(nodePointer).childIndexes;
	}

	public static CompoundIndex[] GetNodeChildCompoundIndexes () {
		int[] nodeChildIndexes = GetNodeChildIndexes();
		CompoundIndex[] nodeChildCompoundIndexes = new CompoundIndex[nodeChildIndexes.Length];
		foreach (var nodeChildCompoundIndex in nodeChildCompoundIndexes) {
			nodeChildCompoundIndex.treeIndex = nodePointer.treeIndex;
		}
		return nodeChildCompoundIndexes;
	}

	public static string[] GetNodeChildNames () {
		int[] nodeChildIndexes = GetNodeChildIndexes();
		string[] nodeChildNames = new string[nodeChildIndexes.Length];
		for (int i = 0; i < nodeChildNames.Length; i++) {
			nodeChildNames[i] = GetNode(nodeChildIndexes[i]).name;
		}

		return nodeChildNames;
	}

	public static bool IsNodeAccessible (int nodeIndex) {
		int[] nodeChildIndexes = GetNodeChildIndexes();
		foreach (var nodeChildIndex in nodeChildIndexes) {
			if (nodeChildIndex == nodeIndex) {
				return true;
			}
		}

		return false;
	}
	/*
	public static bool IsNodeAccessible (string nodeName) {
		int[] nodeChildIndexes = GetNodeChildIndexes();
		foreach (var nodeChildIndex in nodeChildIndexes) {
			if (GetNode(nodeChildIndex).name == nodeName) {
				return true;
			}
		}
		
		return false;
	}
	*/

	public static bool IsTreeAccessible (int treeIndex) {
		Tree tree = forest.trees[treeIndex];

		foreach (var treeInterval in tree.treeIntervals) {
			if (treeInterval.treeIndex != nodePointer.treeIndex) {
				continue;
			}

			int[] path = forest.QueryNodeConnection(treeInterval);
			foreach (var nodeIndex in path) {
				if (nodeIndex == nodePointer.nodeIndex) {
					return true;
				}
			}
		}

		return false;
	}

	public static bool JumpToNode (int nodeIndex) {
		if (!IsNodeAccessible(nodeIndex)) {
			return false;
		}

		nodePointer.nodeIndex = nodeIndex;
		return true;
	}

	public static bool JumpToTree (int treeIndex) {
		if (!IsTreeAccessible(treeIndex)) {
			return false;
		}

		nodePointerStack.Push(nodePointer);
		nodePointer = new CompoundIndex(treeIndex, Forest.entryTreeIndex);
		return true;
	}

	public static bool JumpBack () {
		if (nodePointerStack.Count == 0) {
			return false;
		}
		
		nodePointer = nodePointerStack.Pop();
		return true;
	}

	void OnDrawGizmos() {
		if (Forest.trees[selectedTreeIndex].deleted) {
			return;
		}

		DrawNodeConnections();

		DrawPaths();

		DrawNodes();
	}

	void DrawNodes () {
		// Nodes
		Gizmos.color = Color.cyan;
		if (Forest.trees[selectedTreeIndex].nodes[selectedNodeIndex].existing) {
			Gizmos.DrawWireSphere(Forest.trees[selectedTreeIndex].nodes[selectedNodeIndex].location.vector, 0.5f);
			Gizmos.DrawWireSphere(Forest.trees[selectedTreeIndex].nodes[selectedNodeIndex].location.vector, 0.3f);
		}
		
		Gizmos.color = Color.red;
		foreach (var node in Forest.trees[selectedTreeIndex].nodes) {
			if (node.deleted) {
				continue;
			}
			
			Gizmos.DrawWireSphere(node.location.vector, 0.4f);
		}	
	}

	void DrawNodeConnections () {
		// Node connections
		Gizmos.color = Color.blue;
		foreach (var node in Forest.trees[selectedTreeIndex].nodes) {
			if (node.deleted) {
				continue;
			}
			
			foreach (var childIndex in node.childIndexes) {
				Gizmos.DrawLine(node.location.vector, Forest.trees[selectedTreeIndex].nodes[childIndex].location.vector);
			}
		}
	}

	void DrawPaths () {
		// Alternative tree available
		Gizmos.color = Color.magenta;
		foreach (var tree in Forest.trees) {
			if (tree.deleted) {
				continue;
			}
			
			foreach (var treeInterval in tree.treeIntervals) {
				if (treeInterval.treeIndex == selectedTreeIndex) {
					int[] path = Forest.QueryNodeConnection(treeInterval);
					if (path.Length == 0) {
						Debug.LogWarning(
							"Invalide NodeConnection of Tree " + tree.index 
							+ " from Node " + treeInterval.startNodeIndex 
							+ " to Node " + treeInterval.endNodeIndex
							+ " in Tree " + treeInterval.treeIndex);
						continue;
					}

					int oldNodeIndex = path[0];
					for (int i = 1; i < path.Length; i++) {
						
						Vector3 start = Forest.trees[selectedTreeIndex].nodes[oldNodeIndex].location.vector;
						Vector3 end = Forest.trees[selectedTreeIndex].nodes[path[i]].location.vector;
						
						DrawPath(start, end, 0.5f);
						
						oldNodeIndex = path[i];
					}
				}
			}
		}
	}

	void DrawPath (Vector3 start, Vector3 end, float size) {
		for (float j = 0; j <= 1; j+=(0.2f / Vector3.Distance(start, end))) {
			Gizmos.DrawWireCube(Vector3.Lerp(start, end, j), Vector3.one * 0.2f);
		}
	}
}