using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	public RectTransform scoreRoot;
	public Text triesText;
	public Text matchesText;
	public Text missesText;
	public BeginPlayUI beginPlayUI;
	public EndGameUI endGameUI;
	public Button quitButton;
    public AudioClip clip;

	void Awake()
	{
		Game game = Game.current;

		beginPlayUI.gameObject.SetActive(false);
		quitButton.onClick.AddListener(OnClickQuit);
		quitButton.gameObject.SetActive(false);
		scoreRoot.gameObject.SetActive(false);
	}

	public void OnGameStart()
	{
		beginPlayUI.gameObject.SetActive(true);
		quitButton.gameObject.SetActive(true);
		scoreRoot.gameObject.SetActive(true);

		Game.current.gameStats.onStatsChanged += OnGameStatsChanged;
		OnGameStatsChanged();
	}

	private void OnGameStatsChanged()
	{
		var totalPossibleMatches = Game.current.GetNonLionCards().Length/2;
		triesText.text = Game.current.gameStats.currentTurn.ToString();
		matchesText.text = Game.current.gameStats.matches+"/"+totalPossibleMatches;

		missesText.text = Game.current.gameStats.misses.ToString();
	}

	private void OnClickQuit()
	{
        OneShotAudio.Play(clip, 0, GameSettings.Audio.sfxVolume);
        SceneManager.LoadScene("LevelSelect");
	}

	public void ShowEndScreen()
	{
		beginPlayUI.gameObject.SetActive(false);
		quitButton.gameObject.SetActive(false);
		scoreRoot.gameObject.SetActive(false);
		endGameUI.gameObject.SetActive(true);
	}
}
