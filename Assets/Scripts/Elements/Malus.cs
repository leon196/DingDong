using UnityEngine;
using System.Collections;

public class Malus : Collectible
{
	public Malus (Vector2 pos) : base(pos)
	{
		sprite.GetComponent<Renderer>().material.mainTexture = TextureLoader.GetSkull();

		color = Color.white;
		spriteMaterial.color = color;
	}
}
