using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	CollisionDetector collisionDetector;
	GUI gui;

	public AudioClip clipCollision;
	public AudioClip clipGameOver;

	static public float width = 256f;
	static public float height = 256f;

	public enum GameState { Title, Playing, Transition, Over };
	GameState gameState = GameState.Title;

	float currentScore = 0f;
	int currentBonusCount = 0;

	List<Collectible> collectibleList;
	int[] indexGridArray;
	Button startButton;
	Cooldown cooldownTransition;

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

		cooldownTransition = new Cooldown(5f);

		startButton = new Button(new Vector2(0.5f, 0.5f));
		startButton.UpdateSize(0.3f);
		AddCollectible(startButton);

		GotoTitle();
	}

	void GotoTitle ()
	{
		gameState = GameState.Title;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
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
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
		gui.SetScore(currentScore);
		ClearCollectibleList();
		currentBonusCount = 3;//Random.Range(1, 4);
		SpawnBonus(3);
		Shader.SetGlobalFloat("_SplashRatio", 0f);
		Shader.SetGlobalFloat("_ShowWebcam", 0f);
	}

	void GotoTransition ()
	{
		gameState = GameState.Transition;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
		gui.SetRandomMessage();
		ClearLevel();
		cooldownTransition.Start();
	}

	void GotoNextRound ()
	{
		gameState = GameState.Playing;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
		ClearCollectibleList();
		currentBonusCount = 10;
		SpawnBonus(10, 5);
	}

	void GotoOver ()
	{
		gameState = GameState.Over;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
		gui.SetOverMessage();
		ClearLevel();
		cooldownTransition.Start();
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
					float ratio = 1f - startButton.cooldownSplash.timeRatio;
					Shader.SetGlobalFloat("_SplashRatio", ratio);
					Shader.SetGlobalFloat("_ShowWebcam", ratio);
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
								GotoTransition();
							}
						}
					}
				}

				break;
			}

			// Transition 
			case GameState.Transition : 
			{
				cooldownTransition.Update();

				for (int i = 0; i < collectibleList.Count; ++i) 
				{
					Collectible collectible = collectibleList[i];
					collectible.Update();
				}

				gui.UpdateRainbow();
				float ratio = Mathf.Sin(cooldownTransition.timeRatio * Mathf.PI);
				gui.UpdateMessage(ratio);
				Shader.SetGlobalFloat("_SplashRatio", ratio);

				if (cooldownTransition.IsOver()) {
					GotoNextRound();
				}

				break;
			}

			// SCORE
			case GameState.Over : {

				cooldownTransition.Update();

				for (int i = 0; i < collectibleList.Count; ++i) 
				{
					Collectible collectible = collectibleList[i];
					collectible.Update();
				}

				gui.UpdateRainbow();
				float ratio = Mathf.Sin(cooldownTransition.timeRatio * Mathf.PI);
				gui.UpdateMessage(ratio);
				Shader.SetGlobalFloat("_SplashRatio", ratio);

				if (cooldownTransition.IsOver()) {
					GotoTitle();
				}

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
			Malus malus = new Malus(Grid.GetIndexPosition(indexGridArray[index]));
			malus.Spawn();
			AddCollectible(malus);
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

	void ClearLevel ()
	{
		for (int i = 0; i < collectibleList.Count; ++i) 
		{
			Collectible collectible = collectibleList[i];
			if (collectible.isHitted == false) {
				collectible.Splash();
			}
		}
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
				if (collectible.GetType() == typeof(Bonus))
				{
					collectible.Splash();
					gui.SetScore(++currentScore);
					Shader.SetGlobalVector("_SplashPosition", collectible.position);
					AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);
				}
				else
				{
					Shader.SetGlobalVector("_SplashPosition", collectible.position);
					AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);
					GotoOver();
				}
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
