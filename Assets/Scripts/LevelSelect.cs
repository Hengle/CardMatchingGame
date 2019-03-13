using UnityEngine;
using System.Collections;
using KeenTween;
using System.Linq;

public class LevelSelect:MonoBehaviour
{
	public struct GameInfo
	{
		public Level selectedLevel;
		public bool lionMode;
	}

	public static GameInfo gameInfo;

	public Canvas levelOverlayCanvas;
	public LevelOverlay levelOverlayTemplate;
	public LevelOverlay levelOverlay { get; private set; }
	private static string lastRegionName = "";

	private Vector3 targetScale;

	private void Awake()
	{
		targetScale = transform.localScale;
	}

	private void OnEnable()
	{
		gameInfo = default;
		transform.localScale = Vector3.zero;

		new Tween(null, 0, 1, 1.0f, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!this)
			{
				return;
			}

			transform.localScale = targetScale*t.currentValue;
		});
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(lastRegionName))
		{
			var regions = gameObject.GetComponentsInChildren<MapRegion>();
			var region = regions.FirstOrDefault(v => v.regionName == lastRegionName);
			Debug.Log(lastRegionName);
			if (region)
			{
				OnClickRegion(region);
			}
		}
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
		lastRegionName = region.regionName;
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
