using UnityEngine;
using System.Collections;

/// <summary>
/// Load the GameRecord then enter the scene, initiate the game and Destroy itself
/// The only loader of the GameRecorder
/// Should only be placed in each scene, not in the menu scene.
/// </summary>
public class GameLoader : MonoBehaviour {

	void Start () {
		// If not menuScene, InitiateGame
		if (Application.loadedLevel != 0) {
			InitiateGame();
		}
	}

	/// <summary>
	/// Should ve called in the menu scene.
	/// </summary>
	public void LoadGame () {
		GameRecorder.LoadGameRecord();

		Application.LoadLevel(GameRecorder.sceneIndex);
		Debug.Log(GameRecorder.sceneIndex);
	}

	/// <summary>
	/// Should be called at the Start phase in each scene.
	/// </summary>
	public void InitiateGame () {
		GameRecorder.LoadGameRecord();

		Story.LoadStory();
		StoryObjectManager.RefreshStoryObjects(Story.GetNodePointer());

		WeaponInventory.LoadInventoryStates();
		//ItemInventory.LoadInventoryStates();

		CharacterManager.InitiatePlayer(GameRecorder.playerIndex, GameRecorder.playerPosition);
	}
}
