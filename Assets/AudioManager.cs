using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager m_instance;
    public AudioMixerGroup m_soundEffectGroup;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_instance = this;
    }

    public static AudioManager GetInstance()
    {
        return m_instance;
    }

    private void Start()
    {
        //
        //
        m_audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(m_audioSource);
    }

    public void PlaySoundEffect(AudioClip soundEffectClip, float fVolume)
    {
        m_audioSource.outputAudioMixerGroup = m_soundEffectGroup;
        m_audioSource.PlayOneShot(soundEffectClip);
    }
}
