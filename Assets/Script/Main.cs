using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
	GUIText[] labelArray;
	Webcamer webcamer;

	void Start () 
	{
		webcamer = GetComponent<Webcamer>();
		labelArray = GetComponentsInChildren<GUIText>();
		foreach (GUIText label in labelArray) {
			label.text = "";
			// label.enabled = false;
		}
		UpdateText();
	}
	
	void Update () 
	{
		// Toggle labels
		if (Input.GetKeyDown(KeyCode.Space)) {
			foreach (GUIText label in labelArray) {
				label.enabled = !label.enabled;
			}
		}

		// Control luminance treshold
		if (Input.GetKey(KeyCode.UpArrow)) {
			webcamer.treshold = Mathf.Clamp(webcamer.treshold + 0.001f, 0f, 1f);
			UpdateText();
		} else if (Input.GetKey(KeyCode.DownArrow)) { 
			webcamer.treshold = Mathf.Clamp(webcamer.treshold - 0.001f, 0f, 1f);
			UpdateText();
		}
	}

	void UpdateText()
	{
		foreach (GUIText label in labelArray) {
			label.text = webcamer.webcamName 
				+ "luminance treshold : " + webcamer.treshold;
		}
	}
}
