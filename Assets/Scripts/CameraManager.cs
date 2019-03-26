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
		if (Game.current.currentLevel.timeOfDay)
		{
			lutEffect.LutTexture = Game.current.currentLevel.timeOfDay.lut;
		}
	}
}
