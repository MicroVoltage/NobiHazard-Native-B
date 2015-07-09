using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Data Saver & Loader that interface the PlayerPrefs.
/// </summary>
public class DataSL {
	const string arrayLengthAffix = "Length";
	const string boolArrayAffix = "Bools";

	const string floatTypeString = "System.Single";
	const string intTypeString = "System.Int32";
	const string stringTypeString = "System.String";
	const string boolTypeString = "System.Boolean";

	public static void SaveArray <T> (string arrayName, T[] array) {
		if (PlayerPrefs.HasKey(arrayName + arrayLengthAffix)) {
			for (int i=0; i<PlayerPrefs.GetInt(arrayName+arrayLengthAffix); i++) {
				if (PlayerPrefs.HasKey(arrayName + i)) {
					PlayerPrefs.DeleteKey(arrayName + i);
				}
			}

			PlayerPrefs.DeleteKey(arrayName + boolArrayAffix);
		}

		PlayerPrefs.SetInt(arrayName + arrayLengthAffix, array.Length);

		if (typeof(T) == typeof(bool)) {
			string boolArray = "";
			for (int i=0; i<array.Length; i++) {
				if ((bool)(object)array[i]) {
					boolArray += "1";
				} else {
					boolArray += "0";
				}
			}
			PlayerPrefs.SetString(arrayName + boolArrayAffix, boolArray);
			return;
		}

		for (int i=0; i<array.Length; i++) {
			string keyName = arrayName + i;

			switch (typeof(T).ToString()) {
			case floatTypeString:
				if ((float)(object)array[i] != 0.0f) {
					PlayerPrefs.SetFloat(keyName, (float)(object)array[i]);
				}
				break;
			case intTypeString:
				if ((int)(object)array[i] != 0) {
					PlayerPrefs.SetInt(keyName, (int)(object)array[i]);
				}
				break;
			case stringTypeString:
				if ((string)(object)array[i] != "") {
					PlayerPrefs.SetString(keyName, (string)(object)array[i]);
				}
				break;
			default:
				Debug.LogError("Trying to save unsupported type: " + typeof(T));
				break;
			}
		}
	}

	public static T[] LoadArray <T> (string arrayName) {
 		if (!PlayerPrefs.HasKey(arrayName + arrayLengthAffix)) {
			Debug.LogError("Trying to access non-exist array: " + arrayName);

			return null;
		}

		T[] array = new T[PlayerPrefs.GetInt(arrayName + arrayLengthAffix)];

		if (typeof(T) == typeof(bool)) {
			string boolArray = PlayerPrefs.GetString(arrayName + boolArrayAffix);
			for (int i=0; i<boolArray.Length; i++) {
				Debug.Log(boolArray[i]);
				if (boolArray[i] + "" == "1") {
					array[i] = (T)(object)true;
				} else {
					array[i] = (T)(object)false;
				}
			}
			return array;
		}

		for (int i=0; i<array.Length; i++) {
			string keyName = arrayName + i;

			switch (typeof(T).ToString()) {
			case floatTypeString:
				if (PlayerPrefs.HasKey(keyName)) {
					array[i] = (T)(object)PlayerPrefs.GetFloat(keyName);
				} else {
					array[i] = (T)(object)0.0f;
				}
				break;
			case intTypeString:
				if (PlayerPrefs.HasKey(keyName)) {
					array[i] = (T)(object)PlayerPrefs.GetInt(keyName);
				} else {
					array[i] = (T)(object)0;
				}
				break;
			case stringTypeString:
				if (PlayerPrefs.HasKey(keyName)) {
					array[i] = (T)(object)PlayerPrefs.GetString(keyName);
				} else {
					array[i] = (T)(object)"";
				}
				break;
			default:
				Debug.LogError("Trying to save unsupported type: " + typeof(T));
				break;
			}
		}

		return array;
	}

	public static void SaveData <T> (string dataName, T data) {
		switch (typeof(T).ToString()) {
		case floatTypeString:
			PlayerPrefs.SetFloat(dataName, (float)(object)data);
			break;
		case intTypeString:
			PlayerPrefs.SetInt(dataName, (int)(object)data);
			break;
		case stringTypeString:
			PlayerPrefs.SetString(dataName, (string)(object)data);
			break;
		case boolTypeString:
			BoolKey.SaveBool(dataName, (bool)(object)data);
			break;
		default:
			Debug.LogError("Trying to save unsupported type: " + typeof(T));
			break;
		}
	}

	public static T LoadData <T> (string dataName) {
		switch (typeof(T).ToString()) {
		case floatTypeString:
			return (T)(object)PlayerPrefs.GetFloat(dataName);
		case intTypeString:
			return (T)(object)PlayerPrefs.GetInt(dataName);
		case stringTypeString:
			return (T)(object)PlayerPrefs.GetString(dataName);
		case boolTypeString:
			return (T)(object)BoolKey.LoadBool(dataName);
		default:
			Debug.LogError("Trying to load unsupported type: " + typeof(T));
			return default(T);
		}
	}
}

/// <summary>
/// Capsulate the implimentation of using a StringKey to store all the BoolData.
/// "GameStarted1GameEnd0PlayingGame1"
/// </summary>
class BoolKey {
	const string keyName = "Bools";
	static string keyValue;


	public static bool LoadBool (string name) {
		keyValue = PlayerPrefs.GetString(keyName);

		if (keyValue.Contains(name)) {
			return (keyValue.Substring(keyValue.IndexOf(name) + name.Length, 1) != "0");
		}

		Debug.LogError("Trying to access a non-exist bool: " + name);
		return false;
	}

	public static void SaveBool (string name, bool value) {
		keyValue = PlayerPrefs.GetString(keyName);

		string boolIndicator = "0";
		if (value) {
			boolIndicator = "1";
		}

		if (keyValue.Contains(name)) {
			Debug.Log (keyValue);
			keyValue.Remove(keyValue.IndexOf(name) + name.Length, 1);
			Debug.Log (keyValue);
			Debug.Log (keyValue.Substring(keyValue.IndexOf(name) + name.Length, 1));
			keyValue.Insert(keyValue.IndexOf(name) + name.Length, boolIndicator);
		} else {
			keyValue += name + boolIndicator;
		}
		PlayerPrefs.SetString(keyName, keyValue);
	}
}
