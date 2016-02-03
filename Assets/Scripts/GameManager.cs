using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	MotionManager motion;

	Vector2 respawnPosition;
	float respawnSize = 0.1f;
	float respawnTime = 0f;
	float respawnDelay = 3f;

	static public float width = 256f;
	static public float height = 256f;

	void Start () 
	{
		motion = GameObject.FindObjectOfType<MotionManager>();
		motion.collisionDelegate = Collision;

		width = Mathf.Floor(width * Screen.width / Screen.height);
		
		respawnPosition = Vector2.one * 0.5f;
		respawnSize = Random.Range(0.05f, 0.1f);
		motion.SetTarget(respawnPosition.x, respawnPosition.y, respawnSize);

		motion.UpdateResolution();
		Shader.SetGlobalVector("_Resolution", new Vector2(width, height));
		FrameBuffer[] frameBufferArray = GameObject.FindObjectsOfType<FrameBuffer>();
		foreach (FrameBuffer frameBuffer in frameBufferArray) {
			frameBuffer.UpdateResolution();
		}
	}
	
	void Update () 
	{
		float ratio = Mathf.Clamp((Time.time - respawnTime) / respawnDelay, 0f, 1f);
		SetTarget(respawnPosition, ratio * respawnSize);
	}

	public void Collision (float hitX, float hitY)
	{
		if (respawnTime + respawnDelay < Time.time) 
		{
			respawnTime = Time.time;
			respawnPosition.x = Random.Range(0.25f, 0.75f);
			respawnPosition.y = Random.Range(0.25f, 0.75f);
			respawnSize = Random.Range(0.05f, 0.1f);
		}
	}

	public void SetTarget (Vector2 position, float size) 
	{
		motion.SetTarget(position.x, position.y, size);
		Shader.SetGlobalVector("_BonusPosition", position);
		Shader.SetGlobalFloat("_BonusSize", size);
	}
}
