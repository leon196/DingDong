using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid
{
	static public int width = 6;
	static public int height = 6;
	static public float cellSize = 1f / 6f;

	static public Vector2 GetGridPosition (int index, int width, int height)
	{
		float x = (index % width) / (float)width + 0.5f / (float)width;
		float y = Mathf.Floor(index / width) / (float)height + 0.5f / (float)height;
		return new Vector2(x, y);
	}

	static public Vector2 GetIndexPosition (int index)
	{
		return GetGridPosition(index, Grid.width, Grid.height);
	}
}