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

	public enum MenuType { None, Splash, LevelSelect, Info, Settings }

	public Transform splashScreenTransform;
	public Transform levelSelectTransform;
	public Transform infoTransform;
	public Transform settingsTransform;

	private List<MenuInfo> menuInfos = new List<MenuInfo>();
	private static List<MenuType> menuTypeStack = new List<MenuType>();
    private static MenuType currentMenuType => menuTypeStack.LastOrDefault();

    private void Start()
	{
		menuInfos.Add(new MenuInfo() { type = MenuType.Splash, rootTransform = splashScreenTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.LevelSelect, rootTransform = levelSelectTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.Info, rootTransform = infoTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.Settings, rootTransform = settingsTransform });

		var menuType = currentMenuType;
		menuTypeStack.Clear();

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
		Debug.Log(menuType);
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
		//TODO: Create menu classes and move this logic into those.
		if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
		{
			if (currentMenuType == MenuType.Splash)
			{
				PushMenuType(MenuType.LevelSelect);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PopMenuType();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        OneShotAudio.Play(clip, 0, GameSettings.Audio.sfxVolume);
    }
}
