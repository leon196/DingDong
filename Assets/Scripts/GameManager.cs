using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	WebcamManager webcam;
	// Choupichoup choupichoup;
	GUIText[] labelArray;

	void Start () 
	{
		webcam = GetComponent<WebcamManager>();
		
		// choupichoup = new Choupichoup(webcam.textureWebcam.width, webcam.textureWebcam.height);
		// choupichoup.Spawn();

		// webcam.textureCollider = choupichoup.texture;
		labelArray = GetComponentsInChildren<GUIText>();
		foreach (GUIText label in labelArray) {
			label.text = "";
			// label.enabled = false;
		}
		UpdateText();

	}
	
	void Update () 
	{
		// choupichoup.Update();

		// Toggle labels
		if (Input.GetKeyDown(KeyCode.Space)) {
			// foreach (GUIText label in labelArray) {
			// 	label.enabled = !label.enabled;
			// }
		}

		// Control choupichoup radius
		if (Input.GetKeyDown(KeyCode.V)) {
			// choupichoup.choup.radius = Mathf.Clamp(choupichoup.choup.radius - 1f, 0f, 100f);
		} else if (Input.GetKeyDown(KeyCode.B)) { 
			// choupichoup.choup.radius = Mathf.Clamp(choupichoup.choup.radius + 1f, 0f, 100f);
		}

		if (Input.anyKey) {
			UpdateText();
		}
	}

	void UpdateText ()
	{
		foreach (GUIText label in labelArray) {
			label.text = webcam.webcamName 
				+ "luminance treshold (R/T) : " + webcam.treshold + '\n'
				+ "fade out ratio (F/G) : " + webcam.fadeOutRatio + '\n';
				// + "choupichoup radius (V/B) : " + choupichoup.choup.radius + '\n';
		}
	}

	public void WebcamCollision ()
	{
		// choupichoup.Spawn();
	}
}
