using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class SceneManager : MonoBehaviour
{
    public GameObject mainScreen, settingScreen, firstTimeScreen, highScroceScreen;

    public Button musicOn, musicOff, sfxOn, sfxOff, btnYes, btnNo;

    public Slider musicSLider, sfxSlider;

  

    void Start()
    {
        settingScreen.SetActive(false); 
        firstTimeScreen.SetActive(false);
        highScroceScreen.SetActive(false);
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoOpenScene(GameObject screen) {
        AudioManager.Instance.PlaySFX("Click Sound");
        screen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void DoCloseScene() {
        mainScreen.SetActive(true );
        settingScreen.SetActive(false);
        firstTimeScreen.SetActive(false);
        highScroceScreen.SetActive(false);
    }

    public void ToggleMusic() {
        AudioManager.Instance.ToggleMusic();
        if(AudioManager.Instance.musicPlaying) {
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

    public void MusicVolume() {
        print(musicSLider.value);
        AudioManager.Instance.MusicVolume(musicSLider.value);
    }

    public void SFXVolume() {
        AudioManager.Instance.SFXVolume(sfxSlider.value);
    }

    public void ChangeScene(string screenName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(screenName);
        AudioManager.Instance.PlayMusic("Chapter 1");
    }




}
