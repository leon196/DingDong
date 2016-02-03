using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Camera))]
public class FrameBuffer : MonoBehaviour
{
	public string textureName = "_FrameBuffer";
	Camera cameraCapture;
	int currentTexture;
	RenderTexture[] textures;

	void Awake ()
	{
		currentTexture = 0;
		textures = new RenderTexture[3];
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

	void CreateTextures ()
	{
		for (int i = 0; i < textures.Length; ++i) {
			if (textures[i]) {
				textures[i].Release();
			}
			textures[i] = new RenderTexture((int)GameManager.width, (int)GameManager.height, 24, RenderTextureFormat.ARGB32);
			textures[i].Create();
			textures[i].filterMode = FilterMode.Point;
		}
	}

	void NextTexture ()
	{
		currentTexture = (currentTexture + 1) % 3;
	}

	public RenderTexture GetCurrentTexture ()
	{
		return textures[currentTexture];
	}

	public RenderTexture GetLastTexture ()
	{
		return textures[(currentTexture + 1) % 3];
	}

	public void UpdateResolution ()
	{
		CreateTextures();
	}
}