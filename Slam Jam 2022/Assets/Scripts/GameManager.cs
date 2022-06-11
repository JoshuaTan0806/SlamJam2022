using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        StartCoroutine(AutoSave());
    }

    private void Start()
    {
        Load();
    }

    IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(60);
            Save();
        }
    }

    void Load()
    {
        Player.instance.skillPoints = PlayerPrefs.GetInt("SkillPoints");
    }

    [Button]
    public static void Save()
    {
        PlayerPrefs.SetInt("SkillPoints", Player.instance.skillPoints);
    }
}
