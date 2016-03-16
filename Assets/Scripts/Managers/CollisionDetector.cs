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

		rect = new Rect(0f, 0f, GameManager.width, GameManager.height);
		texture2D = new Texture2D((int)GameManager.width, (int)GameManager.height);
		colorArray = new Color[(int)GameManager.width * (int)GameManager.height];

		if (collectibleList == null) {
			collectibleList = new List<Collectible>();
		}

		position = Vector2.zero;
	}

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
			position.x = (index % GameManager.width);
			position.y = Mathf.Floor(index / GameManager.width);
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

	public void UpdateResolution ()
	{
		rect = new Rect(0f, 0f, GameManager.width, GameManager.height);
		texture2D = new Texture2D((int)GameManager.width, (int)GameManager.height);
		colorArray = new Color[(int)GameManager.width * (int)GameManager.height];
	}

	public void AddCollectible (Collectible collectible)
	{
		if (collectibleList == null) {
			collectibleList = new List<Collectible>();
		}
		collectibleList.Add(collectible);
	}

	public void ClearCollectibleList ()
	{
		collectibleList = new List<Collectible>();
	}
}