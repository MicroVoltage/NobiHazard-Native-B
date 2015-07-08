using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Node {
	public string name;
	public int index;

	public NodeLocation location;

	public int[] childIndexes;

	public Node (int newIndex) {
		name = "";
		index = newIndex;
		location = new NodeLocation(-1, -1);
		childIndexes = new int[0];
	}
}

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

public class Tree {
	public string name;
	public Node[] nodes;

	public Tree () {
		InitiateTree();
	}

	public void InitiateTree () {
		nodes = new Node[1];
		nodes[0] = new Node(0);
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
			if (nodes[index] == null) {
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
		nodes[nodeIndex] = null;
	}

	public int FindNode (int depth, int x) {
		foreach (var node in nodes) {
			if (node != null && node.location.depth == depth && node.location.x == x) {
				return Array.IndexOf(nodes, node);
			}
		}
		return -1;
	}

	public int FindParentIndex (int nodeIndex) {
		foreach (var node in nodes) {
			if (node != null) {
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
			if (node != null) {
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
			LocateNode(node.childIndexes[i], depth + 1);
			if (node.childIndexes.Length > 1) {
				x ++;
			}
		}
	}
}