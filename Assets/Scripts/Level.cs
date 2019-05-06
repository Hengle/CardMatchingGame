using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level:MonoBehaviour
{
	public string identifier = "";
	public List<CardDefGroup> cardDefGroups = new List<CardDefGroup>();
	public List<LionCardDef> lionCardDefs = new List<LionCardDef>();

	public Game.GameMode gameMode = Game.GameMode.Standard;

	public int cardCountX = 4;
	public int cardCountY = 4;
	public bool useConservation;

	public Background background;

	public Sprite thumbnail;
	public Sprite lionThumbnail;

	public TimeOfDay timeOfDay;
}
