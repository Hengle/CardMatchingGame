using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelOverlay:MonoBehaviour
{
	public LevelOverlayItem levelOverlayItemTemplate;
	public GridLayoutGroup gridLayout;
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
		if (mapRegion.levels.Count > 4) {
			gridLayout.constraintCount = mapRegion.levels.Count / 2;
		}
		else {
			gridLayout.constraintCount = 4;
		}

		for (int i = 0; i < mapRegion.levels.Count; i++)
		{
			AddLevelItem(mapRegion.levels[i], i+1);
		}

		backgroundButton.onClick.AddListener(OnClickBackgroundButton);

		//transform.localScale = Vector3.zero;
		//LeanTween.scale(gameObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutCubic);

		mapRegion.OnLevelOverlayOpened(this);
	}

	void OnClickBackgroundButton()
	{
		Destroy(gameObject);
	}

	void AddLevelItem(Level level, int number)
	{
		LevelOverlayItem levelOverlayItem = Instantiate(levelOverlayItemTemplate);
		levelOverlayItem.transform.SetParent(gridLayout.transform, false);
		levelOverlayItem.level = level;
		levelOverlayItem.number = number;
		//levelOverlayItem.transform.localScale = Vector3.zero;
		//LeanTween.scale(levelOverlayItem.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.easeOutElastic).setDelay(number*0.1f);
	}

	void OnDestroy()
	{
		if (mapRegion)
		{
			mapRegion.OnLevelOverlayClosed(this);
		}
	}
}
