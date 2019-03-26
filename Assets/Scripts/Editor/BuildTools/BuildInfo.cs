using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public struct BuildInfo
{
	public int version;
	public int build;
	public string versionString => $"{version}.{build}";

	public const string path = "Assets/Resources/BuildInfo.json";

	public static BuildInfo Load()
	{
		BuildInfo instance = default;
		if (File.Exists(path))
		{
			string json = File.ReadAllText(path);
			instance = JsonUtility.FromJson<BuildInfo>(json);
		}
		return instance;
	}

	public void Save()
	{
		string json = JsonUtility.ToJson(this, true);
		File.WriteAllText(path, json);
		AssetDatabase.ImportAsset(path);
	}
}
