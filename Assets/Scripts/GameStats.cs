using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
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

	private int _matches;
	public int matches
	{
		get => _matches;
		set
		{
			_matches = value;
			onStatsChanged?.Invoke();
		}
	}

	private int _misses;
	public int misses
	{
		get => _misses;
		set
		{
			_misses = value;
			onStatsChanged?.Invoke();
		}
	}

	public int totalScore => Mathf.Max(matches-misses, 0);
}
