using UnityEngine;
using System.Collections;

public class Playground : MonoBehaviour {

	public int intArray1;
	public int intArray2;

	void Start () {
		DataSL.SaveData<int>("test", intArray1);
		intArray2 = DataSL.LoadData<int>("test");
	}
}
