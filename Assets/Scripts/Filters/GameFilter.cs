using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameFilter : Filter 
{
	void Awake () {
		material = new Material( Shader.Find("Hidden/Game") );
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.I))  {
			material.SetFloat("_InvertColor", (material.GetFloat("_InvertColor") + 1f) % 2f);
		}
	}
}