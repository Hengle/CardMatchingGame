using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelOverlayItem:MonoBehaviour
{
	public Text text;
	public Image thumbnailImage;
	public AudioClip tapSound;

	private Button _button;
	public Button button
	{
		get
		{
			return _button ? _button : _button = gameObject.GetComponent<Button>();
		}
	}
	
	private LevelOverlay _levelOverlay;
	public LevelOverlay levelOverlay
	{
		get
		{
			return _levelOverlay ? _levelOverlay : _levelOverlay = gameObject.GetComponentInParent<LevelOverlay>();
		}
	}

	private Level _level;
	public Level level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
		}
	}

	void Start()
	{
		button.onClick.AddListener(OnClickButton);
		if (level.thumbnail)
		{
			thumbnailImage.sprite = level.thumbnail;
		}
	}

	void OnClickButton()
	{
		LevelSelect.currentlySelectedLevelTemplate = level;

		OneShotAudio.Play(tapSound, 0, 1);

		Transition transition = Transition.CreateTransition();
		transition.onMidTransition += () =>
		{
			OneShotAudio.Play(null, 1, 1);
			SceneManager.LoadScene("Game");
		};

		levelOverlay.OnSelectedLevel(level);
	}
}
