using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KeenTween;

public class LevelOverlay:MonoBehaviour
{
	public LevelOverlayItem levelOverlayItemTemplate;
	public RectTransform layoutTransform;
	public Button backgroundButton;
	public Text titleText;

	private bool selectedLevel;

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
			titleText.text = _mapRegion.regionInfo.regionName;
        }
	}
	
	void Start()
	{
		for (int i = 0; i < mapRegion.regionInfo.levels.Count; i++)
		{
			AddLevelItem(mapRegion.regionInfo.levels[i], i+1);
		}

		backgroundButton.onClick.AddListener(OnClickBackgroundButton);

		//transform.localScale = Vector3.zero;
		//LeanTween.scale(gameObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutCubic);

		mapRegion.OnLevelOverlayOpened(this);
	}

	private void OnDisable()
	{
		if (!selectedLevel)
		{
			mapRegion.OnLevelOverlayClosed(this);
		}

		Destroy(gameObject);
	}

	void OnClickBackgroundButton()
	{
		Destroy(gameObject);
	}

	public void OnSelectedLevel(Level level)
	{
		selectedLevel = true;
	}

	void AddLevelItem(Level level, int number)
	{
		LevelOverlayItem levelOverlayItem = Instantiate(levelOverlayItemTemplate);
		levelOverlayItem.transform.SetParent(layoutTransform, false);
		levelOverlayItem.level = level;
		levelOverlayItem.StartAppearAnimation(number);
	}
}
