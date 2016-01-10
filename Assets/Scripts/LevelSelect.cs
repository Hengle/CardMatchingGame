using UnityEngine;
using System.Collections;

public class LevelSelect:MonoBehaviour
{
	static public Level currentlySelectedLevelTemplate;

	public Canvas levelOverlayCanvas;
	public LevelOverlay levelOverlayTemplate;
	public LevelOverlay levelOverlay { get; private set; }

	void Start()
	{
		currentlySelectedLevelTemplate = null;
    }

	public void OnClickRegion(MapRegion region)
	{
		CreateLevelOverlay(region);
    }

	private void CreateLevelOverlay(MapRegion mapRegion)
	{
		if (levelOverlay)
		{
			Destroy(levelOverlay.gameObject);
        }

		levelOverlay = Instantiate(levelOverlayTemplate);
		levelOverlay.transform.SetParent(levelOverlayCanvas.transform, false);
		levelOverlay.mapRegion = mapRegion;
    }
}
