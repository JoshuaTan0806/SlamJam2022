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

    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    public float tempTimeScale = 0f;

    public bool gameIsPaused = true;
    public bool gameStarted = false;
    public GameObject pauseMenuUI;
    public GameObject SoundUI;

    public void Start()
    {
        Time.timeScale = 0f;
        CheckMixerValues();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && gameIsPaused==false) Pause();
        else if (Input.GetButtonDown("Cancel") && gameStarted ==true) Resume();
    }

    public void NewGame()
    {
        //Intro panel then start game
        
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
}
