using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {
	public GameObject[] Characters;

	static GameObject playerInstance;

	static GameObject[] characters;

	void Awake () {
		characters = Characters;
	}

	public static void InitiatePlayer (int playerIndex, Vector3 playerPosition) {
		playerInstance = (GameObject)Instantiate(characters[playerIndex], playerPosition, Quaternion.identity);
	}

	public static GameObject GetPlayerInstace () {
		return playerInstance;
	}
}