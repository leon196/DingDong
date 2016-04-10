using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (FrameBuffer))]
public class CollisionDetector : MonoBehaviour 
{
	FrameBuffer frameBuffer;

	Texture2D texture2D;
	RenderTexture renderTexture;
	Color[] colorArray;
	Rect rect;
	Vector2 position;

	List<Collectible> collectibleList;

  public delegate void CollisionDelegate (Collectible collectible);
  public CollisionDelegate collisionDelegate;

	void Start ()
	{
		frameBuffer = GetComponent<FrameBuffer>();
		renderTexture = frameBuffer.GetCurrentTexture();

		rect = new Rect(0f, 0f, Game.width, Game.height);
		texture2D = new Texture2D((int)Game.width, (int)Game.height);
		colorArray = new Color[(int)Game.width * (int)Game.height];

		if (collectibleList == null) {
			collectibleList = new List<Collectible>();
		}

		position = Vector2.zero;
	}
	/*
	void Update () 
	{
		renderTexture = frameBuffer.GetCurrentTexture();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(rect, 0, 0, false);
		texture2D.Apply(false);

		position = Vector2.zero;

		colorArray = texture2D.GetPixels();
		int index = 0;
		foreach (Color color in colorArray) {
			position.x = (index % Game.width);
			position.y = Mathf.Floor(index / Game.width);
			if (color.r + color.g + color.b == 3f) {
				foreach (Collectible collectible in collectibleList) {
					if (collectible.HitTest(position)) {
						collisionDelegate(collectible);
						break;
					}
				}
			}
			++index;
		}
	}
*/
	public void UpdateResolution ()
	{
		rect = new Rect(0f, 0f, Game.width, Game.height);
		texture2D = new Texture2D((int)Game.width, (int)Game.height);
		colorArray = new Color[(int)Game.width * (int)Game.height];
	}

	public void AddCollectible (Collectible collectible)
	{
		if (collectibleList == null) {
			collectibleList = new List<Collectible>();
		}
		collectibleList.Add(collectible);
	}

	public void RemoveCollectible (int index)
	{
		collectibleList.RemoveAt(index);
	}

	public void ClearCollectibleList ()
	{
		collectibleList = new List<Collectible>();
	}
}