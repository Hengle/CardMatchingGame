using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeenTween;

public class BackgroundManager : MonoBehaviour
{
	public float backgroundAspectRationShiftAmount = 3;
	public Background currentBackground { get; private set; }

	public void Setup()
	{
		var game = Game.current;

		if (currentBackground)
		{
			Destroy(currentBackground.gameObject);
		}

		var regionInfo = RegionInfo.GetLevelRegion(game.currentLevel);
		var backgrounds = regionInfo.backgrounds;
		var backgroundTemplate = backgrounds[Random.Range(0, backgrounds.Count)];

		currentBackground = Instantiate(backgroundTemplate);
		currentBackground.transform.SetParent(transform, false);

		float deltaAspect = Camera.main.aspect/(1920.0f/1080);

		if (deltaAspect < 1)
		{
			currentBackground.transform.Translate((1.0f-deltaAspect)*backgroundAspectRationShiftAmount, 0, 0);
		}
	}
}
