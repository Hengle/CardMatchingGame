using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
		
		List<Vector2> availableCardSlots = new List<Vector2>();
		
		for (int x = 0; x < currentLevel.cardCountX; x++) {
			for (int y = 0; y < currentLevel.cardCountY; y++) {
				availableCardSlots.Add(new Vector2(x, y));
			}
		}

		int counter = 0;
		while(availableCardSlots.Count > 0) {
			CardDef carddef = currentLevel.cardDefs[counter%currentLevel.cardDefs.Count];
			
			int slot0Index = Random.Range(0, availableCardSlots.Count);
			Vector2 slot0 = availableCardSlots[slot0Index];
			availableCardSlots.RemoveAt(slot0Index);
			
			int slot1Index = Random.Range(0, availableCardSlots.Count);
			Vector2 slot1 = availableCardSlots[slot1Index];
			availableCardSlots.RemoveAt(slot1Index);
			
			Card card0 = CreateCardInstance(carddef);
			Card card1 = CreateCardInstance(carddef);
			
			card0.tilePositionX = (int)slot0.x;
			card0.tilePositionY = (int)slot0.y;
			
			card1.tilePositionX = (int)slot1.x;
			card1.tilePositionY = (int)slot1.y;
			
			card0.transform.position = GetCardGridPosition(card0.tilePositionX, card0.tilePositionY);
			card1.transform.position = GetCardGridPosition(card1.tilePositionX, card1.tilePositionY);
			
			cardGrid[card0.tilePositionX, card0.tilePositionY] = card0;
			cardGrid[card1.tilePositionX, card1.tilePositionY] = card1;
			
			card0.isFlipped = true;
			card1.isFlipped = true;

			counter += 1;
		}

		StartCoroutine(PreGameSequence());
	}

	IEnumerator PreGameSequence()
	{
		yield return new WaitForSeconds(3);

		Card[] cards = GetAllCards();

		foreach (var card in cards)
		{
			card.isFlipped = false;
		}

		yield return new WaitForSeconds(1.0f);

		int shuffleCount = cards.Length/2;

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
		if (card == flippedCard) {
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

		if (flippedCard) {
			if (flippedCard.cardDef == card.cardDef) {
				card.OnMatch(flippedCard);
				flippedCard.OnMatch(card);
				currentBackground.hyena.SausageFall();
			}
			else {
				card.isFlipped = false;
				flippedCard.isFlipped = false;
				failCount++;
				if (failCount > currentLevel.maxFailCount)
				{
					StartCoroutine(EndGame());
				}
				currentBackground.hyena.Laugh();
			}
			flippedCard = null;
			
			if (GetMatchedCards().Length == GetAllCards().Length) {
				StartCoroutine(EndGame());
			}
		}
		else {
			flippedCard = card;
		}
	}

	IEnumerator EndGame() {
		bool didWin = !(failCount > currentLevel.maxFailCount);
		//Debug.Log("You Win!");

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
		for (int x = 0; x < currentLevel.cardCountX; x++) {
			for (int y = 0; y < currentLevel.cardCountY; y++) {
				allCards.Add(cardGrid[x,y]);
			}
		}
		return allCards.ToArray();
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
		}

		return card;
	}
}
