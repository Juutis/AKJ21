using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager main;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    private AudioSource loopingSound;

    [SerializeField]
    private List<GameSound> sounds;
    public void PlaySound(GameSoundType soundType)
    {
        if (soundType == GameSoundType.None)
        {
            return;
        }
        GameSound gameSound = sounds.Where(sound => sound.Type == soundType).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.Get();
            if (audio != null)
            {
                audio.pitch = 1f;
                if (gameSound.RandomizePitch)
                {
                    audio.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                }
                audio.Play();
            }
        }
    }

    public void PlayLoop(GameSoundType soundType)
    {
        if (loopingSound) {
            loopingSound.Play();
            return;
        }
        if (soundType == GameSoundType.None)
        {
            return;
        }
        GameSound gameSound = sounds.Where(sound => sound.Type == soundType).FirstOrDefault();
        if (gameSound != null)
        {
            AudioSource audio = gameSound.Get();
            if (audio != null)
            {
                if (!audio.isPlaying)
                {
                    audio.pitch = 1f;
                    audio.loop = true;
                    audio.Play();
                }
                loopingSound = audio;
            }
        }
    }
    public void StopLoop(GameSoundType soundType)
    {
        if (loopingSound != null) {
            loopingSound.Pause();
        }
    }

}


public enum GameSoundType
{
    None,
    Laser,
    Cannon,
    Hit,
    Pickup

}


[System.Serializable]
public class GameSound
{
    [field: SerializeField]
    public GameSoundType Type { get; private set; }

    [field: SerializeField]
    private List<AudioSource> sounds;

    [SerializeField]
    private bool randomizePitch;
    public bool RandomizePitch {get {return randomizePitch;}}

    private List<GameSoundPool> soundPools = new List<GameSoundPool>();
    private bool initialized = false;


    public AudioSource Get()
    {
        if (!initialized)
        {
            initialize();
        }

        if (sounds == null || sounds.Count == 0)
        {
            return null;
        }
        return soundPools[UnityEngine.Random.Range(0, soundPools.Count)].getAvailable();
    }

    private void initialize()
    {
        soundPools = sounds.Select(it => new GameSoundPool(it)).ToList();
        initialized = true;
    }


    private class GameSoundPool
    {
        private AudioSource originalAudioSource;
        private List<AudioSource> audioSources = new List<AudioSource>();

        public GameSoundPool(AudioSource audioSource)
        {
            originalAudioSource = audioSource;
            addNewToPool();
        }

        public AudioSource getAvailable()
        {
            var src = audioSources.Where(it => it.isPlaying == false).FirstOrDefault();
            if (src == null)
            {
                src = addNewToPool();
            }
            return src;
        }

        private AudioSource addNewToPool()
        {
            if (originalAudioSource == null)
            {

            }
            AudioSource newSource = GameObject.Instantiate(originalAudioSource, originalAudioSource.transform.parent);
            audioSources.Add(newSource);
            return newSource;
        }
    }
}