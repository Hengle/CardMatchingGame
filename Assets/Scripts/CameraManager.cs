using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AmplifyColor;

public class CameraManager : MonoBehaviour
{
	public Camera gameCamera;
	public Camera backgroundCamera;

	public AmplifyColorEffect lutEffect;

	private void Start()
	{
		var background = Game.current.backgroundManager.currentBackground;
		if (background.timeOfDay)
		{
			lutEffect.LutTexture = background.timeOfDay.lut;
		}
	}
}
