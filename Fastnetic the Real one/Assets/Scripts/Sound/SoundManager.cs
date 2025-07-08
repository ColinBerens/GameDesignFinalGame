using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
	public Sounds[] Sounds;
	public static SoundManager instance;
	private bool _hasPlayed = false;
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		foreach (Sounds sound in Sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;

			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;
		}
	}
	public Sounds GetSoundInfo(string name)
    {
        Sounds s = Array.Find(Sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }
    public void StopAllSounds()
	{
		foreach (Sounds sound in Sounds)
		{
			sound.source.Stop();
		}
	}
    public Sounds PlayFull(string name)
    {
        Sounds s = Array.Find(Sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }

        if (s.RandomizePitch)
        {
            s.source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
        }

        s.IsPlayed = true;

        s.source.PlayOneShot(s.clip);

        return s;
    }
    public Sounds Play(string name)
	{
		Sounds s = Array.Find(Sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return null;
		}
		if (s.RandomizePitch)
		{
			s.source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
		}
		s.IsPlayed = true;
        s.source.Play();
		return s;
	}
	public void Stop(string name)
	{
		Sounds s = Array.Find(Sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
        if (s.IsPlayed)
        {
            s.IsPlayed = false;
        }
        else
        {
            return;
        }
        s.source.Stop();
	}
}
