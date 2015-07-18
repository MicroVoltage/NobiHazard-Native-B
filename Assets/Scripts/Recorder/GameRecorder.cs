using UnityEngine;
using System.Collections;

/// <summary>
/// Record game critical data using DataSL;
/// Full of static things so do not need to be attached to any GameObject;
/// Intended to load data once in menuScene and save data during the game.
/// </summary>
public class GameRecorder {
	public const string initializedFlagKey = "GameRecorderInitialized";

	public const string sceneIndexKey = "sceneIndex";
	public const string playingTimeKey = "playingTime";
	public const string killCountKey = "killCount";

	public const string playerIndexKey = "playerIndex";
	public const string playerPositionKeyX = "playerPositionX";
	public const string playerPositionKeyY = "playerPositionY";
	public const string playerPositionKeyZ = "playerPositionZ";

	public static int sceneIndex;
	public static float playingTime;
	public static int killCount;

	public static int playerIndex;
	public static Vector3 playerPosition;

	public static void SaveGameRecord () {
		DataSL.SaveData<int>(sceneIndexKey, sceneIndex);
		DataSL.SaveData<float>(playingTimeKey, playingTime);
		DataSL.SaveData<int>(killCountKey, killCount);

		DataSL.SaveData<int>(playerIndexKey, playerIndex);
		DataSL.SaveData<float>(playerPositionKeyX, playerPosition.x);
		DataSL.SaveData<float>(playerPositionKeyX, playerPosition.y);
		DataSL.SaveData<float>(playerPositionKeyX, playerPosition.z);

		DataSL.SaveData<bool>(initializedFlagKey, true);
	}

	public static void LoadGameRecord () {
		if (!DataSL.LoadData<bool>(initializedFlagKey)) {
			SaveGameRecord();
			return;
		}

		sceneIndex = DataSL.LoadData<int>(sceneIndexKey);
		playingTime = DataSL.LoadData<float>(playingTimeKey);
		killCount = DataSL.LoadData<int>(killCountKey);
		
		playerIndex = DataSL.LoadData<int>(playerIndexKey);
		playerPosition = new Vector3(
			DataSL.LoadData<float>(playerPositionKeyX),
			DataSL.LoadData<float>(playerPositionKeyY),
			DataSL.LoadData<float>(playerPositionKeyZ));
	}
}
