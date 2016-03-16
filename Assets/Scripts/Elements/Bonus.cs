using UnityEngine;
using System.Collections;

public class Bonus : Collectible
{
	public Bonus (Vector2 pos) : base(pos)
	{
		sprite.GetComponent<Renderer>().material.mainTexture = TextureLoader.GetRandomBonus();
	}
}
