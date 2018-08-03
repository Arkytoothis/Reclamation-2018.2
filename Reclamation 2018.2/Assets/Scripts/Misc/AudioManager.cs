using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Reclamation.Misc;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)] public float spatialBlend = 0f;
    [Range(0, 1)] public float volume = 0.7f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop;
    public bool playOnAwake;

    public AudioSource source;

    public void SetSource(AudioSource source)
    {
        name = source.name;
        this.source = source;
        source.clip = clip;
    }

    public void Play(string name)
    {
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = spatialBlend;

        source.PlayOneShot(source.clip);
    }
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] Sound[] soundEffects;
    [SerializeField] Sound[] ambientLoops;
    [SerializeField] Sound[] musicTracks;

    public void Initialize()
    {
        foreach (Sound sound in soundEffects)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.spatialBlend = sound.spatialBlend;
            sound.source.loop = sound.loop;
        }

        foreach (Sound sound in ambientLoops)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.spatialBlend = sound.spatialBlend;
            sound.source.loop = sound.loop;
        }

        foreach (Sound sound in musicTracks)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.spatialBlend = sound.spatialBlend;
            sound.source.loop = sound.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        s.source.Play();
    }

    public void PlayAmbient(string name)
    {
        Sound s = Array.Find(ambientLoops, sound => sound.name == name);
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicTracks, sound => sound.name == name);
        s.source.Play();
    }
}