using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level:MonoBehaviour
{
	public List<CardDef> cardDefs = new List<CardDef>();
	public List<CardDefGroup> cardDefGroups = new List<CardDefGroup>();
	public List<LionCardDef> lionCardDefs = new List<LionCardDef>();
	public int cardCountX = 4;
	public int cardCountY = 4;
	public int maxFailCount = 3;
	public Background background;
	public Sprite thumbnail;
	public TimeOfDay timeOfDay;
}
