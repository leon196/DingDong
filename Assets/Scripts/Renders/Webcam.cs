using UnityEngine;
using System.Collections;

public class Webcam : MonoBehaviour 
{
	public string webcamName = "";
	public float treshold = 0.1f;
	public float fadeOutRatio = 0.95f;
	WebCamTexture textureWebcam;
	int currentWebcam;

	void Start () 
	{
		if (WebCamTexture.devices.Length > 0) {

			foreach (WebCamDevice device in WebCamTexture.devices) {
				webcamName = webcamName + device.name + '\n';
			}

			// Setup webcam texture
			textureWebcam = new WebCamTexture();
			Shader.SetGlobalTexture("_TextureWebcam", textureWebcam);
			textureWebcam.Play();

			currentWebcam = 0;
		}

		UpdateUniforms();
	}

	void Update ()
	{
		// Control luminance treshold
		if (Input.GetKey(KeyCode.RightArrow)) {
			treshold = Mathf.Clamp(treshold + 0.001f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.LeftArrow)) { 
			treshold = Mathf.Clamp(treshold - 0.001f, 0f, 1f);
		}

		// Control fade out ratio
		if (Input.GetKey(KeyCode.DownArrow)) {
			fadeOutRatio = Mathf.Clamp(fadeOutRatio - 0.001f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.UpArrow)) { 
			fadeOutRatio = Mathf.Clamp(fadeOutRatio + 0.001f, 0f, 1f);
		}

		// Switch camera
		if (Input.GetKeyDown(KeyCode.C)) {
			if (WebCamTexture.devices.Length > 1) {
				currentWebcam = (currentWebcam + 1) % WebCamTexture.devices.Length;
				textureWebcam.Stop();
				textureWebcam = new WebCamTexture(WebCamTexture.devices[currentWebcam].name);
				Shader.SetGlobalTexture("_TextureWebcam", textureWebcam);
				textureWebcam.Play();
			}
		}

		if (Input.anyKey) {
			UpdateUniforms();
		}
	}

	void UpdateUniforms () {
		Shader.SetGlobalFloat("_TresholdMotion", treshold);
		Shader.SetGlobalFloat("_FadeOutRatio", fadeOutRatio);
	}
}