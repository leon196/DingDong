using UnityEngine;
using System.Collections;

public class Button : Collectible
{
	Color colorAlpha;

	public Button (Vector2 pos) : base(pos)
	{
		color = Color.red;
		spriteMaterial.color = color;
		colorAlpha = color;
		colorAlpha.a = 0.5f;
	}

	public override void Update ()
	{
		base.Update();
		spriteMaterial.color = Color.Lerp(color, colorAlpha, Mathf.Sin(Time.time * 5f) * 0.5f + 0.5f);
	}
}
