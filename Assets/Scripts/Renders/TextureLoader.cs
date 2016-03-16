using UnityEngine;
using System.Collections;

public class TextureLoader
{
	static Texture circle;
	static Texture apple;
	static Texture strawberry;
	static Texture cake;
	static Texture bomb;

	static Texture[] bonusArray;

	static public Texture GetCircle ()
	{
		if (circle == null) { circle = Resources.Load("circle") as Texture; }
		return circle;
	}

	static public Texture GetApple ()
	{
		if (apple == null) { apple = Resources.Load("shiny-apple") as Texture; }
		return apple;
	}

	static public Texture GetStrawberry ()
	{
		if (strawberry == null) { strawberry = Resources.Load("strawberry") as Texture; }
		return strawberry;
	}

	static public Texture GetCake ()
	{
		if (cake == null) { cake = Resources.Load("cake-slice") as Texture; }
		return cake;
	}

	static public Texture GetRandomBonus ()
	{
		if (bonusArray == null) { 
			bonusArray = new Texture[3]; 
			bonusArray[0] = GetApple();
			bonusArray[1] = GetStrawberry();
			bonusArray[2] = GetCake();
		}
		return bonusArray[Random.Range(0, bonusArray.Length)];
	}

	static public Texture GetBomb ()
	{
		if (bomb == null) { bomb = Resources.Load("unlit-bomb") as Texture; }
		return bomb;
	}
}