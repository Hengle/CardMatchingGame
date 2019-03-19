using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KeenTween;

public class LevelOverlayItem:MonoBehaviour
{
	public Image thumbnailImage;
	public Image lionThumbnailImage;
	public Sprite lionThumbnailLockedSprite;
	public Button primaryButton;
	public Button lionButton;
	public AudioClip tapSound;
	public CanvasGroup canvasGroup;
	
	private Tween appearTween;
	
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
		primaryButton.onClick.AddListener(() => OnClickButton(false));
		lionButton.onClick.AddListener(() => OnClickButton(true));

		if (level.thumbnail)
		{
			thumbnailImage.sprite = level.thumbnail;
		}
		if (level.lionCardDefs.Count > 0)
		{
			if (level.lionThumbnail)
			{
				lionThumbnailImage.sprite = level.lionThumbnail;
			}
			else
			{
				Debug.LogWarning($"Level \"{level.name}\" has {nameof(level.lionCardDefs)} but no {nameof(level.lionThumbnail)}.", level);
			}
			if (string.IsNullOrEmpty(level.identifier))
			{
				Debug.LogWarning($"Level \"{level.name}\" has no {nameof(level.identifier)} set.", level);
			}
			var levelStats = GameData.GetLevelStats(level.identifier);
			if (!levelStats.normalStats.beat)
			{
				lionButton.interactable = false;
				lionThumbnailImage.sprite = lionThumbnailLockedSprite;
			}
		}
		else
		{
			lionButton.gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		if (appearTween != null && !appearTween.isDone)
		{
			appearTween.Cancel();
		}
	}

	void OnClickButton(bool lionMode)
	{
		LevelSelect.gameInfo.selectedLevel = level;
		LevelSelect.gameInfo.lionMode = lionMode;

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
