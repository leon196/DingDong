using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Filter : MonoBehaviour 
{
	protected Material material;

	void Awake () {
		material = new Material( Shader.Find("Hidden/Webcam") );
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		Graphics.Blit (source, destination, material);
	}
}