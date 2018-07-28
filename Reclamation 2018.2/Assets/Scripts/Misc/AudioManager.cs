using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume = 0.7f;
    public float pitch = 1f;

    private AudioSource source;

    public void SetSource(AudioSource source)
    {
        this.source = source;
        source.clip = clip;
    }

    public void Play(string name)
    {
        source.volume = volume;
        source.pitch = pitch;

        source.Play();
    }
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private Sound[] sounds;
}