using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	MotionManager motion;
	BonusManager bonus;
	TimeManager time;
	GUIManager gui;

	public AudioClip clipCollision;
	public AudioClip clipGameOver;

	static public float width = 256f;
	static public float height = 256f;

	public enum GameState { Playing, Scoring };
	GameState gameState = GameState.Playing;

	float currentScore = 0f;

	void Start () 
	{
		bonus = GameObject.FindObjectOfType<BonusManager>();
		time = GameObject.FindObjectOfType<TimeManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();
		motion = GameObject.FindObjectOfType<MotionManager>();
		motion.collisionDelegate = Collision;
		
		UpdateResolution();
		gui.SetScore(currentScore);

		Shader.SetGlobalFloat("_SplashesRatio", 0f);
	}
	
	void Update () 
	{
		switch (gameState) {

			// PLAY
			case GameState.Playing : {

				gui.UpdateCooldownSize();

				// Splash
				if (bonus.bonusHitted)  {
					bonus.UpdateSplash();

					// Respawn
					if (bonus.bonusSplashed)  {
						bonus.Respawn();
					}
				} 

				// Game
				else  {
					bonus.UpdateRespawn();

					// Cooldown
					if (bonus.bonusRespawned)  {

						if (currentScore != 0f) {
							time.UpdateCooldown();
						}

						// Goto Score
						if (time.cooldownOver) {
							gameState = GameState.Scoring;
							time.StartCooldownScore(currentScore);
							gui.CenterScore();
							bonus.Idle();
							Shader.SetGlobalFloat("_SplashesRatio", 1f);
							Shader.SetGlobalVector("_CollisionPosition", Vector2.one * 0.5f);
							AudioSource.PlayClipAtPoint(clipGameOver, Camera.main.transform.position, 0.5f);
						}
					}				
				}
				break;
			}

			// SCORE
			case GameState.Scoring : {

				bonus.UpdateIdle();
				time.UpdateCooldown();
				gui.UpdateScore();

				// Goto Play
				if (time.cooldownOver) {
					gameState = GameState.Playing;
					currentScore = 0f;
					gui.SetScore(currentScore);
					time.StartCooldown();
					time.UpdateCooldown();
					gui.SnapScore();
					bonus.Reset();
					Shader.SetGlobalFloat("_SplashesRatio", 0f);
				}

				break;
			}
		}
	}

	public void Collision (float hitX, float hitY)
	{
		if (bonus.bonusHitted == false && bonus.bonusRespawned) {
			time.StartCooldown();
			gui.SetScore(++currentScore);
			AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);
		}
		bonus.HitTest(hitX, hitY);
	}

	public void UpdateResolution ()
	{
		width = Mathf.Floor(width * Screen.width / Screen.height);
		motion.UpdateResolution();
		Shader.SetGlobalVector("_Resolution", new Vector2(width, height));
		FrameBuffer[] frameBufferArray = GameObject.FindObjectsOfType<FrameBuffer>();
		foreach (FrameBuffer frameBuffer in frameBufferArray) {
			frameBuffer.UpdateResolution();
		}
	}
}
