using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour 
{
	public float cooldownDelay = 5f;
	public float cooldownRatio = 1f;
	public bool cooldownOver = false;
	float userDelay;

	void Start ()
	{
		userDelay = cooldownDelay;
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.P)) {
			cooldownRatio = 1f;
			cooldownDelay = Mathf.Clamp(cooldownDelay + 1f, 1f, 100f);
			userDelay = cooldownDelay;

		} else if (Input.GetKeyDown(KeyCode.M)) { 
			cooldownRatio = 1f;
			cooldownDelay = Mathf.Clamp(cooldownDelay - 1f, 1f, 100f);
			userDelay = cooldownDelay;
		}
	}

	public void StartCooldown ()
	{
		cooldownRatio = 1f;
		cooldownDelay = userDelay;
	}

	public void StartCooldownScore (float delay)
	{
		cooldownRatio = 1f;
		cooldownDelay = delay;
	}

	public void UpdateCooldown ()
	{
		cooldownRatio = Mathf.Clamp(cooldownRatio - Time.deltaTime / cooldownDelay, 0f, 1f);
		cooldownOver = cooldownRatio == 0f;
	}
}
