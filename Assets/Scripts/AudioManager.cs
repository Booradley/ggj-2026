using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    public AudioClip[] maskSounds;

    [SerializeField]
    public AudioClip interactSound;

    [SerializeField]
    public AudioSource oneShotAudioSource;

    public void PlayMaskSound(float scoreRatio)
    {
        oneShotAudioSource.PlayOneShot(maskSounds[(int)((maskSounds.Length - 1) * scoreRatio)]);
    }

    public void PlayInteractSound()
    {
        oneShotAudioSource.PlayOneShot(interactSound);
    }
}