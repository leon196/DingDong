using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour 
{
	[HideInInspector] public float cooldownDelay = 5f;
	[HideInInspector] public float cooldownRatio = 1f;
	[HideInInspector] public bool cooldownOver = false;

	void Update () 
	{
		if (Input.GetKey(KeyCode.P)) {
			cooldownDelay = Mathf.Clamp(cooldownDelay + 1f, 1f, 100f);

		} else if (Input.GetKey(KeyCode.M)) { 
			cooldownDelay = Mathf.Clamp(cooldownDelay - 1f, 1f, 100f);
		}
	}

	public void StartCooldown ()
	{
		cooldownRatio = 1f;
	}

	public void UpdateCooldown ()
	{
		cooldownRatio = Mathf.Clamp(cooldownRatio - Time.deltaTime / cooldownDelay, 0f, 1f);
		cooldownOver = cooldownRatio == 0f;
	}
}
