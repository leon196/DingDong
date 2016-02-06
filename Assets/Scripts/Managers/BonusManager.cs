using UnityEngine;
using System.Collections;

public class BonusManager : MonoBehaviour 
{
	GameManager game;
	MotionManager motion;
	GUIManager gui;

	Color bonusColor;
	Vector2 bonusPosition;
	Vector2 splashesPosition;

	public float bonusSize = 0.1f;
	public float minX = 0.25f;
	public float maxX = 0.75f;
	public float minY = 0.25f;
	public float maxY = 0.75f;
	float bonusDelay = 0.5f;
	float bonusRatio = 0f;
	float bonusTime = 0f;
	float bonusSizeInit;
	
	[HideInInspector] public bool bonusHitted = false;
	[HideInInspector] public bool bonusSplashed = false;
	[HideInInspector] public bool bonusRespawned = false;

	void Start () 
	{
		game = GameObject.FindObjectOfType<GameManager>();
		motion = GameObject.FindObjectOfType<MotionManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();

		// Position
		bonusPosition = Vector2.one * 0.5f;
		splashesPosition = bonusPosition;

		// Size
		motion.SetTarget(bonusPosition.x, bonusPosition.y, bonusSize);
		bonusSizeInit = bonusSize;

		// Color
		bonusColor = ColorHSV.GetRandomColor(Random.Range(0.0f, 360f), 1, 1);
		Shader.SetGlobalColor("_BonusColor", bonusColor);

		UpdateUniform();
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

		// Spawn rect
		} else if (Input.GetKey(KeyCode.Keypad1)) {
			minX = Mathf.Clamp(minX - Time.deltaTime * 0.1f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.Keypad2)) {
			minX = Mathf.Clamp(minX + Time.deltaTime * 0.1f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.Keypad8)) {
			maxX = Mathf.Clamp(maxX - Time.deltaTime * 0.1f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.Keypad9)) {
			maxX = Mathf.Clamp(maxX + Time.deltaTime * 0.1f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.Keypad3)) {
			minY = Mathf.Clamp(minY - Time.deltaTime * 0.1f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.Keypad6)) {
			minY = Mathf.Clamp(minY + Time.deltaTime * 0.1f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.Keypad4)) {
			maxY = Mathf.Clamp(maxY - Time.deltaTime * 0.1f, 0f, 1f);
		} else if (Input.GetKey(KeyCode.Keypad7)) {
			maxY = Mathf.Clamp(maxY + Time.deltaTime * 0.1f, 0f, 1f);
		}

		if (Input.anyKey) {
			UpdateUniform();
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
			bonusPosition.x = Random.Range(minX, maxX);
			bonusPosition.y = Random.Range(minY, maxY);
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

	public void UpdateUniform ()
	{
		Shader.SetGlobalFloat("_MinX", minX);
		Shader.SetGlobalFloat("_MaxX", maxX);
		Shader.SetGlobalFloat("_MinY", minY);
		Shader.SetGlobalFloat("_MaxY", maxY);
	}
}
