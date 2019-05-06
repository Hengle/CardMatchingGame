using UnityEngine;
using System.Collections;
using KeenTween;
using System.Linq;
using System.Collections.Generic;

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
	public float idlePopDelay = 3;
	public float idlePopRate = 3;

	private Vector3 targetScale;
	private float lastInteractTime;
	private float lastIdlePopTime;

	private MapRegion[] regions;

	private void Awake()
	{
		targetScale = transform.localScale;
	}

	private void OnEnable()
	{
		gameInfo = default;
		transform.localScale = Vector3.zero;

		regions = gameObject.GetComponentsInChildren<MapRegion>();

		new Tween(null, 0, 1, 1.0f, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!this)
			{
				return;
			}

			transform.localScale = targetScale*t.currentValue;
		});

		ResetIdleState();
	}

	private void Start()
	{
		if (!string.IsNullOrEmpty(lastRegionName))
		{
			var region = regions.FirstOrDefault(v => v.regionName == lastRegionName);
			if (region && !region.GetCompleted() && !region.GetLocked())
			{
				OnClickRegion(region);
			}
		}
	}

	void Update()
	{
		if (levelOverlay)
		{
			ResetIdleState();
		}

		if (!levelOverlay && Input.GetMouseButtonDown(0))
		{
			MapRegion region = TryClickRegion(Input.mousePosition);
			if (region)
			{
				OnClickRegion(region);
			}
        }
		else
		{
			if (Time.timeSinceLevelLoad-lastInteractTime > idlePopDelay)
			{
				if (Time.timeSinceLevelLoad-lastIdlePopTime > idlePopRate)
				{
					var region = regions[Random.Range(0, regions.Length)];
					region.DoIdlePop();
					lastIdlePopTime = Time.timeSinceLevelLoad;
				}
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
		if (region.GetLocked())
		{
			return;
		}

		lastRegionName = region.regionName;
		CreateLevelOverlay(region);
		ResetIdleState();
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

	private void ResetIdleState()
	{
		lastInteractTime = Time.timeSinceLevelLoad;
		lastIdlePopTime = 0;
	}
}
