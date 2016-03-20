using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelOverlayItem:MonoBehaviour
{
	public Text text;

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

	private int _number;
	public int number
	{
		get
		{
			return _number;
        }
		set
		{
			_number = value;
			text.text = _number.ToString();
		}
	}

	void Start()
	{
		button.onClick.AddListener(OnClickButton);
	}

	void OnClickButton()
	{
		LevelSelect.currentlySelectedLevelTemplate = level;
		Debug.Log(level);
		SceneManager.LoadScene("Game");
	}
}
