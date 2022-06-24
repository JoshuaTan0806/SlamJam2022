using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour

{
    #region Singleton

    public static MenuManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public AudioMixer audioMixer;
    public AudioSource SFXAudio;

    public Slider masterSlider;
    public Slider sfxSlider;
    bool playSFX = false;
    public Slider musicSlider;

    public float tempTimeScale = 0f;

    public bool gameIsPaused = true;
    public bool gameStarted = false;
    public GameObject pauseMenuUI;
    public GameObject SoundUI;
    public LevelLoader levelLoader;
    public GameObject introPanel;

    public void Start()
    {
        Time.timeScale = 0f;
        CheckMixerValues();
        playSFX = false;
    }

    /*private void Update()
    {
        if (Input.GetButtonDown("Cancel") && gameIsPaused==false) Pause();
        else if (Input.GetButtonDown("Cancel") && gameStarted ==true) Resume();
        
    if (Input.GetMouseButtonUp(0) && playSFX == true)
        {
            SFXAudio.Play();
            playSFX = false;
        }
    }*/

    private void Update()
    {

    }

    public void NewGame()
    {
        if (PlayerPrefs.HasKey("SkillPoints"))
        {
            levelLoader.LoadLevel(1);
        }
        else
        {
            introPanel.SetActive(true);
        }
    }   

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


    public void CheckMixerValues()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

            audioMixer.SetFloat("MasterVol", Mathf.Log10(masterSlider.value) * 20);
            audioMixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
            audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxSlider.value) * 20);
        }
        else
        {
            audioMixer.SetFloat("MasterVol", Mathf.Log10(.75f) * 20);
            audioMixer.SetFloat("MusicVol", Mathf.Log10(.75f) * 20);
            audioMixer.SetFloat("SFXVol", Mathf.Log10(.75f) * 20);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (tempTimeScale == 0f) { tempTimeScale = 1; }
        Time.timeScale = tempTimeScale;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        tempTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {

    }

    public void SetMaster()
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(masterSlider.value) * 20);
        PlayerPrefs.SetFloat("masterVolume", masterSlider.value);
    }

    public void SetMusic()
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
    }
    public void SetSFX()
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        playSFX = true;
    }
}
