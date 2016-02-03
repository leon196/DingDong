using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	MotionManager motion;

	Vector2 bonusPosition;
	Vector2 splashesPosition;
	float bonusSize = 0.1f;
	float bonusTime = 0f;
	float bonusDelay = 1f;
	Color bonusColor;
	bool bonusHitted = false;

	static public float width = 256f;
	static public float height = 256f;

	void Start () 
	{
		motion = GameObject.FindObjectOfType<MotionManager>();
		motion.collisionDelegate = Collision;

		width = Mathf.Floor(width * Screen.width / Screen.height);
		
		bonusPosition = Vector2.one * 0.5f;
		splashesPosition = bonusPosition;
		bonusSize = Random.Range(0.05f, 0.1f);
		motion.SetTarget(bonusPosition.x, bonusPosition.y, bonusSize);

		motion.UpdateResolution();
		Shader.SetGlobalVector("_Resolution", new Vector2(width, height));
		FrameBuffer[] frameBufferArray = GameObject.FindObjectsOfType<FrameBuffer>();
		foreach (FrameBuffer frameBuffer in frameBufferArray) {
			frameBuffer.UpdateResolution();
		}
		bonusColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		Shader.SetGlobalColor("_BonusColor", bonusColor);
	}
	
	void Update () 
	{
		float ratio;

		if (bonusHitted) 
		{
			ratio = Mathf.Clamp((Time.time - bonusTime) / bonusDelay, 0f, 1f);
			Shader.SetGlobalFloat("_SplashesRatio", 1f - ratio);
			SetTarget(splashesPosition, bonusSize * (1f - ratio));
			
			if (ratio == 1f) 
			{
				bonusTime = Time.time;
				bonusHitted = false;
				bonusColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
				Shader.SetGlobalColor("_BonusColor", bonusColor);
			}

		} else {
			ratio = Mathf.Clamp((Time.time - bonusTime) / bonusDelay, 0f, 1f);
			SetTarget(bonusPosition, ratio * bonusSize);
		}
	}

	public void Collision (float hitX, float hitY)
	{
		if (bonusHitted == false && bonusTime + bonusDelay < Time.time) 
		{
			bonusTime = Time.time;
			splashesPosition.x = bonusPosition.x;
			splashesPosition.y = bonusPosition.y;
			Shader.SetGlobalVector("_CollisionPosition", splashesPosition);
			bonusPosition.x = Random.Range(0.25f, 0.75f);
			bonusPosition.y = Random.Range(0.25f, 0.75f);
			bonusSize = Random.Range(0.01f, 0.03f);
			bonusHitted = true;
		}
	}

	public void SetTarget (Vector2 position, float size) 
	{
		motion.SetTarget(position.x, position.y, size);
		Shader.SetGlobalVector("_BonusPosition", position);
		Shader.SetGlobalFloat("_BonusSize", size);
	}
}
