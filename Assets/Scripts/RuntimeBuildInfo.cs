using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public struct RuntimeBuildInfo
{
	public int version;
	public int build;
	public string versionString => $"{version}.{build}";

	public const string path = "BuildInfo";

	public static RuntimeBuildInfo Load()
	{
		var instance = JsonUtility.FromJson<RuntimeBuildInfo>(Resources.Load<TextAsset>(path).text);
		return instance;
	}
}
