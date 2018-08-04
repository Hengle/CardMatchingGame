using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeenTween;

public class MapRegion:MonoBehaviour
{
	public string regionName = "Region";
	public float popupScale = 5;
	public List<Level> levels = new List<Level>();

    public AudioClip acSelectAudio;
    public AudioClip acDeselectAudio;

	private LevelSelect _levelSelect;
	public LevelSelect levelSelect
	{
		get
		{
			return _levelSelect ? _levelSelect : _levelSelect = gameObject.GetComponentInParent<LevelSelect>();
		}
	}

	private float popupValue = 0;
	private MeshRenderer meshRenderer;
	Tween tween;
	Tween blendTween;

	private void Start()
	{
		meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
	}

	public void OnLevelOverlayOpened(LevelOverlay levelOverlay)
	{
		OneShotAudio.Play(acSelectAudio, 0, GameSettings.Audio.sfxVolume);

		if (tween != null && !tween.isDone)
		{
			tween.Cancel();
		}
		tween = new Tween(null, popupValue, 1, 1.0f, new CurveElastic(TweenCurveMode.Out), UpdateTween);

		if (blendTween != null && !blendTween.isDone) {
			blendTween.Cancel();
		}
		blendTween = new Tween(null, popupValue, 1, 0.5f, new CurveCubic(TweenCurveMode.Out), UpdateBlendTween);
	}

	public void OnLevelOverlayClosed(LevelOverlay levelOverlay)
	{
		OneShotAudio.Play(acDeselectAudio, 0, GameSettings.Audio.sfxVolume);

		if (tween != null && !tween.isDone)
		{
			tween.Cancel();
		}
		tween = new Tween(null, popupValue, 0, 1.0f, new CurveBounce(TweenCurveMode.Out), UpdateTween);

		if (blendTween != null && !blendTween.isDone) {
			blendTween.Cancel();
		}
		blendTween = new Tween(null, popupValue, 0, 0.5f, new CurveCubic(TweenCurveMode.Out), UpdateBlendTween);
		blendTween.delay = 0.25f;
	}

	private void UpdateTween(Tween t)
	{
		if (!this)
		{
			return;
		}
		popupValue = t.currentValue;
		Vector3 scale = transform.localScale;
		scale.y = popupValue*popupScale+1;
		transform.localScale = scale;
	}

	private void UpdateBlendTween(Tween t)
	{
		if (!this)
		{
			return;
		}
		meshRenderer.material.SetFloat("_MainTexBlendFactor", t.currentValue);
	}
}
