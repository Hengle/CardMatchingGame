﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level:MonoBehaviour
{
	public List<CardDef> cardDefs = new List<CardDef>();
	public int cardCountX = 4;
	public int cardCountY = 4;
	public int maxFailCount = 3;
	public Background background;
}
