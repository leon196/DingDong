using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	CollisionDetector collisionDetector;
	GUI gui;

	public AudioClip clipCollision;
	public AudioClip clipGameOver;

	static public float width = 256f;
	static public float height = 256f;

	public enum GameState { Title, Playing, Scoring };
	GameState gameState = GameState.Title;

	float currentScore = 0f;

	List<Collectible> collectibleList;
	Button startButton;

	void Start () 
	{
		gui = GameObject.FindObjectOfType<GUI>();
		collisionDetector = GameObject.FindObjectOfType<CollisionDetector>();
		collisionDetector.collisionDelegate = Collision;

		collectibleList = new List<Collectible>();

		startButton = new Button(new Vector2(0.5f, 0.25f));
		AddCollectible(startButton);

		UpdateResolution();
		GotoTitle();
	}

	void GotoTitle ()
	{
		gameState = GameState.Title;
		gui.GotoTitle();
		startButton.Spawn();
		collisionDetector.ClearCollectibleList();
		collisionDetector.AddCollectible(startButton);
		Shader.SetGlobalFloat("_SplashRatio", 0f);
		Shader.SetGlobalFloat("_ShowWebcam", 1f);
	}

	void GotoGame ()
	{
		gameState = GameState.Playing;
		currentScore = 0f;
		gui.GotoGame();
		gui.UpdateAlpha(1f);
		gui.SetScore(currentScore);
		collisionDetector.ClearCollectibleList();
		Shader.SetGlobalFloat("_SplashRatio", 0f);
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

				startButton.Update();

				if (startButton.isHitted) {
					Shader.SetGlobalFloat("_SplashRatio", 1f - startButton.cooldownSplash.timeRatio);
				}

				if (startButton.SplashIsOver()) {
					GotoGame();
					Debug.Log("ding");
				}

				break;
			}

			// PLAY
			case GameState.Playing : {

				break;
			}

			// SCORE
			case GameState.Scoring : {
				break;
			}
		}
	}

	void SpawnBonus ()
	{
		Collectible collectible = new Collectible(Grid.GetIndexPosition(Random.Range(0, Grid.width * Grid.height)));
		AddCollectible(collectible);
	}

	void AddCollectible (Collectible collectible)
	{
		collectibleList.Add(collectible);
		collisionDetector.AddCollectible(collectible);
	}

	void Collision (Collectible collectible)
	{
		switch (gameState) 
		{
			case GameState.Title : 
			{
				startButton.Splash();
				Shader.SetGlobalVector("_SplashPosition", startButton.position);
				AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);
				break;
			}
			case GameState.Playing : 
			{
				gui.SetScore(++currentScore);
				AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);
				break;
			}
		}
	}

	void UpdateResolution ()
	{
		width = Mathf.Floor(width * Screen.width / Screen.height);
		collisionDetector.UpdateResolution();
		Shader.SetGlobalVector("_Resolution", new Vector2(width, height));
		FrameBuffer[] frameBufferArray = GameObject.FindObjectsOfType<FrameBuffer>();
		foreach (FrameBuffer frameBuffer in frameBufferArray) {
			frameBuffer.UpdateResolution();
		}
	}
}
