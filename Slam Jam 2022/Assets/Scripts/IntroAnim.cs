using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{
    public GameObject introPanel;
    public LevelLoader levelLoader;

    public void IntroFinished()
    {
        levelLoader.LoadLevel(1);
    }
}