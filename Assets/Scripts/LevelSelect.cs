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

	void Update()
	{
		if (!levelOverlay && Input.GetMouseButtonDown(0))
		{
			MapRegion region = TryClickRegion(Input.mousePosition);
			if (region)
			{
				OnClickRegion(region);
			}
        }
	}

	MapRegion TryClickRegion(Vector2 screenPoint)
	{
		MapRegion region = null;

		Ray ray = Camera.main.ScreenPointToRay(screenPoint);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			region = hit.collider.gameObject.GetComponentInParent<MapRegion>();
		}

		return region;
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
