using UnityEngine;
using System.Collections;
using System.Xml;

public class XmlDataSL {
	public static string xmlPath;

	public static void SaveObjects <T> (T[] objects) {
		InitiateXml();
	}

	static void InitiateXml () {
		xmlPath = Application.persistentDataPath;
	}
}
