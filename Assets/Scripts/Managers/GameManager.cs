using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	MotionManager motion;
	BonusManager bonus;
	TimeManager time;
	GUIManager gui;
	DrawGrid grid;

	public AudioClip clipCollision;
	public AudioClip clipGameOver;

	static public float width = 256f;
	static public float height = 256f;

	public enum GameState { Title, Playing, Scoring };
	GameState gameState = GameState.Title;

	float currentScore = 0f;

	void Start () 
	{
		bonus = GameObject.FindObjectOfType<BonusManager>();
		time = GameObject.FindObjectOfType<TimeManager>();
		gui = GameObject.FindObjectOfType<GUIManager>();
		motion = GameObject.FindObjectOfType<MotionManager>();
		grid = GameObject.FindObjectOfType<DrawGrid>();
		motion.collisionDelegate = Collision;
		
		UpdateResolution();
		GotoTitle();
	}

	void GotoTitle ()
	{
		gameState = GameState.Title;
		gui.GotoTitle();
		bonus.SetBonus(new Vector2(0.5f, 0.25f), 0.15f);
		Shader.SetGlobalFloat("_SplashesRatio", 0f);
		Shader.SetGlobalFloat("_ShowWebcam", 1f);
	}

	void GotoGame ()
	{
		gameState = GameState.Playing;
		currentScore = 0f;
		gui.GotoGame();
		gui.UpdateAlpha(1f);
		gui.SetScore(currentScore);
		bonus.SetBonusSize(0.5f / grid.width);
		Shader.SetGlobalFloat("_SplashesRatio", 0f);
		Shader.SetGlobalFloat("_ShowWebcam", 0f);
	}
	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		switch (gameState) {

			// TITLE
			case GameState.Title : {

				// Collision with Start
				if (bonus.bonusHitted)  {
					bonus.UpdateSplash();

					Shader.SetGlobalFloat("_ShowWebcam", 1f - bonus.GetUpdateRatio());
					gui.UpdateAlpha(1f - bonus.GetUpdateRatio());

					// Goto Game
					if (bonus.bonusSplashed)  {
						GotoGame();
					}
				} 

				// Update
				else  {
					bonus.UpdateRespawn();
				}

				break;
			}

			// PLAY
			case GameState.Playing : {

				// gui.UpdateCooldownSize();

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

						// if (currentScore != 0f) {
							// time.UpdateCooldown();
						// }

						// Goto Score
						/*
						if (time.cooldownOver) {
							gameState = GameState.Scoring;
							time.StartCooldownScore(Mathf.Clamp(currentScore, 0f, 10f));
							gui.CenterScore();
							bonus.Idle();
							Shader.SetGlobalFloat("_SplashesRatio", 1f);
							Shader.SetGlobalVector("_CollisionPosition", Vector2.one * 0.5f);
							AudioSource.PlayClipAtPoint(clipGameOver, Camera.main.transform.position, 0.5f);
						}
						*/
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
					Shader.SetGlobalFloat("_SplashesRatio", 0f);
				}

				break;
			}
		}
	}

	public void Collision (int index, float hitX, float hitY)
	{
		if (bonus.bonusHitted == false && bonus.bonusRespawned) {
			if (gameState == GameState.Playing) {
				time.StartCooldown();
				gui.SetScore(++currentScore);
			}
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
