using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LionCardDef:CardDef
{
	public override void OnExposed(Card card)
	{
		base.OnExposed(card);
		card.cardBackSpriteRenderer.sprite = card.exposedLionCardSprite;
	}
}
