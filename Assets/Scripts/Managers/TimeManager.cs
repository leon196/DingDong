using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour 
{
	public float cooldownDelay = 5f;
	public float cooldownRatio = 1f;
	public bool cooldownOver = false;

	void Update () 
	{
		if (Input.GetKey(KeyCode.P)) {
			cooldownDelay = Mathf.Clamp(cooldownDelay + 1f, 1f, 100f);

		} else if (Input.GetKey(KeyCode.M)) { 
			cooldownDelay = Mathf.Clamp(cooldownDelay - 1f, 1f, 100f);
		}
	}

	public void StartCooldown (float delay = 5f)
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
