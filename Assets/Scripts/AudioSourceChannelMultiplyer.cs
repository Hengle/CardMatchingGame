using UnityEngine;
using System.Collections;

public class AudioSourceChannelMultiplyer : MonoBehaviour
{
	public GameSettings.Audio.Channel channel;
	public AudioSource audioSource;

	private void Start()
	{
		var audioSource = this.audioSource ? this.audioSource : gameObject.GetComponent<AudioSource>();
		audioSource.volume *= GameSettings.Audio.GetVolume(channel);
	}
}
