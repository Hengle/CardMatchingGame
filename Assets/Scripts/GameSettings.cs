using UnityEngine;
using System.Collections;

public static class GameSettings
{
	public static class Audio
	{
		public enum Channel
		{
			None,
			Master,
			Voice,
			Music,
			SFX
		}

		private const float defaultVolume = 0.7f;

		public static float masterVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Audio.MasterVolume", AudioListener.volume);
			}
			set
			{
				PlayerPrefs.SetFloat("Audio.MasterVolume", value);
				AudioListener.volume = value;
			}
		}

		public static float voiceVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Audio.VoiceVolume", defaultVolume);
			}
			set
			{
				PlayerPrefs.SetFloat("Audio.VoiceVolume", value);
			}
		}

		public static float musicVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Audio.MusicVolume", defaultVolume);
			}
			set
			{
				PlayerPrefs.SetFloat("Audio.MusicVolume", value);
			}
		}

		public static float sfxVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Audio.SFXVolume", defaultVolume);
			}
			set
			{
				PlayerPrefs.SetFloat("Audio.SFXVolume", value);
			}
		}

		public static float GetVolume(Channel channel)
		{
			if (channel == Channel.Master)
			{
				return masterVolume;
			}
			else if (channel == Channel.Voice)
			{
				return voiceVolume;
			}
			else if (channel == Channel.Music)
			{
				return musicVolume;
			}
			else if (channel == Channel.SFX)
			{
				return sfxVolume;
			}

			return 0;
		}

		public static void SetVolume(Channel channel, float value)
		{
			if (channel == Channel.Master)
			{
				masterVolume = value;
			}
			else if (channel == Channel.Voice)
			{
				voiceVolume = value;
			}
			else if (channel == Channel.Music)
			{
				musicVolume = value;
			}
			else if (channel == Channel.SFX)
			{
				sfxVolume = value;
			}
		}

		public static void InitializeMasterVolume()
		{
			AudioListener.volume = PlayerPrefs.GetFloat("Audio.MasterVolume", AudioListener.volume);
		}
	}
}
