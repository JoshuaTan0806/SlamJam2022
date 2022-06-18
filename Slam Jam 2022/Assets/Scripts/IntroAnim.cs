using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject HUD;

    public void IntroFinished()
    {
        HUD.SetActive(true);
        Time.timeScale = 1f;
        MenuManager.instance.gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MenuManager.instance.gameStarted = true;
        introPanel.SetActive(false);
    }
}