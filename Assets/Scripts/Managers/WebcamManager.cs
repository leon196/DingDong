using UnityEngine;
using System.Collections;

public class WebcamManager : MonoBehaviour 
{
	public string webcamName = "";
	public float treshold = 0.1f;
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

		if (Input.anyKey) {
			UpdateUniforms();
		}
	}

	void UpdateUniforms () {
		Shader.SetGlobalFloat("_TresholdMotion", treshold);
		Shader.SetGlobalFloat("_FadeOutRatio", fadeOutRatio);
	}
}