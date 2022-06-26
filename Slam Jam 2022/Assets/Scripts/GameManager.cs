using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : MonoBehaviour
{
    private const string SAVE_INVENTORY_ID = "I";
    public static GameManager instance;
    public int skillPointGainPerLevel = 5;
    public int skillPointsToLevel = 10;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        StartCoroutine(AutoSave());
        DontDestroyOnLoad(this);
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
    /// <summary>
    /// Call when starting a new game
    /// </summary>
    public static void NewGame()
    {   //Clear all save data
        PlayerPrefs.DeleteAll();
        Items.ItemInventory.Clear();
        Items.ItemRewarder.Clear();
        //Grant the player a reward
        Items.ItemRewarder.GrantReward(0);
    }
    /// <summary>
    /// Call when a new level starts
    /// </summary>
    public static void LevelStart()
    {
        Player.instance.InitialisePotions();
    }
    /// <summary>
    /// Call when the player succeeds a level
    /// </summary>
    public static void LevelFinish()
    {   //Grant player skill points
        Player.instance.skillPoints += instance.skillPointGainPerLevel;
        //Grant the player a scaling reward
        Items.ItemRewarder.GrantReward(Player.instance.skillPoints / instance.skillPointsToLevel);
    }
}
