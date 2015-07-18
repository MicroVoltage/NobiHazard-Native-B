using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interfacing Story to provide high-level abstraction.
/// </summary>
public class StoryObjectManager : MonoBehaviour {
	public static List<StoryObjectHolder> storyObjects;

	void Start () {
		Story.StoryChangeEvent += new StoryChangeEventHandler(RefreshStoryObjects);
	}

	public static void Register (GameObject storyObject, NodeConnection[] activeIntervals) {
		storyObjects.Add(new StoryObjectHolder(storyObject, activeIntervals));

		RefreshStoryObjects(Story.GetNodePointer());
	}

	public static void RefreshStoryObjects (CompoundIndex nodePointer) {
		foreach (var storyObject in storyObjects) {
			foreach (var activeInterval in storyObject.activeIntervals) {
				if (Story.IsNodeBetweenConnection(nodePointer, activeInterval)) {
					storyObject.storyObject.SetActive(true);
				} else {
					storyObject.storyObject.SetActive(false);
				}
			}
		}
	}
}

public class StoryObjectHolder {
	public GameObject storyObject;
	public NodeConnection[] activeIntervals;

	public StoryObjectHolder (GameObject newStoryObejct, NodeConnection[] newActiveIntervals) {
		storyObject = newStoryObejct;
		activeIntervals = newActiveIntervals;
	}
}