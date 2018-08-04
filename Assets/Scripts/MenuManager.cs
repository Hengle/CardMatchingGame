using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private enum MenuType { Splash, LevelSelect, Info, Settings }

	public Transform splashScreenTransform;
	public Transform levelSelectTransform;
	public Transform infoTransform;
	public Transform settingsTransform;

	private static MenuType currentMenuType = MenuType.Splash;
	private List<MenuInfo> menuInfos = new List<MenuInfo>();

	private void Start()
	{
		menuInfos.Add(new MenuInfo() { type = MenuType.Splash, rootTransform = splashScreenTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.LevelSelect, rootTransform = levelSelectTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.Info, rootTransform = infoTransform });
		menuInfos.Add(new MenuInfo() { type = MenuType.Settings, rootTransform = settingsTransform });

		SetMenuType(currentMenuType);
	}

	private void SetMenuType(MenuType menuType)
	{
		foreach (var menuInfo in menuInfos)
		{
			menuInfo.Close();
		}

		currentMenuType = menuType;

		MenuInfo newMenu = menuInfos[(int)currentMenuType];
		newMenu.Open();
	}

	private void Update()
	{
		//TODO: Create menu classes and move this logic into those.
		if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
		{
			
			if (currentMenuType == MenuType.Splash)
			{
				SetMenuType(MenuType.LevelSelect);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (currentMenuType == MenuType.LevelSelect)
			{
				SetMenuType(MenuType.Splash);
			}
            else if (currentMenuType == MenuType.Info)
            {
				SetMenuType(MenuType.LevelSelect);
			}
        }
    }
}
