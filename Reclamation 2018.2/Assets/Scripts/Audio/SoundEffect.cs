using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Audio
{
    [System.Serializable]
    public class SoundEffect : MonoBehaviour
    {
        public new string name;
        public AudioClip clip;

        [Range(0, 1)] public float spatialBlend = 0f;
        [Range(0, 1)] public float volume = 0.7f;
        [Range(0, 0.5f)] public float volumeVariance = 0.1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        [Range(0, 0.5f)] public float pitchVariance = 0.1f;

        public bool loop;
        public bool playOnAwake;

        public AudioSource source;

        public void SetSource(AudioSource source)
        {
            name = source.name;
            this.source = source;
            source.clip = clip;
        }

        public void Play(bool variance)
        {
            if (variance == true)
            {
                source.volume = volume * (1 + UnityEngine.Random.Range(-volumeVariance / 2f, volumeVariance / 2f));
                source.pitch = pitch * (1 + UnityEngine.Random.Range(-pitchVariance / 2f, pitchVariance / 2f));
            }
            else
            {
                source.volume = volume;
                source.pitch = pitch;
            }

            source.spatialBlend = spatialBlend;
            source.PlayOneShot(clip);
        }
    }
}