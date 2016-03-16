using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MotionFilter : Filter 
{
	public Texture bonusTexture;
	public Texture malusTexture;

	void Awake () {
		material = new Material( Shader.Find("Hidden/Motion") );
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetTexture("_BonusTexture", bonusTexture);
		material.SetTexture("_MalusTexture", malusTexture);
		Graphics.Blit (source, destination, material);
	}
}