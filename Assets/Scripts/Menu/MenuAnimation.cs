using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuAnimation : MonoBehaviour {
	public Text caption;
	public float flashingInterval;
	Color originalCaptionColor;

	public Text divergence;


	void Start () {
		originalCaptionColor = caption.color;

		Story.LoadStory();
		divergence.text = Story.QueryDivergence().ToString();
	}

	void Update () {
		if ((((int)(Time.time / flashingInterval)) % 2) == 0) {
			caption.color = Color.clear;
		} else {
			caption.color = originalCaptionColor;
		}
	}
}
