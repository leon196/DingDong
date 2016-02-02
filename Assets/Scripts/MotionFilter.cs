using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MotionFilter : Filter 
{
	void Awake () {
		material = new Material( Shader.Find("Hidden/Motion") );
	}
}