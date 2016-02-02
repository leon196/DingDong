using UnityEngine;
using System.Collections;

public class WebcamManager : MonoBehaviour 
{
	public string webcamName = "";
	public Material material;
	public float treshold = 0.3f;
	public float fadeOutRatio = 0.95f;
	WebCamTexture textureWebcam;

	void Start () 
	{
		if (WebCamTexture.devices.Length > 0) {

			foreach (WebCamDevice device in WebCamTexture.devices) {
				webcamName = webcamName + device.name + '\n';
			}

			Debug.Log(webcamName);

			// Setup webcam texture
			textureWebcam = new WebCamTexture();
			Shader.SetGlobalTexture("_TextureWebcam", textureWebcam);
			textureWebcam.Play();
			if (material != null) {
				material.mainTexture = textureWebcam;
			}
		}
	}

	void Update ()
	{
		// Control luminance treshold
		if (Input.GetKey(KeyCode.R)) {
			treshold = Mathf.Clamp(treshold + 0.001f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.T)) { 
			treshold = Mathf.Clamp(treshold - 0.001f, 0f, 1f);
		}

		// Control fade out ratio
		if (Input.GetKey(KeyCode.F)) {
			fadeOutRatio = Mathf.Clamp(fadeOutRatio - 0.01f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.G)) { 
			fadeOutRatio = Mathf.Clamp(fadeOutRatio + 0.01f, 0f, 1f);
		}
	}
}