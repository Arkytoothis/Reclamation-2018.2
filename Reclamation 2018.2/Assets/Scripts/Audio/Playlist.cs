using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Audio
{
    [System.Serializable]
    public class Playlist : MonoBehaviour
    {
        [SerializeField] List<string> clips;
        [SerializeField] int currentClipIndex = -1;
        [SerializeField] AudioSource audioSource;
        [SerializeField] bool randomPlay = false;

        void Update()
        {
            if (audioSource != null && audioSource.isPlaying == false)
            {
                string nextClip;

                if (randomPlay)
                {
                    nextClip = GetRandomClip();
                }
                else
                {
                    nextClip = GetNextClip();
                }

                currentClipIndex = clips.IndexOf(nextClip);
                audioSource.clip = AudioManager.instance.MusicTracks[nextClip].clip;
                audioSource.Play();
            }
        }

        private string GetRandomClip()
        {
            return clips[Random.Range(0, clips.Count)];
        }

        private string GetNextClip()
        {
            currentClipIndex++;
            if (currentClipIndex > clips.Count - 1)
                currentClipIndex = 0;

            Debug.Log("Playing " + clips[currentClipIndex]);
            return clips[currentClipIndex];
        }

        public void StartPlaying(int index)
        {
            Debug.Log("Starting playlist " + gameObject.name);
            string clip = clips[index];
            Debug.Log("track " + clip);
            audioSource = AudioManager.instance.MusicTracks[clip].source;
            audioSource.Play();
        }
    }
}