using System.Collections;
using System.Collections.Generic;
using KeenTween;
using UnityEngine;

public static class OneShotAudio
{
	public static AudioSource Play(AudioClip clip, float spacialBlend, float volume)
	{
		AudioSource audioSource = null;

		if (clip)
		{
			audioSource = new GameObject("OneShotAudioSource").AddComponent<AudioSource>();
			Object.DontDestroyOnLoad(audioSource.gameObject);
			Object.Destroy(audioSource.gameObject, clip.length);

			audioSource.clip = clip;
			audioSource.spatialBlend = spacialBlend;
			audioSource.volume = volume;
			audioSource.Play();
		}

		return audioSource;
	}
}
