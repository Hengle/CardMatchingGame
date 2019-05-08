using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
	public enum CompletionState
	{
		None,
		Win,
		Lose
	}
	public CompletionState completionState;

	public delegate void OnStatsChangedDelegate();
	public event OnStatsChangedDelegate onStatsChanged;

	private int _currentTurn;
	public int currentTurn
	{
		get => _currentTurn;
		set
		{
			_currentTurn = value;
			onStatsChanged?.Invoke();
		}
	}

	private int _score = 500;
	public int score
	{
		get => _score;
		set
		{
			_score = value;
			onStatsChanged?.Invoke();
		}
	}

	public int GetTotalScore(Level level, bool includeLions)
	{
		if (completionState == CompletionState.Lose)
		{
			return 0;
		}
		var totalScore = score;
		totalScore += includeLions ? level.lionBonusScore : level.bonusScore;
		return totalScore;
	}
}
