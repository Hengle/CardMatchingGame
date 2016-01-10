using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapRegion:MonoBehaviour
{
	public List<Level> levels = new List<Level>();

	private LevelSelect _levelSelect;
	public LevelSelect levelSelect
	{
		get
		{
			return _levelSelect ? _levelSelect : _levelSelect = gameObject.GetComponentInParent<LevelSelect>();
		}
	}

	void OnMouseDown()
	{
		if (levelSelect.levelOverlay)
		{
			return;
		}

		levelSelect.OnClickRegion(this);
	}
}
