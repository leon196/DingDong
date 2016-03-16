using UnityEngine;
using System.Collections;

public class BonusManager : MonoBehaviour 
{
	MotionManager motion;
	GUIManager gui;
	DrawGrid grid;

	Color bonusColor;
	Vector2 bonusPosition;
	Vector2 splashesPosition;

	public float bonusSize;
	public Rect spawnRect;  
	float bonusDelay = 1f;
	float bonusRatio = 0f;
	float bonusTime = 0f;
	
	[HideInInspector] public bool bonusHitted = false;
	[HideInInspector] public bool bonusSplashed = false;
	[HideInInspector] public bool bonusRespawned = false;

	void Start () 
	{
		motion = GameObject.FindObjectOfType<MotionManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();
		grid = GameObject.FindObjectOfType<DrawGrid>();

		// Position
		bonusPosition = Vector2.one * 0.5f;
		splashesPosition = bonusPosition;

		// Size
		bonusSize = 0.5f / grid.width;
		SetBonus(bonusPosition, bonusSize);

		// Spawn
		// spawnRect = new Rect(0.25f, 0.25f, 0.5f, 0.5f);

		// Color
		bonusColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		Shader.SetGlobalColor("_BonusColor", bonusColor);
	}

	public void Respawn ()
	{
		bonusRatio = 0f;
		bonusTime = Time.time;
		bonusHitted = false;
		bonusColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		Shader.SetGlobalColor("_BonusColor", bonusColor);
	}

	public void Idle ()
	{
		bonusTime = Time.time;
	}

	public void UpdateSplash ()
	{
		bonusRatio = Mathf.Clamp((Time.time - bonusTime) / bonusDelay, 0f, 1f);
		bonusSplashed = bonusRatio == 1f;
		Shader.SetGlobalFloat("_SplashesRatio", 1f - bonusRatio);
		UpdateTarget(splashesPosition, bonusSize * (1f - bonusRatio));
	}

	public void UpdateRespawn ()
	{
		bonusRatio = Mathf.Clamp((Time.time - bonusTime) / bonusDelay, 0f, 1f);
		bonusRespawned = bonusRatio == 1f;
		UpdateTarget(bonusPosition, bonusRatio * bonusSize);
	}

	public void UpdateIdle ()
	{
		bonusRatio = Mathf.Clamp((Time.time - bonusTime) / bonusDelay, 0f, 1f);
		UpdateTarget(bonusPosition, bonusSize * (1f - bonusRatio));
	}

	public void HitTest (float hitX, float hitY)
	{
		if (bonusHitted == false && bonusTime + bonusDelay < Time.time) 
		{
			SpawnBonus();
		}
	}

	public Vector2 GetGridPosition (int index, int width, int height)
	{
		float x = (index % width) / (float)width;// + 0.5f / ((float)width - 1);//(width - 1f) + 1f / width;
		float y = Mathf.Floor(index / width) / (float)height;//(height - 1f) + 1f / height;
		return new Vector2(x, y);
	}

	public void SpawnBonus ()
	{
		bonusTime = Time.time;
		splashesPosition.x = bonusPosition.x;
		splashesPosition.y = bonusPosition.y;
		Shader.SetGlobalVector("_CollisionPosition", splashesPosition);
		bonusPosition = GetGridPosition(Random.Range(0, grid.width * grid.height), grid.width, grid.height);
		// bonusPosition.x = Random.Range(spawnRect.x, spawnRect.xMax);
		// bonusPosition.y = Random.Range(spawnRect.y, spawnRect.yMax);
		bonusHitted = true;
		gui.SetColor(bonusColor);
		// bonusSize = Mathf.Clamp(bonusSize - 0.001f, 0.01f, 1f);
	}

	public void SetBonus (Vector2 position, float size) 
	{
		bonusPosition.x = position.x;
		bonusPosition.y = position.y;
		SetBonusSize(size);
	}

	public void SetBonusSize (float size) 
	{
		bonusSize = size;
		UpdateTarget(bonusPosition, bonusSize);
	}

	public void UpdateTarget (Vector2 position, float size) 
	{
		motion.UpdateTarget(position.x, position.y, size);
		Shader.SetGlobalVector("_BonusPosition", position);
		Shader.SetGlobalFloat("_BonusSize", size);
	}

	public float GetUpdateRatio ()
	{
		return bonusRatio;
	}
}
