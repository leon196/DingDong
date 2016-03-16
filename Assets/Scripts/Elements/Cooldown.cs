using UnityEngine;
using System.Collections;

public class Cooldown
{
	public float timeDelay;
	public float timeRatio;
	public float timeSpawn;

	public Cooldown (float delay = 1f)
	{
		timeDelay = delay;
		timeRatio = 0f;
		timeSpawn = 0f;
	}

	public void Start ()
	{
		timeRatio = 0f;
		timeSpawn = Time.time;
	}

	public void Update ()
	{
		timeRatio = Mathf.Clamp((Time.time - timeSpawn) / timeDelay, 0f, 1f);
	}

	public bool IsOver ()
	{
		return timeRatio >= 1f;
	}
}
