using UnityEngine;
using System.Collections;

[RequireComponent (typeof (FrameBuffer))]
public class MotionManager : MonoBehaviour 
{
	FrameBuffer frameBuffer;
	InterfaceManager interfaceManager;

	Texture2D texture2D;
	RenderTexture renderTexture;
	Rect textureRect;

	Color[] colorArray;
	Vector2 target;
	Vector2 position;
	float distanceTreshold;

	void Start ()
	{
		interfaceManager = GameObject.FindObjectOfType<InterfaceManager>();
		frameBuffer = GetComponent<FrameBuffer>();
		renderTexture = frameBuffer.GetCurrentTexture();

		textureRect = new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height);
		texture2D = new Texture2D((int)textureRect.width, (int)textureRect.height);
		colorArray = new Color[(int)textureRect.width * (int)textureRect.height];
		Shader.SetGlobalTexture("_TestTexture", texture2D);	

		position = Vector2.zero;

		target = Vector2.one * 0.5f;
		target.x *= textureRect.width;
		target.y *= textureRect.height;
		distanceTreshold = 0.1f * textureRect.height;
	}

	void Update () 
	{
		renderTexture = frameBuffer.GetCurrentTexture();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(textureRect, 0, 0, false);
		texture2D.Apply(false);

		bool hit = false;
		int index = 0;

		colorArray = texture2D.GetPixels();
		position = Vector2.zero;
		int r = Random.Range(0, colorArray.Length);
		foreach (Color color in colorArray) {
			position.x = (index % textureRect.width);
			position.y = Mathf.Floor(index / textureRect.width);
			if (color.r == 1f && Vector2.Distance(position, target) < distanceTreshold) {
				hit = true;
				break;
			}
			++index;
		}

		if (hit) {
			interfaceManager.EnableCircle();
		} else {
			interfaceManager.DisableCircle();
		}
	}
}