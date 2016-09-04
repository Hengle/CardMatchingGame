using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameUIManager:MonoBehaviour {
	public Game game;
	public Canvas canvas;
	public RectTransform failIconLayout;
	public FailIcon failIconTemplate;

	private FailIcon[] failIcons = new FailIcon[0];
	void Awake()
	{
		failIcons = new FailIcon[game.currentLevel.maxFailCount];
		failIconTemplate.gameObject.SetActive(false);

		for (int i = 0; i < game.currentLevel.maxFailCount; i++)
		{
			FailIcon failIcon = (FailIcon)Instantiate(failIconTemplate, failIconLayout);
			failIcon.gameObject.SetActive(true);
			failIcon.SetActive(false);
			failIcons[i] = failIcon;
		}
	}

	public void UpdateFailCounter()
	{
		for (int i = 0; i < game.currentLevel.maxFailCount; i++)
		{
			FailIcon failIcon = failIcons[i];
			failIcon.SetActive(i < game.failCount);
		}
	}
}
