using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Reclamation.Misc;

namespace Reclamation.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] Dictionary<string, SoundEffect> soundEffects;
        public Dictionary<string, SoundEffect> SoundEffects
        {
            get
            {
                return soundEffects;
            }
        }

        [SerializeField] Dictionary<string, SoundEffect> ambientLoops;
        public Dictionary<string, SoundEffect> AmbientLoops { get { return ambientLoops; } }

        [SerializeField] Dictionary<string, SoundEffect> musicTracks;
        public Dictionary<string, SoundEffect> MusicTracks { get { return musicTracks; } }

        [SerializeField] Playlist worldPlaylist;
        public Playlist WorldPlaylist { get { return worldPlaylist; } }

        public void Initialize()
        {
            soundEffects = new Dictionary<string, SoundEffect>();
            ambientLoops = new Dictionary<string, SoundEffect>();
            musicTracks = new Dictionary<string, SoundEffect>();

            LoadSoundEffects();
            LoadAmbientLoops();
            LoadMusicTracks();

            foreach (KeyValuePair<string, SoundEffect> kvp in soundEffects)
            {
                kvp.Value.source = gameObject.AddComponent<AudioSource>();
                kvp.Value.source.name = kvp.Key;
                kvp.Value.source.clip = kvp.Value.clip;
                kvp.Value.source.volume = kvp.Value.volume;
                kvp.Value.source.pitch = kvp.Value.pitch;
                kvp.Value.source.playOnAwake = kvp.Value.playOnAwake;
                kvp.Value.source.spatialBlend = kvp.Value.spatialBlend;
                kvp.Value.source.loop = kvp.Value.loop;
            }

            foreach (KeyValuePair<string, SoundEffect> kvp in ambientLoops)
            {
                kvp.Value.source = gameObject.AddComponent<AudioSource>();
                kvp.Value.source.name = kvp.Key;
                kvp.Value.source.clip = kvp.Value.clip;
                kvp.Value.source.volume = kvp.Value.volume;
                kvp.Value.source.pitch = kvp.Value.pitch;
                kvp.Value.source.playOnAwake = kvp.Value.playOnAwake;
                kvp.Value.source.spatialBlend = kvp.Value.spatialBlend;
                kvp.Value.source.loop = kvp.Value.loop;
            }

            foreach (KeyValuePair<string, SoundEffect> kvp in musicTracks)
            {
                kvp.Value.source = gameObject.AddComponent<AudioSource>();
                kvp.Value.source.name = kvp.Key;
                kvp.Value.source.clip = kvp.Value.clip;
                kvp.Value.source.volume = kvp.Value.volume;
                kvp.Value.source.pitch = kvp.Value.pitch;
                kvp.Value.source.playOnAwake = kvp.Value.playOnAwake;
                kvp.Value.source.spatialBlend = kvp.Value.spatialBlend;
                kvp.Value.source.loop = kvp.Value.loop;
            }
        }

        void LoadSoundEffects()
        {
            UnityEngine.Object[] objects = Resources.LoadAll("Audio/Sound Effects");

            foreach (GameObject go in objects)
            {
                SoundEffect soundEffect = go.GetComponent<SoundEffect>();

                if (soundEffect == null)
                {
                }
                else
                {
                    //Debug.Log("Sound Effect " + go.name + " loaded");
                    soundEffects.Add(go.name, soundEffect);
                }
            }
        }

        void LoadAmbientLoops()
        {
            UnityEngine.Object[] objects = Resources.LoadAll("Audio/Ambient Loops");

            foreach (GameObject go in objects)
            {
                SoundEffect ambientLoop = go.GetComponent<SoundEffect>();

                if (ambientLoop == null)
                {
                }
                else
                {
                    //Debug.Log("Ambient Loop " + go.name + " loaded");
                    ambientLoops.Add(go.name, ambientLoop);
                }
            }
        }

        void LoadMusicTracks()
        {
            UnityEngine.Object[] objects = Resources.LoadAll("Audio/Music Tracks");

            foreach (GameObject go in objects)
            {
                SoundEffect musicTrack = go.GetComponent<SoundEffect>();

                if (musicTrack == null)
                {
                }
                else
                {
                    //Debug.Log("Music Track " + go.name + " loaded");
                    musicTracks.Add(go.name, musicTrack);
                }
            }
        }

        public void PlaySound(string key, bool variance)
        {
            if (soundEffects.ContainsKey(key) == false)
            {
                Debug.LogWarning("soundEffects does not contain " + key);
            }
            else
            {
                soundEffects[key].Play(variance);
            }
        }

        public void PlayAmbient(string key)
        {
            if (ambientLoops.ContainsKey(key) == false)
            {
                Debug.LogWarning("ambientLoops does not contain " + key);
            }
            else
            {
                ambientLoops[key].source.Play();
            }
        }

        public void PlayMusic(string key)
        {
            if (musicTracks.ContainsKey(key) == false)
            {
                Debug.LogWarning("musicTracks does not contain " + key);
            }
            else
            {
                musicTracks[key].source.Play();
            }
        }
    }
}