using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GameFilter : Filter 
{
	void Awake () {
		material = new Material( Shader.Find("Hidden/Game") );
	}
}