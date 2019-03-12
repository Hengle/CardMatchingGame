using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeInitializer : MonoBehaviour
{
	private void Awake()
	{
		GameSettings.Audio.InitializeMasterVolume();
	}
}
