using System.Collections;
using System.Collections.Generic;
using KeenTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public RectTransform winRootTransform;
	public RectTransform loseRootTransform;
	public Text matchesValueText;
	public Text missesValueText;
	public Text scoreValueText;

	public void Start()
	{
		canvasGroup.alpha = 0;
		winRootTransform.gameObject.SetActive(false);
		loseRootTransform.gameObject.SetActive(false);

		var didWin = Game.current.GetExposedLionCards().Length < 2;
		if (didWin)
		{
			StartCoroutine(RunWinAsync());
		}
		else
		{
			StartCoroutine(RunLoseAsync());
		}
	}

	private IEnumerator RunWinAsync()
	{
		var totalPossibleMatches = Game.current.GetNonLionCards().Length/2;
		matchesValueText.text = Game.current.gameStats.matches+"/"+totalPossibleMatches;

		missesValueText.text = Game.current.gameStats.misses.ToString();

		canvasGroup.alpha = 0;
		winRootTransform.gameObject.SetActive(true);

		var tween = new Tween(null, 0, 1, 0.25f, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!canvasGroup)
			{
				return;
			}
			canvasGroup.alpha = t.currentValue;
		});

		while (!tween.isDone)
		{
			yield return null;
		}

		tween = new Tween(null, 0, 1, 1, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!scoreValueText)
			{
				return;
			}
			scoreValueText.text = Mathf.RoundToInt(t.currentValue*Game.current.gameStats.matches).ToString();
		});

		while (!tween.isDone)
		{
			yield return null;
		}

		tween = new Tween(null, 0, 1, 1, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!scoreValueText)
			{
				return;
			}
			var score = Mathf.Lerp(Game.current.gameStats.matches, Game.current.gameStats.totalScore, t.currentValue);
			scoreValueText.text = Mathf.RoundToInt(score).ToString();
		});

		while (!tween.isDone)
		{
			yield return null;
		}

		yield return new WaitForSeconds(2.0f);

		Transition transition = Transition.CreateTransition();
		transition.onMidTransition += () =>
		{
			SceneManager.LoadScene("LevelSelect");
		};
	}

	private IEnumerator RunLoseAsync()
	{
		canvasGroup.alpha = 0;
		loseRootTransform.gameObject.SetActive(true);

		var tween = new Tween(null, 0, 1, 0.25f, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!canvasGroup)
			{
				return;
			}
			canvasGroup.alpha = t.currentValue;
		});

		while (!tween.isDone)
		{
			yield return null;
		}
		
		yield return new WaitForSeconds(2.0f);

		Transition transition = Transition.CreateTransition();
		transition.onMidTransition += () =>
		{
			SceneManager.LoadScene("Game");
		};
	}
}
