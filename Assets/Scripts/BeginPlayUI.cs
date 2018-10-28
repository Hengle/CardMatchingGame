using System.Collections;
using System.Collections.Generic;
using KeenTween;
using UnityEngine;

public class BeginPlayUI : MonoBehaviour
{
	public Transform goTransform;
	public Transform goCenterTarget;
	public Transform goOffScreenTarget;

	private void Start()
	{

		StartCoroutine(GoAnimation());

	}

	private IEnumerator GoAnimation()
	{
		goTransform.localScale = Vector3.zero;
		goTransform.localPosition = goCenterTarget.localPosition;

		Tween tween = new Tween(null, 0, 1, 1.0f, new CurveElastic(TweenCurveMode.Out), t =>
		{
			if (!goTransform)
			{
				return;
			}
			goTransform.localScale = Vector3.one*t.currentValue;
		});

		while (!tween.isDone)
		{
			yield return 0;
		}

		tween = new Tween(null, 0, 1, 0.5f, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!goTransform)
			{
				return;
			}
			goTransform.localPosition = Vector3.Lerp(goCenterTarget.localPosition, goOffScreenTarget.localPosition, t.currentValue);
		});

		while (!tween.isDone)
		{
			yield return 0;
		}

		gameObject.SetActive(false);
	}
}
