using UnityEngine;
using System.Collections;

public class Collectible
{
	public Vector2 position;
	public Vector2 screenPosition;
	public Color color;
	public float size;
	public float collisionRadius;
	public bool isHitted;

	public Cooldown cooldownSpawn;
	public Cooldown cooldownSplash;

	protected GameObject sprite;
	protected Material spriteMaterial;

	public Collectible (Vector2 pos) 
	{
		cooldownSpawn = new Cooldown();
		cooldownSplash = new Cooldown();
		position = pos;
		screenPosition = new Vector2();
		screenPosition.x = position.x * Game.width;
		screenPosition.y = position.y * Game.height;
		color = ColorHSV.GetRandomColor();
		size = Grid.cellSize * 2f;
		collisionRadius = Grid.cellSize / 2f * Game.height;
		isHitted = false;

		AddSprite();
	}

	public virtual void AddSprite ()
	{
		sprite = GameObject.CreatePrimitive(PrimitiveType.Quad);
		
		Material material = new Material(Shader.Find("DingDong/Collectible"));		
		material.mainTexture = TextureLoader.GetCircle();
		material.color = color;
		sprite.GetComponent<Renderer>().material = material;
		spriteMaterial = sprite.GetComponent<Renderer>().material;

		Vector3 pos = sprite.transform.position;
		pos.x = (position.x * 2f - 1f) * Screen.width / (float)Screen.height;
		pos.y = position.y * 2f - 1f;
		sprite.transform.position = pos;

		sprite.transform.localScale = Vector3.zero;
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
			 return Vector2.Distance(screenPosition, pos) < collisionRadius;
		}
		else
		{
			return false;
		}
	}

	public virtual void Update ()
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

	public void UpdateSize (float newSize)
	{
		size = newSize * 2f;
		collisionRadius = newSize / 2f * Game.height;
	}

	public bool SplashIsOver () 
	{
		return cooldownSplash.IsOver();
	}
}
