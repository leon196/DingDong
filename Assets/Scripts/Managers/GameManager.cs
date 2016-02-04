using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	MotionManager motion;
	BonusManager bonus;
	TimeManager time;
	GUIManager gui;

	static public float width = 256f;
	static public float height = 256f;

	float currentCombo = 0f;

	void Start () 
	{
		bonus = GameObject.FindObjectOfType<BonusManager>();
		time = GameObject.FindObjectOfType<TimeManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();
		motion = GameObject.FindObjectOfType<MotionManager>();
		motion.collisionDelegate = Collision;
		
		UpdateResolution();
		gui.SetCombo(currentCombo);
	}
	
	void Update () 
	{
		if (time.cooldownOver) {
			if (bonus.bonusIdled) {
				bonus.UpdateIdle();

			} else {
				bonus.Idle();
				gui.CenterCombo();
			}

		} else {
			if (bonus.bonusHitted) {
				bonus.UpdateSplash();

				if (bonus.bonusSplashed) {
					bonus.Respawn();
				}
			} else {
				bonus.UpdateRespawn();

				if (bonus.bonusRespawned) {
					time.UpdateCooldown();
				}
			}
		}
	}

	public void Collision (float hitX, float hitY)
	{
		if (bonus.bonusHitted == false && bonus.bonusRespawned) {
			time.StartCooldown();
			gui.SetCombo(++currentCombo);
		}

		bonus.HitTest(hitX, hitY);
	}

	public void UpdateResolution ()
	{
		// Fix aspect ratio
		width = Mathf.Floor(width * Screen.width / Screen.height);

		// Update all
		motion.UpdateResolution();
		Shader.SetGlobalVector("_Resolution", new Vector2(width, height));
		FrameBuffer[] frameBufferArray = GameObject.FindObjectsOfType<FrameBuffer>();
		foreach (FrameBuffer frameBuffer in frameBufferArray) {
			frameBuffer.UpdateResolution();
		}
	}
}
