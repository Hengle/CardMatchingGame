using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameUIManager:MonoBehaviour {
	static private GameUIManager _current;
	static public GameUIManager current
	{
		get
		{
			return _current ? _current : _current = FindObjectOfType<GameUIManager>();
		}
	}

	public Canvas canvas;
	public RectTransform failIconLayout;
	public FailIcon failIconTemplate;

	public BeginPlayUI beginPlayUI;

	private FailIcon[] failIcons = new FailIcon[0];

	void Awake()
	{
		Game game = Game.current;

		failIcons = new FailIcon[game.currentLevel.maxFailCount];
		failIconTemplate.gameObject.SetActive(false);

		for (int i = 0; i < game.currentLevel.maxFailCount; i++)
		{
			FailIcon failIcon = (FailIcon)Instantiate(failIconTemplate, failIconLayout);
			failIcon.gameObject.SetActive(true);
			failIcon.SetActive(false);
			failIcons[i] = failIcon;
		}

		beginPlayUI.gameObject.SetActive(false);
	}

	public void UpdateFailCounter()
	{
		Game game = Game.current;

		for (int i = 0; i < game.currentLevel.maxFailCount; i++)
		{
			FailIcon failIcon = failIcons[i];
			failIcon.SetActive(i < game.failCount);
		}
	}

	public void OnGameStart()
	{
		beginPlayUI.gameObject.SetActive(true);
	}
}
