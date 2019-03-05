using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : MonoBehaviour
{
	public Camera gameCamera;
	public Camera backgroundCamera;

	public PostProcessProfile defaultProfile;

	//private PostProcessProfile profile;
	PostProcessVolume postProcessVolume;

	private void Awake()
	{
		//compositMaterial = new Material(Shader.Find("Hidden/CompositCameras"));

		postProcessVolume = backgroundCamera.gameObject.GetComponent<PostProcessVolume>();
		postProcessVolume.profile = Instantiate(defaultProfile);
	}

	private void Start()
	{
		if (Game.current.currentLevel.timeOfDay)
		{
			ColorGrading colorGrading;
			postProcessVolume.profile.TryGetSettings(out colorGrading);
			if (colorGrading != null)
			{
				colorGrading.ldrLut.value = Game.current.currentLevel.timeOfDay.lut;
			}
		}
	}
}
