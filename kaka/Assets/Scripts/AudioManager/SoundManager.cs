//Author: Small Hedge Games
//Updated: 13/06/2024

using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        private static SoundManager instance = null;
        private AudioSource audioSource;

        private static AudioSource musicAudioSource;

        private void Awake()
        {
            if(!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }
        }

        public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1, bool canLoop = false)
            {
                SoundList soundList = instance.SO.sounds[(int)sound];
                AudioClip[] clips = soundList.sounds;
                AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

                // HIER NEU: Wenn der Sound loopen soll (z.B. deine Hintergrundmusik)
                if (canLoop)
                {
                    // Falls keine extra Source übergeben wurde, erstellen wir eine feste Musik-Source
                    if (source == null)
                    {
                        if (musicAudioSource == null)
                        {
                            musicAudioSource = instance.gameObject.AddComponent<AudioSource>();
                        }
                        source = musicAudioSource;
                    }

                    source.outputAudioMixerGroup = soundList.mixer;
                    source.clip = randomClip;
                    source.volume = volume * soundList.volume;
                    source.loop = true; // Loop aktivieren!
                    source.Play();      // Play() statt PlayOneShot(), damit es loopt
                    
                    return; // Beendet die Methode hier, damit es nicht unten nochmal als SFX triggert
                }

                // --- AB HIER BLEIBT ALLES BEIM ALTEN FÜR DEINE NORMALEN EFFEKTE ---
                if(source)
                {
                    source.outputAudioMixerGroup = soundList.mixer;
                    source.clip = randomClip;
                    source.volume = volume * soundList.volume;
                    source.loop = false; // Reset, falls die Source vorher für Musik genutzt wurde
                    source.Play();
                }
                else
                {
                    instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                    instance.audioSource.PlayOneShot(randomClip, volume * soundList.volume);
                }
            }
        }

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0, 1)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}