using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraManager : MonoBehaviour
{
	public Camera gameCamera;
	public Camera backgroundCamera;
	public PostProcessingProfile defaultProfile;

	private Material compositMaterial;
	private PostProcessingProfile profile;

	private void Awake()
	{
		compositMaterial = new Material(Shader.Find("Hidden/CompositCameras"));
		profile = Instantiate(defaultProfile);
		PostProcessingBehaviour ppb = backgroundCamera.gameObject.GetComponent<PostProcessingBehaviour>();
		ppb.profile = profile;
	}

	private void Start()
	{
		UserLutModel.Settings settings = profile.userLut.settings;
		settings.lut = null;
		if (Game.current.currentLevel.timeOfDay)
		{
			settings.lut = Game.current.currentLevel.timeOfDay.lut;
		}
		profile.userLut.settings = settings;
	}

	/*
	RenderTexture backgroundRT;
	private void OnPreCull()
	{
		Vector3 position = backgroundCamera.transform.localPosition;
		Quaternion rotation = backgroundCamera.transform.localRotation;
		int cullingMask = backgroundCamera.cullingMask;
		CameraClearFlags clearFlags = backgroundCamera.clearFlags;

		if (backgroundRT)
		{
			throw new System.Exception("backgroundRT should be null.");
		}

		backgroundRT = RenderTexture.GetTemporary(gameCamera.pixelWidth, gameCamera.pixelHeight, 24, RenderTextureFormat.ARGB32);

		backgroundCamera.targetTexture = backgroundRT;
		backgroundCamera.Render();

		backgroundCamera.transform.localPosition = position;
		backgroundCamera.transform.localRotation = rotation;
		backgroundCamera.cullingMask = cullingMask;
		backgroundCamera.clearFlags = clearFlags;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		compositMaterial.SetTexture("_MainTex2", source);
		Graphics.Blit(backgroundRT, destination, compositMaterial);

		RenderTexture.ReleaseTemporary(backgroundRT);
		backgroundRT = null;
	}
	*/
}
