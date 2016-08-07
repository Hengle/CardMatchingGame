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

	private Animator _animator;
	public Animator animator
	{
		get
		{
			return _animator ? _animator : _animator = gameObject.GetComponent<Animator>();
		}
	}

	public void OnLevelOverlayOpened(LevelOverlay levelOverlay)
	{
		animator.CrossFade("MapRegionLift", 0.5f);
	}

	public void OnLevelOverlayClosed(LevelOverlay levelOverlay)
	{
		animator.CrossFade("MapRegionLower", 0.5f);
	}
}
