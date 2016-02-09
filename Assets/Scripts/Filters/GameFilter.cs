using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameFilter : Filter 
{
	public float colorGrayCount = 8f;

	void Awake () {
		material = new Material( Shader.Find("Hidden/Game") );
		material.SetFloat("_GrayCount", colorGrayCount);
	}
}