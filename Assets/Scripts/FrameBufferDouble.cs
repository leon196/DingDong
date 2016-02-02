using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class FrameBufferDouble : MonoBehaviour
{
	public string textureName = "_FrameBuffer";
	public float pixelSize = 1f;
	int depth = 3;
	Camera cameraCapture;
	int currentTexture;
	RenderTexture[] textures;

	void Start ()
	{
		currentTexture = 0;
		textures = new RenderTexture[depth];
		CreateTextures();
		cameraCapture = GetComponent<Camera>();
	}

	void Update ()
	{
		Shader.SetGlobalTexture(textureName, GetCurrentTexture());
		Shader.SetGlobalTexture(textureName + "Last", GetLastTexture());
		NextTexture();
		cameraCapture.targetTexture = GetCurrentTexture();
	}

	void NextTexture ()
	{
		currentTexture = (currentTexture + 1) % depth;
	}

	RenderTexture GetCurrentTexture ()
	{
		return textures[currentTexture];
	}

	RenderTexture GetLastTexture ()
	{
		return textures[(currentTexture + 1) % depth];
	}

	void CreateTextures ()
	{
		int width = (int)(Screen.width * (1f / pixelSize));
		int height = (int)(Screen.height * (1f / pixelSize));

		for (int i = 0; i < textures.Length; ++i) {
			if (textures[i]) {
				textures[i].Release();
			}
			textures[i] = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
			textures[i].Create();
			textures[i].filterMode = FilterMode.Point;
		}
	}
}