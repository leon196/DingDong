using UnityEngine;
using System.Collections;

public class TextureLoader
{
	static Texture circle;

	static public Texture GetCircle ()
	{
		if (circle == null) {
			circle = Resources.Load("circle") as Texture;
		}
		return circle;
	}
}