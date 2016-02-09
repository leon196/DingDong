using UnityEngine;
using System.Collections;

public class BonusManager : MonoBehaviour 
{
	MotionManager motion;
	GUIManager gui;
	DrawLine drawLine;

	Color bonusColor;
	Vector2 bonusPosition;
	Vector2 splashesPosition;

	public float bonusSize = 0.1f;
	public Rect spawnRect;  
	float bonusDelay = 1f;
	float bonusRatio = 0f;
	float bonusTime = 0f;
	float bonusSizeInit;
	
	[HideInInspector] public bool bonusHitted = false;
	[HideInInspector] public bool bonusSplashed = false;
	[HideInInspector] public bool bonusRespawned = false;

	void Start () 
	{
		motion = GameObject.FindObjectOfType<MotionManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();
		drawLine = GameObject.FindObjectOfType<DrawLine>();

		// Position
		bonusPosition = Vector2.one * 0.5f;
		splashesPosition = bonusPosition;

		// Size
		motion.SetTarget(bonusPosition.x, bonusPosition.y, bonusSize);
		bonusSizeInit = bonusSize;

		// Spawn
		spawnRect = new Rect(0.25f, 0.25f, 0.5f, 0.5f);

		// Color
		bonusColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		Shader.SetGlobalColor("_BonusColor", bonusColor);

		drawLine.AddRectangle("spawnRect", spawnRect);
	}

	void Update ()
	{
		// Bonus size
		if (Input.GetKey(KeyCode.KeypadPlus)) {
			bonusSize = Mathf.Clamp(bonusSize + Time.deltaTime * 0.1f, 0.01f, 1f);
			bonusSizeInit = bonusSize;
		} else if (Input.GetKey(KeyCode.KeypadMinus)) {
			bonusSize = Mathf.Clamp(bonusSize - Time.deltaTime * 0.1f, 0.01f, 1f);
			bonusSizeInit = bonusSize;
		}

		// Spawn rect
		if (Input.GetMouseButtonDown(0)) {
			spawnRect.xMin = Input.mousePosition.x / Screen.width;
			spawnRect.yMin = Input.mousePosition.y / Screen.height;

		} else if (Input.GetMouseButton(0)) {
			spawnRect.xMax = Input.mousePosition.x / Screen.width;
			spawnRect.yMax = Input.mousePosition.y / Screen.height;
		}

		if (Input.anyKey) {
			drawLine.UpdateRectangle("spawnRect", spawnRect);
		}
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

	public void Reset ()
	{
		bonusSize = bonusSizeInit;
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
			bonusPosition.x = Random.Range(spawnRect.x, spawnRect.xMax);
			bonusPosition.y = Random.Range(spawnRect.y, spawnRect.yMax);
			bonusHitted = true;
			gui.SetColor(bonusColor);
			bonusSize = Mathf.Clamp(bonusSize - 0.001f, 0.01f, 1f);
		}
	}

	public void SetTarget (Vector2 position, float size) 
	{
		motion.SetTarget(position.x, position.y, size);
		Shader.SetGlobalVector("_BonusPosition", position);
		Shader.SetGlobalFloat("_BonusSize", size);
	}
}
