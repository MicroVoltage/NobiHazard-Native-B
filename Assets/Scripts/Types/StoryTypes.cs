using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Node {
	public string name;
	public int index;
	public int[] childIndexes;

	public NodeLocation location;

	public bool deleted;
	public bool existing {
		set {
			deleted = !value;
		}
		get {
			return !deleted;
		}
	}


	public Node (int newIndex) {
		name = "";
		index = newIndex;
		deleted = false;
		childIndexes = new int[0];

		location = new NodeLocation(-1, -1);
	}
}

[System.Serializable]
public class NodeLocation {
	public int depth;
	public int x;

	public Vector3 vector {
		get {
			return new Vector3(x, depth);
		}
	}

	public NodeLocation (int newDepth, int newX) {
		depth = newDepth;
		x = newX;
	}
}

[System.Serializable]
public class Tree {
	public string name;
	public int index;
	public NodeConnection[] treeIntervals;
	public Node[] nodes;

	public bool deleted;
	public bool existing {
		set {
			deleted = !value;
		}
		get {
			return !deleted;
		}
	}


	public Tree (int treeIndex) {
		InitiateTree(treeIndex);
	}

	public void InitiateTree (int treeIndex) {
		index = treeIndex;
		treeIntervals = new NodeConnection[0];
		nodes = new Node[1];
		nodes[0] = new Node(0);

		deleted = false;

		LocateNodes(0);
	}

	public int InsertNode (int nodeIndex) {
		int newNodeIndex = AddNode();

		nodes[newNodeIndex].childIndexes = (int[])nodes[nodeIndex].childIndexes.Clone();
		SetInt(ref nodes[nodeIndex].childIndexes, newNodeIndex);

		return newNodeIndex;
	}

	public int AppendNode (int nodeIndex) {
		int newNodeIndex = AddNode();
		
		AddInt(ref nodes[nodeIndex].childIndexes, newNodeIndex);
		
		return newNodeIndex;
	}

	void AddInt (ref int[] intArray, int newInt) {
		Array.Resize<int>(ref intArray, intArray.Length+1);
		intArray[intArray.Length - 1] = newInt;
	}
	
	void SetInt (ref int[] intArray, int newInt) {
		intArray = new int[1];
		intArray[0] = newInt;
	}

	public int RemoveNode (int nodeIndex) {
		int[] parentIndexes = FindAllParentIndexes(nodeIndex);
		if (parentIndexes.Length == 0) {
			return 0;
		}

		foreach (var parentIndex in parentIndexes) {
			RemoveInt(ref nodes[parentIndex].childIndexes, nodeIndex);
			foreach (var childIndex in nodes[nodeIndex].childIndexes) {
				AddInt(ref nodes[parentIndex].childIndexes, childIndex);
			}
		}

		DeleteNode(nodeIndex);

		return parentIndexes[0];
	}

	void RemoveInt (ref int[] intArray, int oldInt) {
		int[] oldIntArray = (int[])intArray.Clone();
		intArray = new int[0];

		foreach (var value in oldIntArray) {
			if (value != oldInt) {
				AddInt(ref intArray, value);
			}
		}
	}
	
	/// <summary>
	/// Insert a node to a vacant index or Adds a node to the end;
	/// Return newNode's index.
	/// </summary>
	int AddNode () {
		int index;
		for (index = 0; index < nodes.Length; index++) {
			if (nodes[index].deleted) {
				nodes[index] = new Node(index);

				return index;
			}
		}

		index = nodes.Length;
		Array.Resize<Node>(ref nodes, index + 1);
		nodes[index] = new Node(index);

		return index;
	}

	void DeleteNode (int nodeIndex) {
		nodes[nodeIndex].deleted = true;;
	}

	public int FindNode (int depth, int x) {
		foreach (var node in nodes) {
			if (node.existing && node.location.depth == depth && node.location.x == x) {
				return Array.IndexOf(nodes, node);
			}
		}
		return -1;
	}

	public int FindParentIndex (int nodeIndex) {
		foreach (var node in nodes) {
			if (node.existing) {
				foreach (var childIndex in node.childIndexes) {
					if (childIndex == nodeIndex) {
						return Array.IndexOf(nodes, node);
					}
				}
			}
		}
		return -1;
	}

	public int[] FindAllParentIndexes (int nodeIndex) {
		List<int> parents = new List<int>();
		foreach (var node in nodes) {
			if (node.existing) {
				foreach (var childIndex in node.childIndexes) {
					if (childIndex == nodeIndex) {
						parents.Add(Array.IndexOf(nodes, node));
					}
				}
			}
		}

		return parents.ToArray();
	}

	/// <summary>
	/// Assign location to each node from the startNode.
	/// </summary>
	public void LocateNodes (int startNodeIndex) {
		x = 0;

		LocateNode(startNodeIndex, 0);
	}
	int x;
	void LocateNode (int nodeIndex, int depth) {
		Node node = nodes[nodeIndex];

		node.location = new NodeLocation(depth, x);
		
		for (int i = 0; i < node.childIndexes.Length; i++) {
			if (i > 0) {
				x++;
			}
			LocateNode(node.childIndexes[i], depth + 1);

		}


	}

	/// <summary>
	/// Find one posible path between two node.
	/// </summary>
	public int[] QueryNodeConnection (int startNodeIndex, int endNodeIndex) {
		Stack<int> path = new Stack<int>();
		int nodeTraverser = endNodeIndex;

		while (nodeTraverser != -1) {
			path.Push(nodeTraverser);
			if (path.Peek() == startNodeIndex) {
				int[] pathArray = path.ToArray();
				Array.Reverse(pathArray);
				return pathArray;
			}
			nodeTraverser = FindParentIndex(nodeTraverser);
		}
		return new int[0];
	}
}

[System.Serializable]
public class NodeConnection {
	public int treeIndex;
	public int startNodeIndex;
	public int endNodeIndex;

	public NodeConnection (int newTreeIndex, int newStartNodeIndex, int newEndNodeIndex) {
		treeIndex = newTreeIndex;
		startNodeIndex = newStartNodeIndex;
		endNodeIndex = newEndNodeIndex;
	}
}

[System.Serializable]
public class Forest {
	public Tree[] trees;
	public const int entryTreeIndex = 0;
	public int entryNodeIndex;

	public Forest () {
		trees = new Tree[1];
		trees[0] = new Tree(0);
	}

	public int NewTree () {
		int index;
		for (index = 0; index < trees.Length; index++) {
			if (trees[index].deleted) {
				trees[index] = new Tree(index);
				
				return index;
			}
		}
		
		index = trees.Length;
		Array.Resize<Tree>(ref trees, index + 1);
		trees[index] = new Tree(index);
		
		return index;
	}

	public void DeleteTree (int treeIndex) {
		if (treeIndex == 0) {
			return;
		}
		trees[treeIndex].deleted = true;
	}

	public Node GetNode (CompoundIndex index) {
		return GetNode(index.treeIndex, index.nodeIndex);
	}
	public Node GetNode (int treeIndex, int nodeIndex) {
 		return trees[treeIndex].nodes[nodeIndex];
	}

	public int[] QueryNodeConnection (NodeConnection nodeConnection) {
		return trees[nodeConnection.treeIndex].QueryNodeConnection(nodeConnection.startNodeIndex, nodeConnection.endNodeIndex);
	}
}

[System.Serializable]
public class CompoundIndex {
	public int treeIndex;
	public int nodeIndex;

	public CompoundIndex (int newTreeIndex, int newNodeIndex) {
		treeIndex = newTreeIndex;
		nodeIndex = newNodeIndex;
	}
}