using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WebcamFilter : Filter 
{
	public bool mirrorX = false;
	public bool mirrorY = false;

	void Awake () 
	{
		material = new Material( Shader.Find("Hidden/Webcam") );
		material.SetFloat("_MirrorX", mirrorX ? 1f: 0f);
		material.SetFloat("_MirrorY", mirrorY ? 1f: 0f);
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.X))  {
			SetMirrorX(!mirrorX);

		} else if (Input.GetKeyDown(KeyCode.Y)) {
			SetMirrorY(!mirrorY);
		}
	}

	public void SetMirrorX (bool value)
	{
		mirrorX = value;
		material.SetFloat("_MirrorX", mirrorX ? 1f: 0f);
	}

	public void SetMirrorY (bool value)
	{
		mirrorY = value;
		material.SetFloat("_MirrorY", mirrorY ? 1f: 0f);
	}
}