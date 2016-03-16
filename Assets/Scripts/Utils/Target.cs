using UnityEngine;
using System.Collections;

public class Target
{
	public int index;
	public Vector2 position;

	public Target (int i, float x, float y) 
	{
		index = i;
		position = new Vector2(x, y);
	}
}