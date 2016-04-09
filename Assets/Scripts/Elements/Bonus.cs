using UnityEngine;
using System.Collections;

public class Bonus : Collectible
{
	public enum BonusType { Heart, Clover, Apple }
	public BonusType type;

	public Bonus (Vector2 pos) : base(pos)
	{
		// sprite.GetComponent<Renderer>().material.mainTexture = TextureLoader.GetRandomBonus();

		switch (Random.Range(0, 3)) {
			case 0 : {
				color = ColorHSV.GetColor(Random.Range(340f, 380f) % 360f, 1f, 1f);
				spriteMaterial.color = color;
				spriteMaterial.mainTexture = TextureLoader.GetHeart();
				type = BonusType.Heart; 
				break;
			}
			case 1 : {
				color = ColorHSV.GetColor(Random.Range(80f, 140f), 1f, 1f);
				spriteMaterial.color = color;
				spriteMaterial.mainTexture = TextureLoader.GetClover();
				type = BonusType.Clover;
				break;
			}
			case 2 : {
				color = ColorHSV.GetColor(Random.Range(0f, 140f), 1f, 1f);
				spriteMaterial.color = color;
				spriteMaterial.mainTexture = TextureLoader.GetApple();
				type = BonusType.Apple;
				break;
			}
		}

		size = Random.Range(Grid.cellSize, Grid.cellSize * 2f);
		collisionRadius = size / 4f * Game.height;
	}
}
