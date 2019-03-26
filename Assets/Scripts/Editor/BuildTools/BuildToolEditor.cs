using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildToolEditor : EditorWindow
{
	public bool buildWindows
	{
		get
		{
			return EditorPrefs.GetBool("BuildToolEditor.buildWindows", true);
		}
		set
		{
			EditorPrefs.SetBool("BuildToolEditor.buildWindows", value);
		}
	}
	public bool buildMac
	{
		get
		{
			return EditorPrefs.GetBool("BuildToolEditor.buildMac", true);
		}
		set
		{
			EditorPrefs.SetBool("BuildToolEditor.buildMac", value);
		}
	}
	public bool buildAndroid
	{
		get
		{
			return EditorPrefs.GetBool("BuildToolEditor.buildAndroid", true);
		}
		set
		{
			EditorPrefs.SetBool("BuildToolEditor.buildAndroid", value);
		}
	}
	public bool buildIOS
	{
		get
		{
			return EditorPrefs.GetBool("BuildToolEditor.buildIOS", true);
		}
		set
		{
			EditorPrefs.SetBool("BuildToolEditor.buildIOS", value);
		}
	}

	[MenuItem("Build/Build Tool")]
	static void Init()
	{
		BuildToolEditor window = GetWindow<BuildToolEditor>(true, "Build Tool");
		window.Show();
	}

	private void Awake()
	{
		maxSize = minSize = new Vector2(200, 200);
	}

	void OnGUI()
	{
		var buildInfo = BuildInfo.Load();

		EditorGUILayout.HelpBox($"version is {buildInfo.versionString}", MessageType.Info);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Increment Build Number", GUILayout.ExpandWidth(false)))
		{
			buildInfo.build++;
			buildInfo.Save();
			BuildUtility.UpdatePlayerBuildInfo(buildInfo);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		EditorGUILayout.Space();

		buildWindows = GUILayout.Toggle(buildWindows, "Windows");
		buildMac = GUILayout.Toggle(buildMac, "Mac");
		buildAndroid = GUILayout.Toggle(buildAndroid, "Android");
		buildIOS = GUILayout.Toggle(buildIOS, "IOS");

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Make Build(s)", GUILayout.ExpandWidth(false)))
		{
			if (buildWindows)
			{
				BuildUtility.MakeBuild(BuildTarget.StandaloneWindows, buildInfo);
			}
			if (buildMac)
			{
				BuildUtility.MakeBuild(BuildTarget.StandaloneOSX, buildInfo);
			}
			if (buildAndroid)
			{
				BuildUtility.MakeBuild(BuildTarget.Android, buildInfo);
			}
			if (buildIOS)
			{
				BuildUtility.MakeBuild(BuildTarget.iOS, buildInfo);
			}
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
}
