using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelOverlay:MonoBehaviour
{
	public LevelOverlayItem levelOverlayItemTemplate;
	public Transform itemParent;
	public Button backgroundButton;

	[HideInInspector]
	public MapRegion mapRegion;
	
	void Start()
	{
		foreach (var level in mapRegion.levels)
		{
			AddLevelItem(level);
		}

		backgroundButton.onClick.AddListener(OnClickBackgroundButton);
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
}
