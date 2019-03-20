using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuManager : MonoBehaviour
{
	private class MenuInfo
	{
		public MenuType type;
		public Transform rootTransform;

		public void Open()
		{
			rootTransform.gameObject.SetActive(true);
		}

		public void Close()
		{
			rootTransform.gameObject.SetActive(false);
		}
	}

	public enum MenuType { None, Splash, LevelSelect, Info, Settings, Credits, Directions }

	public Transform splashScreenTransform;
	public Transform levelSelectTransform;
	public Transform infoTransform;
	public Transform settingsTransform;
    public Transform creditsTransform;
    public Transform directionsTransform;

	private List<MenuInfo> menuInfos = new List<MenuInfo>();
	private static List<MenuType> menuTypeStack = new List<MenuType>();
    private static MenuType currentMenuType => menuTypeStack.LastOrDefault();

    private void Start()
	{
		menuInfos.Add(new MenuInfo() { type = MenuType.Splash, rootTransform = splashScreenTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.LevelSelect, rootTransform = levelSelectTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.Info, rootTransform = infoTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.Settings, rootTransform = settingsTransform });
        menuInfos.Add(new MenuInfo() { type = MenuType.Credits, rootTransform = creditsTransform });
        menuInfos.Add(new MenuInfo() { type = MenuType.Directions, rootTransform = directionsTransform });

        var menuType = currentMenuType;

		if (menuType == MenuType.None)
		{
			menuType = MenuType.Splash;
		}
		PushMenuType(menuType);
	}

	public void PushMenuTypeInt(int menuTypeInt)
	{
		PushMenuType((MenuType)menuTypeInt);
	}

	public void PushMenuType(MenuType menuType)
	{
		foreach (var menuInfo in menuInfos)
		{
			menuInfo.Close();
		}

		if (currentMenuType != menuType)
		{
			menuTypeStack.Add(menuType);
		}

		MenuInfo menu = menuInfos.First(v => v.type == menuType);
		menu.Open();
	}

	public void PopMenuType()
	{
		if (menuTypeStack.Count <= 1)
		{
			return;
		}

		foreach (var menuInfo in menuInfos)
		{
			menuInfo.Close();
		}

		menuTypeStack.RemoveAt(menuTypeStack.Count-1);

		MenuInfo menu = menuInfos.First(v => v.type == currentMenuType);
		menu.Open();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			menuTypeStack.ForEach(v => Debug.Log(v));
			if (currentMenuType == MenuType.Splash)
			{
				Application.Quit();
			}
			PopMenuType();
		}
		else if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
		{
			if (currentMenuType == MenuType.Splash)
			{
				PushMenuType(MenuType.LevelSelect);
			}
		}
    }

    public void PlaySound(AudioClip clip)
    {
        OneShotAudio.Play(clip, 0, GameSettings.Audio.sfxVolume);
    }
}
