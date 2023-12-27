using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class GameManager : MonoBehaviour
{   
    //UI
    public GameObject winMenu, pauseMenu;
    public Button musicOn, musicOff, sfxOn, sfxOff;
    public Slider musicSlider, sfxSlider;
    public TextMeshProUGUI timeText;

    //Global Light
    public UnityEngine.Rendering.Universal.Light2D globalLight;
    [SerializeField] private float intensityMin = 0.02f;
    [SerializeField] private float intensityMax = 1.0f;

    //Time finish the game
    [SerializeField] private float timeRemaining = 0f;
    private bool timeIsRunning = true;
    private bool isPause = false;

    //Global Variable
    public List<Enemy> listEnemy;
    private string timeFinished;

    
    public static GameManager Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void Start() {

        globalLight.intensity = intensityMin;

        StartCoroutine(ChangeLightIntensity());

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

    private void Update() {
        if (!isPause) {
            timeRemaining += Time.deltaTime;
            Debug.Log(timeRemaining);
        }
        else {
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timeFinished = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeText.text = "Time: " + timeFinished;
            Debug.Log("Time finished: " + timeFinished);

        }
    }

    public void Win()
    {
        winMenu.SetActive(true);
        isPause = true;
       
        Time.timeScale = 0;
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        isPause = true;
        Time.timeScale = 0;
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        isPause = false;
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

    IEnumerator ChangeLightIntensity() {
        while (true) {
            // Đợi 5 giây
            
            yield return new WaitForSeconds(30f);
            AudioManager.Instance.PlaySFX("Thunder Sound");
            globalLight.intensity = intensityMax;

            yield return new WaitForSeconds(1f);
            globalLight.intensity = intensityMin;
        }
    }
}
