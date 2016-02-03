using UnityEngine;
using System.Collections;

[RequireComponent (typeof (FrameBuffer))]
public class MotionManager : MonoBehaviour 
{
	FrameBuffer frameBuffer;

	Texture2D texture2D;
	RenderTexture renderTexture;
	Color[] colorArray;
	Rect rect;

	Vector2 target;
	Vector2 position;
	float distanceTreshold;

  public delegate void CollisionDelegate (float x, float y);
  public CollisionDelegate collisionDelegate;

	void Start ()
	{
		frameBuffer = GetComponent<FrameBuffer>();
		renderTexture = frameBuffer.GetCurrentTexture();

		rect = new Rect(0f, 0f, GameManager.width, GameManager.height);
		texture2D = new Texture2D((int)GameManager.width, (int)GameManager.height);
		colorArray = new Color[(int)GameManager.width * (int)GameManager.height];

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
			if (color.r + color.g + color.b == 3f && Vector2.Distance(position, target) < distanceTreshold) {
				collisionDelegate(position.x, position.y);
				break;
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

	public void SetTarget (float x, float y, float size) 
	{
		target = new Vector2(x * GameManager.width, y * GameManager.height);
		distanceTreshold = size * GameManager.height;
	}
}