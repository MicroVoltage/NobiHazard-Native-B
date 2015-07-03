using UnityEngine;
using System.Collections;

public class ItemEditor : MonoBehaviour {
	public static ItemEditor instance;

	public Item[] items;

	void Awake () {
		instance = this;
	}
}
