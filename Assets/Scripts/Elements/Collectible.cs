using UnityEngine;
using System.Collections;

public class Collectible
{
	public Vector2 position;
	public Color color;
	public float size;
	public float collisionRadius;
	public bool isHitted;
	public bool isBonus;

	public Cooldown cooldownSpawn;
	public Cooldown cooldownSplash;

	GameObject sprite;

	public Collectible (Vector2 pos = new Vector2()) 
	{
		cooldownSpawn = new Cooldown();
		cooldownSplash = new Cooldown();
		position = pos;
		color = ColorHSV.GetRandomColor();
		size = Grid.cellSize * 2f;
		collisionRadius = Grid.cellSize * GameManager.height;
		isHitted = false;
		isBonus = true;

		AddSprite();
	}

	void AddSprite ()
	{
		sprite = GameObject.CreatePrimitive(PrimitiveType.Quad);
		
		Material material = new Material(Shader.Find("DingDong/Collectible"));		
		material.mainTexture = TextureLoader.GetCircle();
		material.color = color;
		sprite.GetComponent<Renderer>().material = material;

		Vector3 pos = sprite.transform.position;
		pos.x = (position.x * 2f - 1f) * Screen.width / (float)Screen.height;
		pos.y = position.y * 2f - 1f;
		sprite.transform.position = pos;

		sprite.transform.localScale = Vector3.one * size;
	}

	public void Spawn ()
	{
		cooldownSpawn.Start();
	}

	public void Splash ()
	{
		isHitted = true;
		cooldownSplash.Start();
	}

	public bool HitTest (Vector2 pos)
	{
		if (isHitted == false && cooldownSpawn.IsOver())
		{
			 return Vector2.Distance(position, pos) < collisionRadius;
		}
		else
		{
			return false;
		}
	}

	public void Update ()
	{
		if (isHitted == false) 
		{
			cooldownSpawn.Update();
			sprite.transform.localScale = Vector3.one * Mathf.Lerp(0f, size, cooldownSpawn.timeRatio);
		} 
		else 
		{
			cooldownSplash.Update();
			sprite.transform.localScale = Vector3.one * Mathf.Lerp(size, 0f, cooldownSplash.timeRatio);
		}
	}

	public bool SplashIsOver () 
	{
		return cooldownSplash.IsOver();
	}
}
