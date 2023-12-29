using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;
using DapperDino.Scoreboards;
using System.IO;

public class GameManager : MonoBehaviour {
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
    private bool isFinished = false;
    private bool isPause = false;
    private bool isSaved = false;

    //Global Variable
    public List<Enemy> listEnemy;
    private string timeFinished;

    //High Score;
    [SerializeField] private int maxScoreboardEntries = 5;
    private string SavePath => $"{Application.persistentDataPath}/highscores.json";



    public static GameManager Instance { get; private set; }

    private void Awake() {
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
        if (!isPause && !isFinished) {
            timeRemaining += Time.deltaTime;
        }

        if (isFinished && !isSaved) {
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timeFinished = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeText.text = "Time: " + timeFinished;
            AddEntry(new ScoreboardEntryData() {
                entryName = "Level 1",
                entryScore = timeFinished
            });
            isSaved = true;
        }
    }

    public void AddEntry(ScoreboardEntryData scoreboardEntryData) {
        ScoreboardSaveData savedScores = GetSavedScores();


        bool scoreAdded = false;

        //Check if the score is high enough to be added.
        for (int i = 0; i < savedScores.highscores.Count; i++) {
            if (string.Compare(scoreboardEntryData.entryScore, savedScores.highscores[i].entryScore) < 0) {
                savedScores.highscores.Insert(i, scoreboardEntryData);
                scoreAdded = true;
                break;
            }
        }

        //Check if the score can be added to the end of the list.
        if (!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries) {
            savedScores.highscores.Add(scoreboardEntryData);
        }

        //Remove any scores past the limit.
        if (savedScores.highscores.Count > maxScoreboardEntries) {
            savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);
        }

        SaveScores(savedScores);
    }

    public void Win() {
        winMenu.SetActive(true);
        isFinished = true;
        Time.timeScale = 0;
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        isPause = true;
        Debug.Log("Pausing");
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

    private ScoreboardSaveData GetSavedScores() {
        if (!File.Exists(SavePath)) {
            File.Create(SavePath).Dispose();
            return new ScoreboardSaveData();
        }

        using (StreamReader stream = new StreamReader(SavePath)) {
            string json = stream.ReadToEnd();

            return JsonUtility.FromJson<ScoreboardSaveData>(json);
        }
    }

    private void SaveScores(ScoreboardSaveData scoreboardSaveData) {
        using (StreamWriter stream = new StreamWriter(SavePath)) {
            string json = JsonUtility.ToJson(scoreboardSaveData, true);
            stream.Write(json);
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
