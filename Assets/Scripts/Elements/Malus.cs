using UnityEngine;
using System.Collections;

public class Malus : Collectible
{
	public Malus (Vector2 pos) : base(pos)
	{
		sprite.GetComponent<Renderer>().material.mainTexture = TextureLoader.GetSkull();
		color = Color.white;//ColorHSV.GetColor(300f, 1f, 1f);
		spriteMaterial.color = color;
	}
}
