using UnityEngine;
using System.Collections;

public class Webcamer : MonoBehaviour 
{
	WebCamTexture textureWebcam;
	Texture2D textureDifference;
	Color[] colorArray;
	Color[] colorBufferArray;

	void Start () 
	{
		foreach (WebCamDevice device in WebCamTexture.devices) {
			Debug.Log(device.name);
		}
		if (WebCamTexture.devices.Length > 0) {

			// Setup webcam texture
			textureWebcam = new WebCamTexture();
			GetComponent<Renderer>().material.mainTexture = textureWebcam;
			Shader.SetGlobalTexture("_TextureWebcam", textureWebcam);
			textureWebcam.Play();

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

	void LateUpdate ()
	{
		Color[] colorPixelArray = textureWebcam.GetPixels();
		for (int i = 0; i < colorArray.Length; ++i) {
			Color currentColor = colorArray[i];
			Color newColor = colorPixelArray[i];
			Color bufferColor = colorBufferArray[i];
			// currentColor.r *= 0.99f;
			float lum = newColor.r - bufferColor.r;
			lum = lum < 0.1f ? currentColor.r * 0.99f : newColor.r;//Mathf.Max(currentColor.r, newColor.r - bufferColor.r);
			colorArray[i] = new Color(lum, lum, lum, 1f);
			colorBufferArray[i] = newColor;
		}
		textureDifference.SetPixels(colorArray);
		textureDifference.Apply(false);
	}
}