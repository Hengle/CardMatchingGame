using System.Collections;
using System.Collections.Generic;
using KeenTween;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
	public float animationLength = 0.5f;
	public delegate void OnMidTransitionDelegate();
	public event OnMidTransitionDelegate onMidTransition;
	public Image imageTop;
	public Image imageBottom;

	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		RectTransform rtTop = imageTop.GetComponent<RectTransform>();
		RectTransform rtBottom = imageBottom.GetComponent<RectTransform>();

		rtTop.anchorMin = new Vector2(0, 1);
		rtTop.anchorMax = new Vector2(1, 1);
		rtBottom.anchorMin = new Vector2(0, 0);
		rtBottom.anchorMax = new Vector2(1, 0);

		Tween tween0 = new Tween(null, 0, 0.5f, animationLength, new CurveCubic(TweenCurveMode.Out), (t) =>
		{
			rtTop.anchorMin = new Vector2(0, 1.0f-t.currentValue);
			rtBottom.anchorMax = new Vector2(1, t.currentValue);
		});
		tween0.onFinish += (t) => {
			if (onMidTransition != null)
			{
				onMidTransition();
			}
		};

		Tween tween1 = new Tween(tween0, 0.5f, 0, animationLength, new CurveCubic(TweenCurveMode.In), (t) =>
		{
			rtTop.anchorMin = new Vector2(0, 1.0f-t.currentValue);
			rtBottom.anchorMax = new Vector2(1, t.currentValue);
		});

		tween1.onFinish += (t) =>
		{
			Destroy(gameObject);
		};
		//tween1.delay = 0.25f;
	}

	public static Transition CreateTransition()
	{
		return Instantiate(Resources.Load<Transition>("UI/TransitionCanvas"));
	}
}
