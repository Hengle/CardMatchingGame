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

	//This difficulty stuff should probably exist somewhere else?
	//Im not sure what the UI for this is going to be.
	public enum Difficulty { Easy, Medium, Hard };
	public Difficulty difficulty = Difficulty.Medium;

	public Level testLevel;
	public Background currentBackground;
	public Card cardPrefab;
	public float cardSpacing = 1.25f;
	public float cardScaling = 1.0f;
    public AudioClip clip;
    public AudioClip clipStart;
    public AudioClip clipFruitFall;
    public AudioClip clipLion;

	private int _failCount = 0;
	public int failCount
	{
		get
		{
			return _failCount;
		}
		set
		{
			_failCount = value;
			GameUIManager.current.UpdateFailCounter();
		}
	}

	public Level currentLevel {get; private set;}

	private Card[,] cardGrid = null;
	private Card flippedCard = null;
	private bool gameIsStarted = false;

	void Awake() {
		currentLevel = testLevel;
		if (LevelSelect.currentlySelectedLevelTemplate)
		{
			currentLevel = LevelSelect.currentlySelectedLevelTemplate;
		}
    }

	void Start()
	{
		SetupNewGame();
	}

	void SetupNewGame() {
		failCount = 0;
		flippedCard = null;
		gameIsStarted = false;

		if (currentBackground)
		{
			Destroy(currentBackground.gameObject);
		}

		currentBackground = Instantiate(currentLevel.background);
		currentBackground.transform.SetParent(transform, false);

		ClearCards();
		cardGrid = new Card[currentLevel.cardCountX, currentLevel.cardCountY];
		
		List<Vector2Int> availableCardSlots = new List<Vector2Int>();
		
		for (int x = 0; x < currentLevel.cardCountX; x++) {
			for (int y = 0; y < currentLevel.cardCountY; y++) {
				availableCardSlots.Add(new Vector2Int(x, y));
			}
		}

		int counter = 0;
		while (availableCardSlots.Count > 0 && counter < currentLevel.lionCardDefs.Count)
		{
			CardDef cardDef = currentLevel.lionCardDefs[counter];
			AddCard(availableCardSlots, cardDef);
			counter++;
		}

		counter = 0;

		//TODO: Remove Level.cardDefs path when all data has been migrated.
		List<CardDef> shuffledCardDefs = null;
		if (currentLevel.cardDefGroups.Count > 0)
		{
			shuffledCardDefs = new List<CardDef>();
			foreach (var cardDefGroup in currentLevel.cardDefGroups)
			{
				shuffledCardDefs.Add(cardDefGroup.GetRandomCard());
			}
		}
		else
		{
			shuffledCardDefs = new List<CardDef>(currentLevel.cardDefs);
		}
	

        Shuffle(shuffledCardDefs);

		while (availableCardSlots.Count > 0) {
			CardDef cardDef = shuffledCardDefs[counter%shuffledCardDefs.Count];
			if (!cardDef || Random.value >= cardDef.probability)
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

		card0.transform.position = GetCardGridPosition(card0.tilePositionX, card0.tilePositionY);
		card1.transform.position = GetCardGridPosition(card1.tilePositionX, card1.tilePositionY);

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
                OneShotAudio.Play(clipStart, 0, GameSettings.Audio.sfxVolume);

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
                OneShotAudio.Play(clip, 0, GameSettings.Audio.sfxVolume);

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
		if (card.isMatched) {
			return;
		}

		if (card.isFlipped) {
			return;
		}

		StartCoroutine(FlipCard(card));
	}

	IEnumerator FlipCard(Card card) {
		card.isFlipped = true;
		
		yield return new WaitForSeconds(0.35f);

		if (card.cardDef is LionCardDef)
		{
			if (flippedCard)
			{
				flippedCard.isFlipped = false;
			}
			var flippedLionCards = GetFlippedLionCards();
			if (flippedLionCards.Length >= 2)
			{
				StartCoroutine(EndGame(false));
			}
			currentBackground.hyena.Laugh();
			flippedCard = null;
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
                }
                else
				{
                    yield return new WaitForSeconds(0.5f);
					card.isFlipped = false;
					flippedCard.isFlipped = false;
					failCount++;
					if (failCount > currentLevel.maxFailCount)
					{
						StartCoroutine(EndGame(!(failCount > currentLevel.maxFailCount)));
					}
					currentBackground.hyena.Laugh();
				}
				flippedCard = null;

				if (GetMatchedCards().Length >= GetNonLionCards().Length)
				{
					StartCoroutine(EndGame(!(failCount > currentLevel.maxFailCount)));
				}
			}
			else
			{
				flippedCard = card;
			}
		}
	}

	IEnumerator EndGame(bool didWin) {
		CanvasGroup endGameGroup = Instantiate(Resources.Load<CanvasGroup>("UI/EndGameUI"));
		endGameGroup.transform.SetParent(GameUIManager.current.canvas.transform, false);

		Text endGameText = endGameGroup.transform.Find("Text").gameObject.GetComponent<Text>();
		endGameText.text = "You\n"+(didWin ? "Win" : "Lose");

		endGameGroup.alpha = 0;
		
		while (endGameGroup.alpha < 1)
		{
			endGameGroup.alpha += Time.deltaTime*2;
			yield return 0;
		}
		endGameGroup.alpha = 1;

		yield return new WaitForSeconds(2.0f);

		if (!didWin)
		{
			SetupNewGame();
		}

		Card[] cards = GetAllCards();

		for (int i = 0; i < cards.Length; i++) {
			cards[i].isFlipped = false;
		}

		//LeanTween.value(endGameGroup.gameObject, (v) => { endGameGroup.alpha = v; }, 1.0f, 0.0f, 0.5f).setEase(LeanTweenType.easeOutCubic);
		endGameGroup.alpha = 0;
		while (endGameGroup.alpha > 0)
		{
			endGameGroup.alpha -= Time.deltaTime*2;
			yield return 0;
		}
		endGameGroup.alpha = 0;
		
		Destroy(endGameGroup.gameObject);
		
		if (didWin)
		{
			SceneManager.LoadScene("LevelSelect");
		}
	}
	
	void ClearCards() {
		foreach (Card card in GetAllCards()) {
			Destroy(card.gameObject);
		}
	}

	Card[] GetMatchedCards() {
		List<Card> matchedCards = new List<Card>();
		foreach (Card card in GetAllCards()) {
			if (card.isMatched) {
				matchedCards.Add(card);
			}
		}
		return matchedCards.ToArray();
	}

	Card[] GetAllCards() {
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

	Card[] GetNonLionCards()
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

	Card[] GetFlippedLionCards()
	{
		var cards = new List<Card>();
		for (int y = 0; y < currentLevel.cardCountY; y++)
		{
			for (int x = 0; x < currentLevel.cardCountX; x++)
			{
				var card = cardGrid[x, y];
				if (card.cardDef is LionCardDef && card.isFlipped)
				{
                    OneShotAudio.Play(clipLion, 0, GameSettings.Audio.sfxVolume);
                    cards.Add(cardGrid[x, y]);
				}
			}
		}
		return cards.ToArray();
	}

	float GetCardScaler() {
		float scaler = 1.0f/Mathf.Max(currentLevel.cardCountX/4.0f, currentLevel.cardCountY/4.0f);
		scaler *= cardScaling;
		return scaler;
	}

	Vector3 GetCardGridPosition(int x, int y) {
		float scaler = GetCardScaler();
		float totalWidth = currentLevel.cardCountX*cardSpacing-cardSpacing;
		float totalHeight = currentLevel.cardCountY*cardSpacing-cardSpacing;
		Vector3 pos = new Vector3(x,y,0)*cardSpacing;
		pos.x -= totalWidth/2.0f;
		pos.y -= totalHeight/2.0f;
		pos *= scaler;

		return pos;
	}

	Card CreateCardInstance(CardDef cardDef) {
		Card card = Instantiate(cardPrefab);
		card.name = cardDef.name;
		card.cardDef = cardDef;

		card.transform.localScale = Vector3.one*GetCardScaler();

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
