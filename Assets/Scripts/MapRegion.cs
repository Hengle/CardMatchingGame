using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeenTween;

public class MapRegion:MonoBehaviour
{
	public string regionName = "Region";
	public float popupScale = 5;
	public Color selectionColor = Color.white;
	public List<Level> levels = new List<Level>();

    public AudioClip acSelectAudio;
    public AudioClip acDeselectAudio;
    public AudioSource asSelectSource;

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
	private Color originalMaterialColor;
	Tween tween;

	private void Start()
	{
		meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
		originalMaterialColor = meshRenderer.material.color;
	}

	public void OnLevelOverlayOpened(LevelOverlay levelOverlay)
	{
        asSelectSource.clip = acSelectAudio;
        asSelectSource.Play();

		if (tween != null && !tween.isDone)
		{
			tween.Cancel();
		}
		tween = new Tween(null, popupValue, 1, 1.0f, new CurveElastic(TweenCurveMode.Out), UpdateTween);
	}

	public void OnLevelOverlayClosed(LevelOverlay levelOverlay)
	{
        asSelectSource.clip = acDeselectAudio;
        asSelectSource.Play();

		if (tween != null && !tween.isDone)
		{
			tween.Cancel();
		}
		tween = new Tween(null, popupValue, 0, 1.0f, new CurveBounce(TweenCurveMode.Out), UpdateTween);
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

		meshRenderer.material.color = Color.Lerp(originalMaterialColor, selectionColor, t.currentValue);
	}
}
