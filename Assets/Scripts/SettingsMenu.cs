using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingsMenu:MonoBehaviour
{
	public GameObject propertySliderTemplate;

	private Slider masterVolumeSlider;

	private void Start()
	{
		propertySliderTemplate.gameObject.SetActive(false);

		CreateSlider("Master Volume", v => GameSettings.Audio.masterVolume = v).value = GameSettings.Audio.masterVolume;
		CreateSlider("Music Volume", v => GameSettings.Audio.musicVolume = v).value = GameSettings.Audio.musicVolume;
		CreateSlider("SFX Volume", v => GameSettings.Audio.sfxVolume = v).value = GameSettings.Audio.sfxVolume;
		CreateSlider("Voice Volume", v => GameSettings.Audio.voiceVolume = v).value = GameSettings.Audio.voiceVolume;

		Canvas.ForceUpdateCanvases();

		gameObject.SetActive(false);
		gameObject.SetActive(true);
	}

	private Slider CreateSlider(string title, UnityAction<float> onChanged)
	{
		var instance = Instantiate(propertySliderTemplate, propertySliderTemplate.transform.parent);
		var slider = instance.GetComponentInChildren<Slider>();
		if (onChanged != null)
		{
			slider.onValueChanged.AddListener(onChanged);
		}
		var text = instance.GetComponentInChildren<Text>();
		text.text = title;
		instance.gameObject.SetActive(true);
		return slider;
	}
}
