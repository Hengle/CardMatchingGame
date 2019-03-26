using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BuildUtility
{
	public static void UpdatePlayerBuildInfo(BuildInfo buildInfo)
	{
		PlayerSettings.bundleVersion = buildInfo.versionString;
		PlayerSettings.macOS.buildNumber = buildInfo.build.ToString();
		PlayerSettings.Android.bundleVersionCode = buildInfo.build;
		PlayerSettings.iOS.buildNumber = buildInfo.build.ToString();
	}

	public static void MakeBuild(BuildTarget target, BuildInfo buildInfo)
	{
		string targetName = BuildTargetToString(target);

		var scenePaths = new List<string>();
		var buildScenes = EditorBuildSettings.scenes;
		foreach (var scene in buildScenes)
		{
			if (scene.enabled)
			{
				scenePaths.Add(scene.path);
			}
		}

		UpdatePlayerBuildInfo(buildInfo);

		//Useful for itch.io butler utility.
		File.WriteAllText($"Builds/version.txt", buildInfo.versionString);
		
		string path = "";

		switch (target)
		{
			case BuildTarget.StandaloneWindows:
			path = $"{buildInfo.versionString}/AntelopeUp.exe";
			break;
			case BuildTarget.StandaloneOSX:
			path = $"{buildInfo.versionString}/AntelopeUp.app";
			break;
			case BuildTarget.Android:
			path = $"AntelopeUp_{buildInfo.versionString}.apk";
			break;
		}

		BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
		buildPlayerOptions.scenes = scenePaths.ToArray();
		buildPlayerOptions.locationPathName = $"Builds/{targetName}/{path}";
		buildPlayerOptions.target = target;
		buildPlayerOptions.options = BuildOptions.None;

		BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
		BuildSummary summary = report.summary;

		if (summary.result == BuildResult.Succeeded)
		{
			Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
		}

		if (summary.result == BuildResult.Failed)
		{
			Debug.Log("Build failed");
		}
	}

	private static string BuildTargetToString(BuildTarget target)
	{
		switch (target)
		{
			case BuildTarget.StandaloneOSX:
			return "Mac";
			case BuildTarget.StandaloneWindows64:
			return "Windows64";
			case BuildTarget.StandaloneWindows:
			return "Windows";
			case BuildTarget.iOS:
			return "IOS";
			default:
			return target.ToString();
		}
	}
}
