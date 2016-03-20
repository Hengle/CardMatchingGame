using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapRegion:MonoBehaviour
{
	public string regionName = "Region";
	public List<Level> levels = new List<Level>();

	private LTDescr lastTween;

	private LevelSelect _levelSelect;
	public LevelSelect levelSelect
	{
		get
		{
			return _levelSelect ? _levelSelect : _levelSelect = gameObject.GetComponentInParent<LevelSelect>();
		}
	}

	private void StopLastTween()
	{
		if (lastTween != null)
		{
			lastTween.cancel();
		}
	}

	public void OnLevelOverlayOpened(LevelOverlay levelOverlay)
	{
		StopLastTween();
		lastTween = LeanTween.moveLocalZ(gameObject, -0.03f, 1.0f).setEase(LeanTweenType.easeOutElastic);
    }

	public void OnLevelOverlayClosed(LevelOverlay levelOverlay)
	{
		StopLastTween();
		lastTween = LeanTween.moveLocalZ(gameObject, 0.0f, 1.0f).setEase(LeanTweenType.easeOutElastic);
	}
}
