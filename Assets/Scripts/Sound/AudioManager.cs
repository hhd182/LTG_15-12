using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSoure, sfxSource;

    public bool musicPlaying = true, sfxPlaying = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        PlayMusic("Back Ground Sound");
    }

    public void PlayMusic(string name) {
        Sound s = Array.Find(musicSounds, x => x.audioName == name);

        if(s == null) {
            Debug.Log("Sound not found");
        }
        else {
            musicSoure.clip = s.clip;
            musicSoure.Play();
        }
    }

    public void PlaySFX(string name) {
        Sound s = Array.Find(sfxSounds, x => x.audioName == name);

        if (s == null) {
            Debug.Log("SFX not found");
        }
        else {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic() {
        musicSoure.mute = !musicSoure.mute;
        musicPlaying = !musicPlaying;
    }

    public void ToggleSFX() {
        sfxSource.mute = !sfxSource.mute;
        sfxPlaying = !sfxPlaying;
    }

    public void MusicVolume(float volume) {
        print("Volume: " + volume);
        musicSoure.volume = volume;
    }

    public void SFXVolume(float volume) {
        sfxSource.volume = volume;
    }




}
