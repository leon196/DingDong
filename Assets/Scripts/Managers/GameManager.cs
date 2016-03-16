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
	int currentBonusCount = 0;

	List<Collectible> collectibleList;
	int[] indexGridArray;
	Button startButton;

	void Start () 
	{
		gui = GameObject.FindObjectOfType<GUI>();
		collisionDetector = GameObject.FindObjectOfType<CollisionDetector>();
		collisionDetector.collisionDelegate = Collision;

		collectibleList = new List<Collectible>();
		indexGridArray = new int[Grid.width * Grid.height];
		for (int i = 0; i < indexGridArray.Length; ++i) {
			indexGridArray[i] = i;
		}

		UpdateResolution();

		startButton = new Button(new Vector2(0.5f, 0.5f));
		startButton.UpdateSize(0.3f);
		AddCollectible(startButton);

		GotoTitle();
	}

	void GotoTitle ()
	{
		gameState = GameState.Title;
		gui.GotoTitle();
		ClearCollectibleList();
		startButton.Spawn();
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
		ClearCollectibleList();
		currentBonusCount = Random.Range(1, 4);
		SpawnBonus(currentBonusCount);
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
					Shader.SetGlobalFloat("_ShowWebcam", 1f - startButton.cooldownSplash.timeRatio);
				}

				if (startButton.SplashIsOver()) {
					GotoGame();
				}

				break;
			}

			// PLAY
			case GameState.Playing : {

				for (int i = 0; i < collectibleList.Count; ++i) 
				{
					Collectible collectible = collectibleList[i];
					collectible.Update();

					if (collectible.isHitted) 
					{
						Shader.SetGlobalFloat("_SplashRatio", 1f - collectible.cooldownSplash.timeRatio);

						if (collectible.SplashIsOver()) 
						{
							// Recycle
							RemoveCollectible(i);
							i = Mathf.Max(0, i - 1);

							// Win check
							--currentBonusCount;
							if (currentBonusCount <= 0) 
							{
								currentBonusCount = Random.Range(1, 4);
								SpawnBonus(currentBonusCount);
							}
						}
					}
				}

				break;
			}

			// SCORE
			case GameState.Scoring : {
				break;
			}
		}
	}

	void SpawnBonus (int bonusCount = 1, int malusCount = 0)
	{
		int index = 0;
		indexGridArray = ArrayUtils.Shuffle<int>(indexGridArray);
		for (int i = 0; i < bonusCount; ++i) {
			Bonus bonus = new Bonus(Grid.GetIndexPosition(indexGridArray[index]));
			bonus.Spawn();
			AddCollectible(bonus);
			++index;
		}
		for (int i = 0; i < malusCount; ++i) {
			Collectible collectible = new Collectible(Grid.GetIndexPosition(indexGridArray[index]));
			collectible.Spawn();
			AddCollectible(collectible);
			++index;
		}
	}

	void AddCollectible (Collectible collectible)
	{
		collectibleList.Add(collectible);
		collisionDetector.AddCollectible(collectible);
	}

	void RemoveCollectible (int index)
	{
		collectibleList.RemoveAt(index);
		collisionDetector.RemoveCollectible(index);
	}

	void ClearCollectibleList ()
	{
		collectibleList = new List<Collectible>();
		collisionDetector.ClearCollectibleList();
	}

	void Collision (Collectible collectible)
	{
		switch (gameState) 
		{
			case GameState.Title : 
			{
				collectible.Splash();
				Shader.SetGlobalVector("_SplashPosition", collectible.position);
				AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);
				break;
			}
			case GameState.Playing : 
			{
				collectible.Splash();
				gui.SetScore(++currentScore);
				Shader.SetGlobalVector("_SplashPosition", collectible.position);
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
