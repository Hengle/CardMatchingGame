using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelOverlay:MonoBehaviour
{
	public LevelOverlayItem levelOverlayItemTemplate;
	public Transform itemParent;
	public Button backgroundButton;
	public Text titleText;

	private MapRegion _mapRegion;
	public MapRegion mapRegion
	{
		get
		{
			return _mapRegion;
		}
		set
		{
			_mapRegion = value;
			titleText.text = _mapRegion.regionName;
        }
	}
	
	void Start()
	{
		foreach (var level in mapRegion.levels)
		{
			AddLevelItem(level);
		}

		backgroundButton.onClick.AddListener(OnClickBackgroundButton);

		transform.localScale = Vector3.zero;
		LeanTween.scale(gameObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutCubic);

		mapRegion.OnLevelOverlayOpened(this);
	}

	void OnClickBackgroundButton()
	{
		Destroy(gameObject);
	}

	void AddLevelItem(Level level)
	{
		LevelOverlayItem levelOverlayItem = Instantiate(levelOverlayItemTemplate);
		levelOverlayItem.transform.SetParent(itemParent, false);
		levelOverlayItem.level = level;
	}

	void OnDestroy()
	{
		if (mapRegion)
		{
			mapRegion.OnLevelOverlayClosed(this);
		}
	}
}
