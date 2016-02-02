using UnityEngine;
using System.Collections;

[RequireComponent (typeof (FrameBuffer))]
public class MotionManager : MonoBehaviour 
{
	Texture2D texture2D;
	RenderTexture renderTexture;
	FrameBuffer frameBuffer;

	void Start ()
	{
		frameBuffer = GetComponent<FrameBuffer>();
		renderTexture = frameBuffer.GetCurrentTexture();
		texture2D = new Texture2D(renderTexture.width, renderTexture.height);
		Shader.SetGlobalTexture("_TestTexture", texture2D);
	}

	void Update () 
	{
		renderTexture = frameBuffer.GetCurrentTexture();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, false);
		texture2D.Apply(false);
	}
}