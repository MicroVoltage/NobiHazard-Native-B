using UnityEngine;
using System.Collections;

[System.Serializable]
public class Holdable {
	public string name;
	public int index;

	public string description;
}

public enum ItemType {
	Key,
	Herb,
	EventItem
}

[System.Serializable]
public class Item : Holdable {
	public ItemType itemType;
}

[System.Serializable]
public class Key : Item {
	new public const ItemType itemType = ItemType.Key;

	public int keyIndex;
}

[System.Serializable]
public class Herb : Item {
	new public const ItemType itemType = ItemType.Herb;

	public float healHealth;
}

[System.Serializable]
public class EventItem : Item {
	new public const ItemType itemType = ItemType.EventItem;

	public GameObject itemEvent;
}