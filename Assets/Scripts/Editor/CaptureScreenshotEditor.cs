using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CaptureScreenshotEditor : EditorWindow
{
	public int outputWidth => Camera.main ? Camera.main.pixelWidth : 0;
	public int outputHeight => Camera.main ? Camera.main.pixelHeight : 0;


	[MenuItem("Tools/Capture Screenshot")]
	static void Init()
	{
		CaptureScreenshotEditor window = GetWindow<CaptureScreenshotEditor>(true, "Capture Screenshot");
		window.Show();
	}

	private void Awake()
	{
		maxSize = minSize = new Vector2(300, 100);
	}

	private void Update()
	{
		
	}

	void OnGUI()
	{
		EditorGUILayout.Space();

		EditorGUILayout.HelpBox("Set Resolution with Game View resolution dropdown.", MessageType.None);
		EditorGUILayout.HelpBox($"Output resolution: {outputWidth}x{outputHeight}", MessageType.None);

		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Capture", GUILayout.ExpandWidth(false)))
		{
			Capture();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void Capture()
	{
		string pathBase = "Media/screenshot_";
		string path = null;
		int counter = 0;
		while (true)
		{
			path = pathBase+counter+".png";
			if (!File.Exists(path))
			{
				break;
			}
			counter++;
		}
		ScreenCapture.CaptureScreenshot(path);
	}
}
