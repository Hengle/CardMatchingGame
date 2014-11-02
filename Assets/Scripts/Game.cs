using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Game:MonoBehaviour {
	public GameUIManager uiManger;

	public Card cardPrefab;
	public float cardSpacing = 1.25f;
	public int cardCountX = 4;
	public int cardCountY = 4;

	public int maxFails = 10;
	public int failCount = 0;

	private Card[,] cardGrid = null;
	private Card flippedCard = null;

	private bool isFlipAnimating = false;

	void Start() {
		SetupNewGame();
	}

	void SetupNewGame() {
		isFlipAnimating = false;
		failCount = 0;
		flippedCard = null;

		ClearCards();
		cardGrid = new Card[cardCountX, cardCountY];

		uiManger.failedTrysCounter.text = (maxFails-failCount).ToString();
		
		CardDef[] cardDefs = GetCardDefs();
		
		List<Vector2> availableCardSlots = new List<Vector2>();
		
		for (int x = 0; x < cardCountX; x++) {
			for (int y = 0; y < cardCountY; y++) {
				availableCardSlots.Add(new Vector2(x, y));
			}
		}
		
		for (int x = 0; x < cardCountX; x++) {
			for (int y = 0; y < cardCountY; y++) {
				
			}
		}
		Debug.Log(cardDefs.Length);
		while(availableCardSlots.Count > 0) {
			CardDef carddef = cardDefs[Random.Range(0, cardDefs.Length)];
			
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
		isFlipAnimating = true;
		card.isFlipped = true;

		yield return new WaitForSeconds(1);

		if (flippedCard) {
			if (flippedCard.cardDef == card.cardDef) {
				//Its a match
				flippedCard.isMatched = true;
				card.isMatched = true;
			}
			else {
				card.isFlipped = false;
				flippedCard.isFlipped = false;
				failCount++;
			}
			flippedCard = null;

			uiManger.failedTrysCounter.text = (maxFails-failCount).ToString();

			Debug.Log("MatchedCards "+GetMatchedCards().Length+"/"+GetAllCards().Length);
			if (GetMatchedCards().Length == GetAllCards().Length) {
				StartCoroutine(GameWin());
			}
			else {
				if (failCount >= maxFails) {
					StartCoroutine(GameOver());
				}
			}
		}
		else {
			flippedCard = card;
		}
		isFlipAnimating = false;
	}

	IEnumerator GameWin() {
		Debug.Log("You Win!");

		Card[] cards = GetAllCards();

		for (int i = 0; i < cards.Length; i++) {
			cards[i].isFlipped = false;
		}

		yield return new WaitForSeconds(1);

		SetupNewGame();
	}

	IEnumerator GameOver() {
		Debug.Log("You Lose!");

		Card[] cards = GetAllCards();
		
		for (int i = 0; i < cards.Length; i++) {
			cards[i].isFlipped = false;
		}
		
		yield return new WaitForSeconds(1);

		SetupNewGame();
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
		for (int x = 0; x < cardCountX; x++) {
			for (int y = 0; y < cardCountY; y++) {
				allCards.Add(cardGrid[x,y]);
			}
		}
		return allCards.ToArray();
	}

	Vector3 GetCardGridPosition(int x, int y) {
		float totalWidth = cardCountX*cardSpacing-cardSpacing;
		float totalHeight = cardCountY*cardSpacing-cardSpacing;
		Vector3 pos = new Vector3(x,y,0)*cardSpacing;
		pos.x -= totalWidth/2;
		pos.y -= totalHeight/2;
		return pos;
	}

	Card CreateCardInstance(CardDef cardDef) {
		Card card = (Card)Instantiate(cardPrefab);
		card.name = cardDef.name;
		card.cardDef = cardDef;

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

	CardDef[] GetCardDefs() {
		return Resources.LoadAll<CardDef>("Prefabs/CardDefs");
	}
}
