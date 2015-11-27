using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour 
{
	Webcamer webcamer;
	Choupichoup choupichoup;
	GUIText[] labelArray;

	void Start () 
	{
		webcamer = GetComponent<Webcamer>();
		
		choupichoup = new Choupichoup(webcamer.textureWebcam.width, webcamer.textureWebcam.height);
		choupichoup.Spawn();

		webcamer.textureCollider = choupichoup.texture;
		labelArray = GetComponentsInChildren<GUIText>();
		foreach (GUIText label in labelArray) {
			label.text = "";
			// label.enabled = false;
		}
		UpdateText();

	}
	
	void Update () 
	{
		choupichoup.Update();

		// Toggle labels
		if (Input.GetKeyDown(KeyCode.Space)) {
			// foreach (GUIText label in labelArray) {
			// 	label.enabled = !label.enabled;
			// }
		}

		// Control luminance treshold
		if (Input.GetKey(KeyCode.R)) {
			webcamer.treshold = Mathf.Clamp(webcamer.treshold + 0.001f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.T)) { 
			webcamer.treshold = Mathf.Clamp(webcamer.treshold - 0.001f, 0f, 1f);
		}

		// Control fade out ratio
		if (Input.GetKey(KeyCode.F)) {
			webcamer.fadeOutRatio = Mathf.Clamp(webcamer.fadeOutRatio - 0.01f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.G)) { 
			webcamer.fadeOutRatio = Mathf.Clamp(webcamer.fadeOutRatio + 0.01f, 0f, 1f);
		}

		// Control choupichoup radius
		if (Input.GetKeyDown(KeyCode.V)) {
			choupichoup.choup.radius = Mathf.Clamp(choupichoup.choup.radius - 1f, 0f, 100f);
		} else if (Input.GetKeyDown(KeyCode.B)) { 
			choupichoup.choup.radius = Mathf.Clamp(choupichoup.choup.radius + 1f, 0f, 100f);
		}

		if (Input.anyKey) {
			UpdateText();
		}
	}

	void UpdateText ()
	{
		foreach (GUIText label in labelArray) {
			label.text = webcamer.webcamName 
				+ "luminance treshold (R/T) : " + webcamer.treshold + '\n'
				+ "fade out ratio (F/G) : " + webcamer.fadeOutRatio + '\n'
				+ "choupichoup radius (V/B) : " + choupichoup.choup.radius + '\n';
		}
	}

	public void WebcamCollision ()
	{
		choupichoup.Spawn();
	}
}
