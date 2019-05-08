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
	public Text scoreText;
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
		scoreText.text = Game.current.gameStats.score.ToString();
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
