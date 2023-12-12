using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject settingScreen;
    public GameObject firstTimeScreen;
    public GameObject highScroceScreen;

    public Button musicOn;
    public Button musicOff;
    public Button btnYes;
    public Button btnNo;

    public AudioSource src;
    public AudioClip sound1;
    public AudioClip sfx1;
    

    private bool isEnableSound = true;

    
    void Start()
    {
        settingScreen.SetActive(false); 
        firstTimeScreen.SetActive(false);
        highScroceScreen.SetActive(false);
        src.clip = sound1;
        DoSound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoOpenScene(GameObject screen) {
        screen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void DoCloseScene() {
        mainScreen.SetActive(true );
        settingScreen.SetActive(false);
        firstTimeScreen.SetActive(false);
        highScroceScreen.SetActive(false);
    }

    public void SetSound() {
        isEnableSound = !isEnableSound;
        DoSound();
    }

    public void DoSound() {
        if(isEnableSound == true) {
            src.Play();
        }
        else {
            src.Pause();
        }
    }

}
