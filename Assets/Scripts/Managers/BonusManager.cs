using UnityEngine;
using System.Collections;

public class BonusManager : MonoBehaviour 
{
	MotionManager motion;
	GUIManager gui;

	Color bonusColor;
	Vector2 bonusPosition;
	Vector2 splashesPosition;

	float bonusSize = 0.1f;
	float bonusTime = 0f;
	float bonusDelay = 1f;
	float bonusRatio = 0f;
	
	[HideInInspector] public bool bonusHitted = false;
	[HideInInspector] public bool bonusSplashed = false;
	[HideInInspector] public bool bonusRespawned = false;

	void Start () 
	{
		motion = GameObject.FindObjectOfType<MotionManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();

		// Position
		bonusPosition = Vector2.one * 0.5f;
		splashesPosition = bonusPosition;

		// Size
		bonusSize = Random.Range(0.05f, 0.1f);
		motion.SetTarget(bonusPosition.x, bonusPosition.y, bonusSize);

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
		SetTarget(splashesPosition, bonusSize * (1f - bonusRatio));
	}

	public void UpdateRespawn ()
	{
		bonusRatio = Mathf.Clamp((Time.time - bonusTime) / bonusDelay, 0f, 1f);
		bonusRespawned = bonusRatio == 1f;
		SetTarget(bonusPosition, bonusRatio * bonusSize);
	}

	public void UpdateIdle ()
	{
		bonusRatio = Mathf.Clamp((Time.time - bonusTime) / bonusDelay, 0f, 1f);
		SetTarget(bonusPosition, bonusSize * (1f - bonusRatio));
	}

	public void HitTest (float hitX, float hitY)
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
			gui.SetColor(bonusColor);
		}
	}

	public void SetTarget (Vector2 position, float size) 
	{
		motion.SetTarget(position.x, position.y, size);
		Shader.SetGlobalVector("_BonusPosition", position);
		Shader.SetGlobalFloat("_BonusSize", size);
	}
}
