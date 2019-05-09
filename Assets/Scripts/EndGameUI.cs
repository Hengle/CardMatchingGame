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
	public Text scoreText;
	public Text globalScoreText;

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
		canvasGroup.alpha = 0;
		winRootTransform.gameObject.SetActive(true);

		var game = Game.current;

		var gameScore = Game.current.gameStats.GetTotalScore(game.currentLevel, game.includeLions);
		var newGlobalScore = GameData.GetGlobalStats().score;
		var previousGlobalScore = newGlobalScore-gameScore;

		scoreText.text = gameScore.ToString();
		globalScoreText.text = previousGlobalScore.ToString();

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
			if (!globalScoreText)
			{
				return;
			}
			var score = Mathf.Lerp(previousGlobalScore, newGlobalScore, t.currentValue);
			globalScoreText.text = Mathf.RoundToInt(score).ToString();
		});

		bool wantToSkip = TestSkip();

		while (!wantToSkip && !tween.isDone)
		{
			yield return null;
			wantToSkip |= TestSkip();
		}

		float counter = 0;
		while (!wantToSkip && counter < 5)
		{
			wantToSkip |= TestSkip();
			counter += Time.deltaTime;
			if (!wantToSkip)
			{
				yield return null;
			}
		}

		Transition transition = Transition.CreateTransition();
		transition.onMidTransition += () =>
		{
			SceneManager.LoadScene("LevelSelect");
		};
	}

	private bool TestSkip()
	{
		return Input.GetMouseButton(0);
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
