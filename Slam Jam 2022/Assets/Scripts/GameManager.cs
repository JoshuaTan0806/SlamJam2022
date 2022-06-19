using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    private const string SAVE_INVENTORY_ID = "I";
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
        Items.ItemInventory.Load(PlayerPrefs.GetString(SAVE_INVENTORY_ID, null));
    }

    [Button]
    public static void Save()
    {
        PlayerPrefs.SetInt("SkillPoints", Player.instance.skillPoints);
        PlayerPrefs.SetString(SAVE_INVENTORY_ID, Items.ItemInventory.Save());
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
