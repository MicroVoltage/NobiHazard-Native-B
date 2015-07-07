using UnityEngine;
using System.Collections;

/// <summary>
/// Holds informations and behaviors of items.
/// </summary>
public class ItemEditor : MonoBehaviour {
	public const int itemCount = 3;

	/// <summary>
	/// Generate automatically at run-time.
	/// </summary>
	public static Item[] items;

	public static Key[] keys;
	public static Herb[] herbs;
	public static EventItem[] eventItems;

	public Key[] Keys;
	public Herb[] Herbs;
	public EventItem[] EventItems;

	void Awake () {
		StatizeObjects();

		GenerateItems();
	}

	void StatizeObjects () {
		keys = Keys;
		herbs = Herbs;
		eventItems = EventItems;
	}

	void GenerateItems () {
		items = new Item[keys.Length + herbs.Length + eventItems.Length];
		int itemIndex = 0;

		foreach (var key in keys) {
			key.index = itemIndex;
			items[itemIndex] = key;

			itemIndex ++;
		}

		foreach (var herb in herbs) {
			herb.index = itemIndex;
			items[itemIndex] = herb;
			
			itemIndex ++;
		}

		foreach (var eventItem in eventItems) {
			eventItem.index = itemIndex;
			items[itemIndex] = eventItem;
			
			itemIndex ++;
		}
	}
}
