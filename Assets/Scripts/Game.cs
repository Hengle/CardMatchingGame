using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KeenTween;

public class Game:MonoBehaviour
{
	static private Game _current;
	static public Game current
	{
		get
		{
			return _current ? _current : _current = FindObjectOfType<Game>();
		}
	}

	public enum GameMode { Standard, MateMatch }

	//This difficulty stuff should probably exist somewhere else?
	//Im not sure what the UI for this is going to be.
	public enum Difficulty { Easy, Medium, Hard };
	public Difficulty difficulty = Difficulty.Medium;

	public Level testLevel;
	public bool includeLions = true;
	public Background currentBackground;
	public float backgroundAspectRationShiftAmount = 1;
	public Card cardPrefab;
	public float cardSafeBounds = 4;
	public float cardSpacing = 1.25f;
    public AudioClip cardFlipClip;
    public AudioClip clipFruitFall;
    public AudioClip clipLion;

	public GameStats gameStats;

	public Level currentLevel {get; private set;}

	private Card[,] cardGrid = null;
	private Card flippedCard = null;
	private bool gameIsStarted = false;
	private Coroutine flipCardCoroutine;

	void Awake() {
		currentLevel = testLevel;
		if (LevelSelect.gameInfo.selectedLevel)
		{
			currentLevel = LevelSelect.gameInfo.selectedLevel;
			includeLions = LevelSelect.gameInfo.lionMode;
		}
    }

	void Start()
	{
		SetupNewGame();
	}

	void SetupNewGame() {
		flippedCard = null;
		gameIsStarted = false;
		flipCardCoroutine = null;

		gameStats = new GameStats();

		SetupBackground();

		ClearCards();
		cardGrid = new Card[currentLevel.cardCountX, currentLevel.cardCountY];
		
		List<Vector2Int> availableCardSlots = new List<Vector2Int>();
		
		for (int x = 0; x < currentLevel.cardCountX; x++) {
			for (int y = 0; y < currentLevel.cardCountY; y++) {
				availableCardSlots.Add(new Vector2Int(x, y));
			}
		}

		int counter = 0;

		if (includeLions)
		{
			while (availableCardSlots.Count > 0 && counter < currentLevel.lionCardDefs.Count)
			{
				CardDef cardDef = currentLevel.lionCardDefs[counter];
				AddCard(availableCardSlots, cardDef);
				counter++;
			}

			counter = 0;
		}

		List<CardDef> shuffledCardDefs = new List<CardDef>();
		foreach (var cardDefGroup in currentLevel.cardDefGroups)
		{
			shuffledCardDefs.Add(cardDefGroup.GetRandomCard());
		}

		Shuffle(shuffledCardDefs);

		while (availableCardSlots.Count > 0) {
			CardDef cardDef = shuffledCardDefs[counter%shuffledCardDefs.Count];

			bool skip = !cardDef;
			if (currentLevel.useConservation)
			{
				skip |= Random.value >= cardDef.probability;
			}

			if (skip)
			{
				counter++;
				continue;
			}

			AddCard(availableCardSlots, cardDef);

			counter++;
		}

		StartCoroutine(PreGameSequence());
	}

	public static void Shuffle<T>(List<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = Random.Range(0, n+1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	private void AddCard(List<Vector2Int> availableCardSlots, CardDef cardDef)
	{
		int slot0Index = Random.Range(0, availableCardSlots.Count);
		Vector2 slot0 = availableCardSlots[slot0Index];
		availableCardSlots.RemoveAt(slot0Index);

		int slot1Index = Random.Range(0, availableCardSlots.Count);
		Vector2 slot1 = availableCardSlots[slot1Index];
		availableCardSlots.RemoveAt(slot1Index);

		Card card0 = CreateCardInstance(cardDef);
		Card card1 = CreateCardInstance(cardDef);

		card0.tilePositionX = (int)slot0.x;
		card0.tilePositionY = (int)slot0.y;

		card1.tilePositionX = (int)slot1.x;
		card1.tilePositionY = (int)slot1.y;

		(card0.transform.position, card0.transform.localScale) = GetCardGridTransformation(card0.tilePositionX, card0.tilePositionY);
		(card1.transform.position, card1.transform.localScale) = GetCardGridTransformation(card1.tilePositionX, card1.tilePositionY);

		cardGrid[card0.tilePositionX, card0.tilePositionY] = card0;
		cardGrid[card1.tilePositionX, card1.tilePositionY] = card1;
	}

	IEnumerator PreGameSequence()
	{
		Card[] cards = GetAllCards();

		Vector3 targetCardScale = cardGrid[0, 0].transform.localScale;
		foreach (var card in cards)
		{
			card.transform.localScale = Vector3.zero;
		}

		yield return new WaitForSeconds(0.5f);

		float waveDelay = 0.1f;

		for (int y = 0; y < currentLevel.cardCountY; y++)
		{
			for (int x = 0; x < currentLevel.cardCountX; x++)
			{
                OneShotAudio.Play(cardFlipClip, 0, GameSettings.Audio.sfxVolume);

                Card card = cardGrid[x, currentLevel.cardCountY-y-1];
				card.transform.localScale = Vector3.zero;
				new Tween(null, 0, 1, 1.0f, new CurveElastic(TweenCurveMode.Out), t =>
				{
					if (!card)
					{
						return;
					}
					card.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, targetCardScale, t.currentValue);
				});
				
				yield return new WaitForSeconds(waveDelay); 
			}
		}

		yield return new WaitForSeconds(0.25f);

		for (int y = 0; y < currentLevel.cardCountY; y++)
		{
			for (int x = 0; x < currentLevel.cardCountX; x++)
			{
                Card card = cardGrid[x, currentLevel.cardCountY-y-1];
				card.isFlipped = true;
				yield return new WaitForSeconds(waveDelay);
			}
		}

		yield return new WaitForSeconds(3);
		
		for (int y = 0; y < currentLevel.cardCountY; y++)
		{
			for (int x = 0; x < currentLevel.cardCountX; x++)
			{
                OneShotAudio.Play(cardFlipClip, 0, GameSettings.Audio.sfxVolume*0.5f);

                Card card = cardGrid[x, currentLevel.cardCountY-y-1];
				card.isFlipped = false;
				yield return new WaitForSeconds(waveDelay);
			}
		}

		yield return new WaitForSeconds(1.0f);

		int shuffleCount = Mathf.FloorToInt((currentLevel.cardCountX+currentLevel.cardCountY)/2.0f); 

		if (difficulty <= Difficulty.Easy)
		{
			shuffleCount = 0;
		}

		for (int i = 0; i < shuffleCount; i++)
		{
            int card0Index = Random.Range(0, cards.Length);
			int card1Index = card0Index;
			while (card1Index == card0Index)
			{
				card1Index = Random.Range(0, cards.Length);
			}
			Card card0 = cards[card0Index];
			Card card1 = cards[card1Index];

			var card0TilePosX = card0.tilePositionX;
			var card0TilePosY = card0.tilePositionY;

			card0.tilePositionX = card1.tilePositionX;
			card0.tilePositionY = card1.tilePositionY;

			card1.tilePositionX = card0TilePosX;
			card1.tilePositionY = card0TilePosY;

			cardGrid[card0.tilePositionX, card0.tilePositionY] = card0;
			cardGrid[card1.tilePositionX, card1.tilePositionY] = card1;

			Vector3 card0Position = card0.transform.position;
			Vector3 card1Position = card1.transform.position;
			Vector3 center = (card0Position+card1Position)/2;

			card0.MoveTo(card1Position, center, 0.75f);
			card1.MoveTo(card0Position, center, 0.75f);

			yield return new WaitForSeconds(1.0f);
		}

		gameIsStarted = true;

		GameUIManager.current.OnGameStart();
	}

	void Update() {
		if (gameIsStarted && Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				if (hit.collider) {
					Card hitCard = hit.collider.GetComponent<Card>();
					if (hitCard) {
						TapCard(hitCard);
					}
				}
			}
		}
	}

	void TapCard(Card card) {
		if (!gameIsStarted)
		{
			return;
		}

		if (flipCardCoroutine != null)
		{
			return;
		}

		if (card.isMatched) {
			return;
		}

		if (card.isFlipped) {
			return;
		}

		flipCardCoroutine = StartCoroutine(FlipCard(card));
	}

	IEnumerator FlipCard(Card card) {
		card.isFlipped = true;
		card.Expose(gameStats.currentTurn);

		yield return new WaitForSeconds(0.35f);

		if (card.cardDef is LionCardDef)
		{
			OneShotAudio.Play(clipLion, 0, GameSettings.Audio.sfxVolume);

			var exposedLionCards = GetExposedLionCards();
			if (exposedLionCards.Length >= 2)
			{
				foreach (var exposedCard in exposedLionCards)
				{
					exposedCard.isFlipped = true;
				}

				StartCoroutine(EndGame());
			}
			else
			{
				yield return new WaitForSeconds(0.5f);
				if (flippedCard)
				{
					flippedCard.isFlipped = false;
				}
				card.isFlipped = false;
				yield return new WaitForSeconds(0.5f);
			}
			flippedCard = null;
			gameStats.currentTurn++;
		}
		else
		{
			if (flippedCard)
			{
				if (flippedCard.cardDef == card.cardDef)
				{
					card.OnMatch(flippedCard);
					flippedCard.OnMatch(card);
					currentBackground.hyena.SausageFall();
                    OneShotAudio.Play(clipFruitFall, 0, GameSettings.Audio.sfxVolume);
					gameStats.matches++;
                }
                else
				{
                    yield return new WaitForSeconds(0.5f);
					card.isFlipped = false;
					flippedCard.isFlipped = false;
					if (card.exposedTurn < gameStats.currentTurn)
					{
						gameStats.misses++;
					}
				}
				flippedCard = null;

				if (GetMatchedCards().Length >= GetNonLionCards().Length)
				{
					StartCoroutine(EndGame());
				}
				gameStats.currentTurn++;
			}
			else
			{
				flippedCard = card;
			}
		}

		flipCardCoroutine = null;
	}

	private void SetupBackground()
	{
		if (currentBackground)
		{
			Destroy(currentBackground.gameObject);
		}

		currentBackground = Instantiate(currentLevel.background);
		currentBackground.transform.SetParent(transform, false);

		float deltaAspect = Camera.main.aspect/(1920.0f/1080);

		if (deltaAspect < 1)
		{
			currentBackground.transform.Translate((1.0f-deltaAspect)*backgroundAspectRationShiftAmount, 0, 0);
		}
	}

	IEnumerator EndGame() {
		gameIsStarted = false;
		var didWin = GetExposedLionCards().Length < 2;

		yield return new WaitForSeconds(1);

		if (!didWin || gameStats.totalScore <= 0)
		{
			currentBackground.hyena.Laugh();
		}

		yield return new WaitForSeconds(1.5f);

		UpdateStats(didWin);

		GameUIManager.current.ShowEndScreen();
	}

	private void UpdateStats(bool didWin)
	{
		if (!string.IsNullOrEmpty(currentLevel.identifier))
		{
			var levelStats = GameData.GetLevelStats(currentLevel.identifier);

			ref var modeStats = ref (includeLions ? ref levelStats.lionStats : ref levelStats.normalStats);
			
			if (didWin)
			{
				modeStats.beat = true;
				modeStats.bestScore = Mathf.Max(gameStats.totalScore, modeStats.bestScore);
			}
			modeStats.playCount++;

			GameData.SetLevelStats(currentLevel.identifier, levelStats);
		}
	}

	void ClearCards() {
		foreach (Card card in GetAllCards()) {
			Destroy(card.gameObject);
		}
	}

	public Card[] GetMatchedCards() {
		List<Card> matchedCards = new List<Card>();
		foreach (Card card in GetAllCards()) {
			if (card.isMatched) {
				matchedCards.Add(card);
			}
		}
		return matchedCards.ToArray();
	}

	public Card[] GetAllCards() {
		List<Card> allCards = new List<Card>();
		if (cardGrid == null) {
			return allCards.ToArray();
		}
		for (int y = 0; y < currentLevel.cardCountY; y++) {
			for (int x = 0; x < currentLevel.cardCountX; x++) {
				allCards.Add(cardGrid[x,y]);
			}
		}
		return allCards.ToArray();
	}

	public Card[] GetNonLionCards()
	{
		List<Card> cards = new List<Card>();
		for (int y = 0; y < currentLevel.cardCountY; y++)
		{
			for (int x = 0; x < currentLevel.cardCountX; x++)
			{
				var card = cardGrid[x, y];
				if (!(card.cardDef is LionCardDef))
				{
					cards.Add(card);
				}
			}
		}
		return cards.ToArray();
	}

	public Card[] GetExposedLionCards()
	{
		var cards = new List<Card>();
		for (int y = 0; y < currentLevel.cardCountY; y++)
		{
			for (int x = 0; x < currentLevel.cardCountX; x++)
			{
				var card = cardGrid[x, y];
				if (card.cardDef is LionCardDef && card.hasBeenExposed)
				{
                    cards.Add(cardGrid[x, y]);
				}
			}
		}
		return cards.ToArray();
	}

	(Vector3 position, Vector3 scale) GetCardGridTransformation(int x, int y) {
		float targetAspect = 16.0f/9;
		float deltaAspect = Mathf.Clamp01(Camera.main.aspect/targetAspect);

		var safeBox = new Vector2(cardSafeBounds*deltaAspect, cardSafeBounds);

		float totalWidth = currentLevel.cardCountX*cardSpacing;
		float totalHeight = currentLevel.cardCountY*cardSpacing;
		float scaler = Mathf.Min(safeBox.x/totalWidth, safeBox.y/totalHeight);

		Vector3 pos = new Vector3(x+0.5f,y+0.5f,0)*cardSpacing;
		pos.x -= totalWidth/2.0f;
		pos.y -= totalHeight/2.0f;
		pos *= scaler;

		return (pos, Vector3.one*scaler);
	}

	public IEnumerable<Card> GetAdjacentCards(int x, int y)
	{
		if (PositionIsValid(x-1, y))
		{
			yield return cardGrid[x-1, y];
		}
		if (PositionIsValid(x+1, y))
		{
			yield return cardGrid[x+1, y];
		}
		if (PositionIsValid(x, y-1))
		{
			yield return cardGrid[x, y-1];
		}
		if (PositionIsValid(x, y+1))
		{
			yield return cardGrid[x, y+1];
		}
	}

	public bool PositionIsValid(int x, int y)
	{
		return x >= 0 && x < currentLevel.cardCountX && y >= 0 && y < currentLevel.cardCountY;
	}

	Card CreateCardInstance(CardDef cardDef) {
		Card card = Instantiate(cardPrefab);
		card.name = cardDef.name;
		card.cardDef = cardDef;

		MaterialPropertyBlock block = new MaterialPropertyBlock();

		if (currentLevel.timeOfDay)
		{
			foreach (Renderer r in card.gameObject.GetComponentsInChildren<Renderer>())
			{
				r.GetPropertyBlock(block);
				block.SetColor("_Color", currentLevel.timeOfDay.cardTint);
				r.SetPropertyBlock(block);
			}
		}

		GameObject meshObject = Instantiate(cardDef.meshPrefab);
		meshObject.transform.parent = card.transform.Find("RotationPivot/AnimalPivot");
		meshObject.transform.localPosition = Vector3.zero;
		meshObject.transform.localRotation = Quaternion.identity;
		meshObject.transform.localScale = Vector3.one*cardDef.animalScaler;

		card.animalAnimator.runtimeAnimatorController = cardDef.animatorController;
		card.animalAnimator.applyRootMotion = false;

		Renderer[] renderers = meshObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers) {
			r.sharedMaterial = cardDef.material;

			if (currentLevel.timeOfDay)
			{
				r.GetPropertyBlock(block);
				block.SetColor("_Color", currentLevel.timeOfDay.cardTint);
				r.SetPropertyBlock(block);
			}
		}

		return card;
	}
}
