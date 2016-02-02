using UnityEngine;
using System.Collections;

public class WebcamManager : MonoBehaviour 
{
	public string webcamName = "";
	public Material material;
	public float treshold = 0.3f;
	public float fadeOutRatio = 0.95f;
	WebCamTexture textureWebcam;
	Texture2D textureDifference;
	Texture2D textureCollider;
	Color[] colorArray;
	Color[] colorBufferArray;

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

			// Setup color array
			colorArray = new Color[textureWebcam.width * textureWebcam.height];
			colorBufferArray = new Color[textureWebcam.width * textureWebcam.height];
			for (int i = 0; i < colorArray.Length; ++i) {
				colorArray[i] = Color.black;
				colorBufferArray[i] = Color.black;
			}

			// Setup procedural texture
			textureDifference = new Texture2D(textureWebcam.width, textureWebcam.height, TextureFormat.ARGB32, false);
			textureDifference.SetPixels(colorArray);
			textureDifference.Apply(false);
			Shader.SetGlobalTexture("_TextureDifference", textureDifference);
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

		if (textureWebcam) {
			Color[] colorPixelArray = textureWebcam.GetPixels();
			// Color[] colorColliderArray = textureCollider.GetPixels();
			for (int i = 0; i < colorArray.Length; ++i) {
				Color currentColor = colorArray[i];
				Color newColor = colorPixelArray[i];
				Color bufferColor = colorBufferArray[i];
				// currentColor.r *= 0.99f;
				float lumCurrent = (currentColor.r + currentColor.g + currentColor.b) / 3.0f;
				float lumNew = (newColor.r + newColor.g + newColor.b) / 3.0f;
				float lumBuffer = (bufferColor.r + bufferColor.g + bufferColor.b) / 3.0f;
				float lum = Mathf.Abs(lumNew - lumBuffer);

				if (lum < treshold) {
					lum = lumCurrent * fadeOutRatio;
				} else {
					lum = 1f;
					// Color colliderColor = colorColliderArray[i];
					// if (colliderColor != Color.black) {
					// 	main.WebcamCollision();
					// }
				}

				colorArray[i] = new Color(lum, lum, lum, 1f);
				colorBufferArray[i] = newColor;
			}
			textureDifference.SetPixels(colorArray);
			textureDifference.Apply(false);
		}
	}
}