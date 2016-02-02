using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WebcamFilter : Filter 
{
	void Awake () {
		material = new Material( Shader.Find("Hidden/Webcam") );
	}
}