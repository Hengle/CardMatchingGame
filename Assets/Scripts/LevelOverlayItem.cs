using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KeenTween;

public class LevelOverlayItem:MonoBehaviour
{
	public Text text;
	public Image thumbnailImage;
	public AudioClip tapSound;
	public CanvasGroup canvasGroup;

	private Tween appearTween;

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

	private void OnDestroy()
	{
		if (appearTween != null && !appearTween.isDone)
		{
			appearTween.Cancel();
		}
	}

	void OnClickButton()
	{
		LevelSelect.currentlySelectedLevelTemplate = level;

		OneShotAudio.Play(tapSound, 0, GameSettings.Audio.sfxVolume);

		Transition transition = Transition.CreateTransition();
		transition.onMidTransition += () =>
		{
			SceneManager.LoadScene("Game");
		};

		levelOverlay.OnSelectedLevel(level);
	}

	public void StartAppearAnimation(int itemIndex)
	{
		canvasGroup.alpha = 0;
		appearTween = new Tween(null, 0, 1, 0.25f, new CurveCubic(TweenCurveMode.Out), t =>
		{
			if (!this)
			{
				return;
			}
			transform.localScale = Vector3.one*Mathf.Lerp(0.75f, 1, t.currentValue);
			canvasGroup.alpha = t.currentValue;
		});
		appearTween.delay = itemIndex*0.1f;
	}
}
