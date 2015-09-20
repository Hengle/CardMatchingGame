using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Game:MonoBehaviour
{
	static private Game _current;
	static public Game current
	{
		get
		{
			return _current ?? (_current = FindObjectOfType<Game>());
		}
	}

	public GameUIManager uiManger;

	public Background currentBackground;

	public Level[] testLevels;
	private Level testLevel;

	public Card cardPrefab;
	public float cardSpacing = 1.25f;

	private int _rawScore;
	public int rawScore
	{
		get
		{
			return _rawScore;
		}
		set
		{
			_rawScore = value;
			uiManger.scoreText.text = score.ToString();
		}
	}
	public int score
	{
		get
		{
			return (rawScore/(failCount+1));
        }
	}

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
			uiManger.scoreText.text = score.ToString();
		}
	}

	public Level currentLevel {get; private set;}

	private Card[,] cardGrid = null;
	private Card flippedCard = null;

	void Start() {
		testLevel = testLevels[0];
		SetupNewGame(testLevel);
	}

	void SetupNewGame(Level level) {
		currentLevel = level;
		rawScore = 0;
		failCount = 0;
		flippedCard = null;

		if (currentBackground)
		{
			Destroy(currentBackground.gameObject);
		}

		currentBackground = Instantiate(level.background);
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
			
			card0.isFlipped = false;
			card1.isFlipped = false;

			counter += 1;
		}
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
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
				rawScore += 100;

				card.OnMatch(flippedCard);
				flippedCard.OnMatch(card);
			}
			else {
				card.isFlipped = false;
				flippedCard.isFlipped = false;
				failCount++;
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
		bool didWin = score >= currentLevel.targetScore;
		//Debug.Log("You Win!");

		CanvasGroup endGameGroup = Instantiate(Resources.Load<CanvasGroup>("UI/EndGameUI"));
		endGameGroup.transform.SetParent(uiManger.canvas.transform, false);

		Text endGameText = endGameGroup.transform.Find("Text").gameObject.GetComponent<Text>();
		endGameText.text = "You\n"+(didWin?"Win":"Lose");

		endGameGroup.alpha = 0;
		LeanTween.value(endGameGroup.gameObject, (v) => { endGameGroup.alpha = v; }, 0.0f, 1.0f, 0.5f).setEase(LeanTweenType.easeOutCubic);

		yield return new WaitForSeconds(5);

		Card[] cards = GetAllCards();

		for (int i = 0; i < cards.Length; i++) {
			cards[i].isFlipped = false;
		}

		LeanTween.value(endGameGroup.gameObject, (v) => { endGameGroup.alpha = v; }, 1.0f, 0.0f, 0.5f).setEase(LeanTweenType.easeOutCubic);

		yield return new WaitForSeconds(0.5f);

		Destroy(endGameGroup.gameObject);

		if (didWin)
		{
			testLevel = testLevels[(System.Array.IndexOf(testLevels, testLevel)+1)%testLevels.Length];
        }

		SetupNewGame(testLevel);
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
		Card card = (Card)Instantiate(cardPrefab);
		card.name = cardDef.name;
		card.cardDef = cardDef;

		card.transform.localScale = Vector3.one*GetCardScaler();

		GameObject meshObject = (GameObject)Instantiate(cardDef.meshPrefab);
		meshObject.transform.parent = card.transform.Find("AnimalPivot");
		meshObject.transform.localPosition = Vector3.zero;
		meshObject.transform.localRotation = Quaternion.identity;
		meshObject.transform.localScale = Vector3.one;

		card.animator.runtimeAnimatorController = cardDef.animatorController;
		card.animator.applyRootMotion = false;

		Renderer[] renderers = meshObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers) {
			r.sharedMaterial = cardDef.material;
		}

		return card;
	}

	//CardDef[] GetCardDefs() {
	//	return Resources.LoadAll<CardDef>("Prefabs/CardDefs");
	//}
}
