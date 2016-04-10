using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	CollisionDetector collisionDetector;
	GUI gui;

	public AudioClip clipCollision;
	public AudioClip clipGameOver;
	public AudioClip clipWin;

	static public float width = 512f;
	static public float height = 512f;

	public enum GameState { Title, Playing, Transition, Over, Chilling };
	GameState gameState = GameState.Chilling;

	float currentScore = 0f;
	int currentBonusCount = 0;
	int currentRound = 0;

	List<Collectible> collectibleList;
	int[] indexGridArray;
	Button startButton;
	Cooldown cooldownTransition;
	Cooldown cooldownChilling;

	float playerLife;
	bool playerHurted;

	float debugWebcam = 0f;

	void Awake ()
	{
		width = Mathf.Floor(width * Screen.width / Screen.height);
	}

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
		cooldownChilling = new Cooldown(0.5f);
		cooldownChilling.Start();

		startButton = new Button(new Vector2(0.5f, 0.5f));
		startButton.UpdateSize(0.3f);
		// AddCollectible(startButton);

		Cursor.visible = false;

		GotoChilling();
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
		Shader.SetGlobalFloat("_HurtRatio", 0f);
	}

	void GotoGame ()
	{
		gameState = GameState.Playing;
		currentScore = 0f;
		currentRound = 0;
		playerLife = 3f;
		playerHurted = false;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
		gui.SetScore(currentScore, playerLife);
		ClearCollectibleList();
		currentBonusCount = 3;
		SpawnBonus(3);
		Shader.SetGlobalFloat("_SplashRatio", 0f);
		Shader.SetGlobalFloat("_ShowWebcam", 0f);
		Shader.SetGlobalFloat("_HurtRatio", 0f);
	}

	void GotoTransition ()
	{
		gameState = GameState.Transition;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);

		if (playerHurted == false) {
			gui.SetRandomMessage();
			ClearLevel();
		}
		
		cooldownTransition.Start();
	}

	void GotoBackToGame ()
	{
		gameState = GameState.Playing;
		gui.Goto(gameState);
		playerHurted = false;
		Shader.SetGlobalFloat("_HurtRatio", 0f);
	}

	void GotoNextRound ()
	{
		gameState = GameState.Playing;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
		ClearCollectibleList();
		++currentRound;
		currentBonusCount = Random.Range(3, 3 + (int)Mathf.Clamp(currentRound, 0, 10));
		SpawnBonus(currentBonusCount, Random.Range(1, 1 + (int)Mathf.Clamp(currentRound, 0, 3)));
		Shader.SetGlobalFloat("_HurtRatio", 0f);
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

	void GotoChilling ()
	{
		gameState = GameState.Chilling;
		gui.Goto(gameState);
		gui.UpdateAlpha(1f);
		Shader.SetGlobalFloat("_SplashRatio", 0f);
		Shader.SetGlobalFloat("_ShowWebcam", 0f);
		Shader.SetGlobalFloat("_HurtRatio", 0f);
	}
	
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.D)) {
			debugWebcam = (debugWebcam + 1f) % 2f;
			Shader.SetGlobalFloat("_ShowWebcam", debugWebcam);
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
				Shader.SetGlobalFloat("_SplashRatio", ratio);

				if (playerHurted == false) {
					gui.UpdateMessage(ratio);

					if (cooldownTransition.IsOver()) {
						GotoNextRound();
					}
				} else {
					gui.UpdateWatchOut(ratio);
					float ratioScanline = Mathf.Sin(Mathf.Clamp(cooldownTransition.timeRatio * 20f, 0f, Mathf.PI));
					Shader.SetGlobalFloat("_HurtRatio", ratioScanline);

					if (cooldownTransition.IsOver()) {
						GotoBackToGame();
					}
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

				float ratioScanline = Mathf.Sin(Mathf.Clamp(cooldownTransition.timeRatio * 20f, 0f, Mathf.PI));
				Shader.SetGlobalFloat("_HurtRatio", ratioScanline);

				if (cooldownTransition.IsOver()) {
					GotoTitle();
				}

				break;
			}

			// CHILLING
			case GameState.Chilling : {

				cooldownChilling.Update();

				if (cooldownChilling.IsOver()) {
					cooldownChilling.Start();

					if (collectibleList.Count < Grid.width * Grid.height - 1) {
						SpawnBonus(1);
					}
				}

				for (int i = 0; i < collectibleList.Count; ++i) 
				{
					Collectible collectible = collectibleList[i];
					collectible.Update();

					if (collectible.isHitted)  {
						Shader.SetGlobalFloat("_SplashRatio", 1f - collectible.cooldownSplash.timeRatio);

						if (collectible.SplashIsOver())  {
							RemoveCollectible(i);
							i = Mathf.Max(0, i - 1);
						}
					}
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
					gui.SetScore(++currentScore, playerLife);
					Shader.SetGlobalVector("_SplashPosition", collectible.position);
					AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);

					// Win check
					--currentBonusCount;
					if (currentBonusCount <= 0) 
					{
						GotoTransition();
						AudioSource.PlayClipAtPoint(clipWin, Camera.main.transform.position);
					}

					// Get Life
					Bonus bonus = (Bonus)collectible;
					if (bonus.type == Bonus.BonusType.Heart) {
						++playerLife;
						gui.SetScore(currentScore, playerLife);
					}
				}
				else
				{
					collectible.Splash();
					Shader.SetGlobalVector("_SplashPosition", collectible.position);

					// Game over check
					--playerLife;
					if (playerLife < 0f) {
						GotoOver();
						AudioSource.PlayClipAtPoint(clipGameOver, Camera.main.transform.position);
					} else {
						gui.SetScore(currentScore, playerLife);
						playerHurted = true;
						GotoTransition();
						AudioSource.PlayClipAtPoint(clipCollision, Camera.main.transform.position);
					}
				}
				break;
			}
			case GameState.Chilling : 
			{
				collectible.Splash();
				Shader.SetGlobalVector("_SplashPosition", collectible.position);
				break;
			}
		}
	}

	void UpdateResolution ()
	{
		collisionDetector.UpdateResolution();
		Shader.SetGlobalVector("_Resolution", new Vector2(width, height));
		FrameBuffer[] frameBufferArray = GameObject.FindObjectsOfType<FrameBuffer>();
		foreach (FrameBuffer frameBuffer in frameBufferArray) {
			frameBuffer.UpdateResolution();
		}
	}
}
