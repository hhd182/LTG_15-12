using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    public GameObject pauseMenu;

    public Button musicOn, musicOff, sfxOn, sfxOff;

    public Slider musicSlider, sfxSlider;


    public void Start() {
        float musicVolume = AudioManager.Instance.GetMusicVolume();
        float sfxVolume = AudioManager.Instance.GetSFXVolume();

        if (AudioManager.Instance.musicPlaying) {
            musicOn.gameObject.SetActive(true);
            musicOff.gameObject.SetActive(false);
        }
        else {
            musicOn.gameObject.SetActive(false);
            musicOff.gameObject.SetActive(true);
        }

        if (AudioManager.Instance.sfxPlaying) {
            sfxOn.gameObject.SetActive(true);
            sfxOff.gameObject.SetActive(false);
        }
        else {
            sfxOn.gameObject.SetActive(false);
            sfxOff.gameObject.SetActive(true);
        }

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveSystem.Instance.Load("saveSlot");
        }
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Home() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScreen");
        Time.timeScale = 1;
    }

    public void Restart() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ChangeMusicVolume() {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void ChangeSFXVolume() {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    public void ToggleMusic() {
        AudioManager.Instance.ToggleMusic();
        if (AudioManager.Instance.musicPlaying) {
            musicOn.gameObject.SetActive(true);
            musicOff.gameObject.SetActive(false);
        }
        else {
            musicOn.gameObject.SetActive(false);
            musicOff.gameObject.SetActive(true);
        }
    }

    public void ToggleSFX() {
        AudioManager.Instance.ToggleSFX();
        if (AudioManager.Instance.sfxPlaying) {
            sfxOn.gameObject.SetActive(true);
            sfxOff.gameObject.SetActive(false);
        }
        else {
            sfxOn.gameObject.SetActive(false);
            sfxOff.gameObject.SetActive(true);
        }
    }
}
