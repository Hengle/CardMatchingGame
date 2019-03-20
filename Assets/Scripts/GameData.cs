using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
	[System.Serializable]
	public struct LevelStats
	{
		[System.Serializable]
		public struct ModeStats
		{
			public int bestScore;
			public int playCount;
			public bool beat;
		}
		[System.NonSerialized]
		public bool hasData;
		public ModeStats normalStats;
		public ModeStats lionStats;
	}

	private static string LevelIdentifierToKey(string levelIdentifier)
	{
		return "game.levelStats."+levelIdentifier;
	}

	public static LevelStats GetLevelStats(string levelIdentifier)
	{
		var key = LevelIdentifierToKey(levelIdentifier);
		LevelStats levelStats = default;
		bool hasData = false;
		if (PlayerPrefs.HasKey(key))
		{
			try
			{
				levelStats = JsonUtility.FromJson<LevelStats>(PlayerPrefs.GetString(key));
				hasData = true;
			}
			catch (System.Exception)
			{
				Debug.LogError($"Error parsing {nameof(LevelStats)} for {nameof(levelIdentifier)} {levelIdentifier}");
				throw;
			}
		}
		levelStats.hasData = hasData;
		return levelStats;
	}

	public static void SetLevelStats(string levelIdentifier, LevelStats stats)
	{
		var key = LevelIdentifierToKey(levelIdentifier);
		var json = JsonUtility.ToJson(stats);
		PlayerPrefs.SetString(key, json);
		PlayerPrefs.Save();
	}

	public static void RemoveLevelStats(string levelIdentifier)
	{
		var key = LevelIdentifierToKey(levelIdentifier);
		PlayerPrefs.DeleteKey(key);
		PlayerPrefs.Save();
	}

	public static void Clear()
	{
		var levels = Resources.FindObjectsOfTypeAll<Level>();
		foreach (var level in levels)
		{
			if (string.IsNullOrEmpty(level.identifier))
			{
				continue;
			}
			RemoveLevelStats(level.identifier);
		}
	}
}
