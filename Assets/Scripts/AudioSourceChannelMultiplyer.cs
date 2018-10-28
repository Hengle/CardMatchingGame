using UnityEngine;
using System.Collections;

public class AudioSourceChannelMultiplyer : MonoBehaviour
{
	public GameSettings.Audio.Channel channel;
	public AudioSource audioSource;
	private float baseVolume = Mathf.NegativeInfinity;

	private void OnEnable()
	{
		var audioSource = this.audioSource ? this.audioSource : gameObject.GetComponent<AudioSource>();
		if (baseVolume == Mathf.NegativeInfinity)
		{
			baseVolume = audioSource.volume;
		}
		audioSource.volume = baseVolume*GameSettings.Audio.GetVolume(channel);
	}
}
