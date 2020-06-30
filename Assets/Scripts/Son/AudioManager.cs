using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;
	
	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		for (int i = 0; i < sounds.Length; i++)
		{
			Sound s = sounds[i];
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.spatialBlend = 1;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = findSound(sound);

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		if (!s.source.isPlaying)
		{
			s.source.Play();
		}
	}
	public void Stop(string sound)
	{
		Sound s = findSound(sound);
		s.source.Stop();
	}

	public void PlayAtPoint(string sound,Vector3 position)
	{
		Sound s = findSound(sound);

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		float clipLength = s.clip.length; // j'ai pas fini le code
		AudioSource.PlayClipAtPoint(s.clip, position);
	}

	public void Pause(string sound)
	{
		Sound s = findSound(sound);

		if (s.source.isPlaying)
		{
			s.source.Pause();
		}
	}
	public void UnPause(string sound)
	{
		Sound s = findSound(sound);
		s.source.UnPause();
	}

	public void PauseAll()
	{
		foreach(Sound s in sounds)
		{
			Pause(s.name);
		}
	}

	public void UnPauseAll()
	{
		foreach (Sound s in sounds)
		{
			UnPause(s.name);
		}
	}

	public Sound findSound(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return null;
		}
		return s;
	}

	public void ChangePitch(string sound, float pitch)
	{
		Sound s = findSound(sound);
		s.pitch = pitch;

	}

	public void ChangeAllPitch(float pitch)
	{
		foreach (Sound s in sounds)
		{
			ChangePitch(s.name, pitch);
		}
	}


}
