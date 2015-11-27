using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Choup
{
	public Vector2 position = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
	public float radius = 5f;
	public Color color = Color.red;

	public void Rumble ()
	{
		position = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
	}
}

public class Choupichoup
{
	public Choup choup;
	public Texture2D texture;
	Color[] colorArray;

	public Choupichoup (int textureWidth, int textureHeight)
	{
		choup = new Choup();

		colorArray = new Color[textureWidth * textureHeight];
		for (int i = 0; i < colorArray.Length; ++i) {
			colorArray[i] = Color.black;
		}

		texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);
		texture.SetPixels(colorArray);
		texture.Apply(false);
		Shader.SetGlobalTexture("_TextureChoupichoup", texture);
	}

	public void Update ()
	{
	}

	public void Spawn ()
	{
		choup.Rumble();
		for (int i = 0; i < colorArray.Length; ++i) {
			float x = i % texture.width;
			float y = Mathf.Floor(i / texture.width);
			float cx = choup.position.x * texture.width;
			float cy = choup.position.y * texture.height;
			float distance = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));
			float shade = Mathf.Round(Mathf.Clamp(distance, 0f, choup.radius) / choup.radius);
			colorArray[i] = Color.Lerp(Color.red, Color.black, shade);
		}
		texture.SetPixels(colorArray);
		texture.Apply(false);
	}
}