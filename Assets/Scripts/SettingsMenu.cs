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
		Debug.Log(GameSettings.Audio.masterVolume);
		CreateSlider("Master Volume", v => GameSettings.Audio.masterVolume = v).value = GameSettings.Audio.masterVolume;
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
