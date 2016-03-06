using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapRegion:MonoBehaviour
{
	public string regionName = "Region";
	public List<Level> levels = new List<Level>();

	private LevelSelect _levelSelect;
	public LevelSelect levelSelect
	{
		get
		{
			return _levelSelect ? _levelSelect : _levelSelect = gameObject.GetComponentInParent<LevelSelect>();
		}
	}

	public void OnLevelOverlayOpened(LevelOverlay levelOverlay)
	{
		LeanTween.moveLocalZ(gameObject, -0.03f, 1.0f).setEase(LeanTweenType.easeOutElastic);
	}

	public void OnLevelOverlayClosed(LevelOverlay levelOverlay)
	{
		LeanTween.moveLocalZ(gameObject, 0.0f, 1.0f).setEase(LeanTweenType.easeOutElastic);
	}
}
