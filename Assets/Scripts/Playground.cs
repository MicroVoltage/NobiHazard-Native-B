using UnityEngine;
using System.Collections;

public class Playground : MonoBehaviour {

	public bool intArray1;
	public bool intArray2;

	void Start () {
		DataSL.SaveData<bool>(intArray1, "test");
		intArray2 = DataSL.LoadData<bool>("test");
	}
}
