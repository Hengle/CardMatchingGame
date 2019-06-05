using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LionCardDef:CardDef
{
	public Sprite strikeSprite;

	public override void OnExposed(Card card)
	{
		base.OnExposed(card);

		card.backSpriteRenderer.sprite = strikeSprite;
	}
}
