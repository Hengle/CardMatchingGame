using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;

public class CardDefGroup:MonoBehaviour
{
	public List<CardDef> cardDefs = new List<CardDef>();

	public CardDef GetRandomCard()
	{
		if (cardDefs.Count <= 0)
		{
			return null;
		}
		return cardDefs[Random.Range(0, cardDefs.Count)];
	}
}
