using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Music")]
    // [SerializeField] private AudioClip musicClip;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip shootGunClip;
    [SerializeField] private AudioClip reloadGunClip;
    [SerializeField] private AudioClip impactClip;  
    /*[SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip impactClip;
    
    [SerializeField] private AudioClip itemClip;
    */

    public AudioClip ShootClip => shootClip;
    public AudioClip CoinClip => coinClip;
    public AudioClip ShootGunClip => shootGunClip;
    public AudioClip ReloadGunClip => reloadGunClip;
    public AudioClip ImpactClip => impactClip;
    /*public AudioClip ImpactClip => impactClip;
    public AudioClip CoinClip => coinClip;
    public AudioClip ItemClip => itemClip;
    public AudioClip DashClip => dashClip;*/ 

    private AudioSource musicAudioSource;
    private ObjectPooler soundObjectPooler;

    protected override void Awake()
    {
        soundObjectPooler = GetComponent<ObjectPooler>();
        // musicAudioSource = GetComponent<AudioSource>();
        
        // PlayMusic();
    }

    private void PlayMusic()
    {
        // musicAudioSource.loop = true;
        // musicAudioSource.clip = musicClip;
        // musicAudioSource.Play();
    }
    
    public void PlaySound(AudioClip clipToPlay, float volume)
    {
        GameObject audioPooled = soundObjectPooler.GetObjectFromPool();
        AudioSource audioSource = null;

        if (audioPooled != null)
        {
            audioPooled.SetActive(true);
            audioSource = audioPooled.GetComponent<AudioSource>();
        }

        audioSource.clip = clipToPlay;
        audioSource.volume = volume;
        audioSource.Play();

        StartCoroutine(ReturnToPool(audioPooled, clipToPlay.length + 1));
    }

    private IEnumerator ReturnToPool(GameObject objectPool, float delay)
    {
        yield return new WaitForSeconds(delay);
        objectPool.SetActive(false);
    }

    public void RebuildSoundPool() {
        if (soundObjectPooler == null) {
            soundObjectPooler = GetComponent<ObjectPooler>();
        }
        
    } 
}
