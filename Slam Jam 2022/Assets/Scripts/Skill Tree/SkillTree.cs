using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTree : MonoBehaviour
{
    public static float coordinateMultiplier = 50;
    [SerializeField] GameObject Node;
    [SerializeField] RectTransform NodeHolder;
    [SerializeField] GameObject line;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TextMeshProUGUI skillPointsLabel;
    public static TMP_InputField inputField;
    [SerializeField] Button close;
    [SerializeField] Button reset;
    [SerializeField] Button statsButton;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float scrollMultiplier;
    [SerializeField] GameObject StatholderPos;

    List<NodeButton> NodeButtons = new List<NodeButton>();
    List<Node> AddedNodes = new List<Node>();

    private void Awake()
    {
        Populate();
        SetSkillPointsText();
        inputField = GetComponentInChildren<TMP_InputField>();
        Player.OnSkillPointsChanged += SetSkillPointsText;
        reset.onClick.AddListener(() => SkillTreeManager.instance.ResetSkillTree());
        close.onClick.AddListener(() => SkillTreeManager.instance.ToggleSkillTree());
        statsButton.onClick.AddListener(() => ToggleStatsMenu());
    }

    private void OnDestroy()
    {
        Player.OnSkillPointsChanged -= SetSkillPointsText;
    }

    void SetSkillPointsText()
    {
        skillPointsLabel.SetText("Spill points: " + Player.instance.skillPoints.ToString());
    }

    void Populate()
    {
        for (int i = 0; i < SkillTreeManager.allNodes.Count; i++)
        {
            GameObject g = Instantiate(Node, NodeHolder.transform);
            NodeButton n = g.GetComponentInChildren<NodeButton>();
            n.Node = SkillTreeManager.allNodes[i];
            NodeButtons.Add(n);

            AddedNodes.Add(SkillTreeManager.allNodes[i]);

            for (int j = 0; j < SkillTreeManager.allNodes[i].connectedNodes.Count; j++)
            {
                if (AddedNodes.Contains(SkillTreeManager.allNodes[i].connectedNodes[j]))
                    continue;

                GameObject l = Instantiate(line, NodeHolder);
                l.transform.SetAsFirstSibling();
                RectTransform r = l.GetComponent<RectTransform>();
                Vector2 linePos = SkillTreeManager.allNodes[i].coordinates + SkillTreeManager.allNodes[i].connectedNodes[j].coordinates;
                linePos = coordinateMultiplier * linePos / 2;
                r.anchoredPosition = linePos;
                r.right = SkillTreeManager.allNodes[i].connectedNodes[j].coordinates - SkillTreeManager.allNodes[i].coordinates;
                r.localScale = new Vector3(Vector3.Distance(SkillTreeManager.allNodes[i].connectedNodes[j].coordinates, SkillTreeManager.allNodes[i].coordinates) * coordinateMultiplier, r.localScale.y);
            }
        }
    }

    private void Update()
    {
        Zoom();
        Clamp();
    }

    void Zoom()
    {
        if (Input.mouseScrollDelta.y == 0)
            return;

        float scale = NodeHolder.transform.localScale.x + (Input.mouseScrollDelta.y * scrollMultiplier);
        scale = Mathf.Clamp(scale, minScale, maxScale);

        NodeHolder.transform.localScale = scale * Vector3.one;
    }

    void Clamp()
    {
        Vector3 oldPos = NodeHolder.anchoredPosition;
        float zoom = NodeHolder.localScale.x;

        float xCoordinate = (zoom - 0.5f) * 1250;

        Vector3 newPos = new Vector3();
        newPos.x = Mathf.Clamp(oldPos.x, -xCoordinate, xCoordinate);
        newPos.y = Mathf.Clamp(oldPos.y, -xCoordinate - 300, xCoordinate + 300);

        NodeHolder.anchoredPosition = newPos;

        if (newPos.x != oldPos.x)
            scrollRect.velocity = new Vector3(0, scrollRect.velocity.y);

        if (newPos.y != oldPos.y)
            scrollRect.velocity = new Vector3(scrollRect.velocity.x, 0);
    }

    public void Refresh()
    {
        for (int i = 0; i < NodeButtons.Count; i++)
        {
            NodeButtons[i].ChangeHighlight();
        }

        if(statHolderReference)
        {
            ToggleStatsMenu(false);
            ToggleStatsMenu(true);
        }
    }

    public GameObject statHolderPrefab;
    GameObject statHolderReference;
    public void ToggleStatsMenu()
    {
        if (statHolderReference)
        {
            SoundManager.instance.PlaySFX(SoundManager.instance.cancelSFX);
            Destroy(statHolderReference);
        }
        else
        {
            SoundManager.instance.PlaySFX(SoundManager.instance.confirmSFX);
            statHolderReference = Instantiate(statHolderPrefab, StatholderPos.transform);
            MenuHolder statsHolder = statHolderReference.GetComponent<MenuHolder>();
            statsHolder.SpawnTitle("Stats");

            foreach (KeyValuePair<Stat, StatData> stat in StatManager.StatDictionary)
            {
                StatData statData = Player.instance.GetStat(stat.Key);

                if (statData.FlatValue != 0)
                {
                    statsHolder.SpawnDescription(statData.FlatValue + " to " + StatManager.StatDictionary[stat.Key].InGameName);
                }

                if (statData.PercentValue != 0)
                {
                    statsHolder.SpawnDescription(statData.PercentValue + "% increased " + StatManager.StatDictionary[stat.Key].InGameName);
                }

                if (statData.FinalMultiplier != 1)
                {
                    statsHolder.SpawnDescription(statData.FinalMultiplier + "x " + StatManager.StatDictionary[stat.Key].InGameName);
                }
            }
        }
    }

    void ToggleStatsMenu(bool on)
    {
        if (on)
        {
            SoundManager.instance.PlaySFX(SoundManager.instance.confirmSFX);
            statHolderReference = Instantiate(statHolderPrefab, StatholderPos.transform);
            MenuHolder statsHolder = statHolderReference.GetComponent<MenuHolder>();
            statsHolder.SpawnTitle("Stats");

            foreach (KeyValuePair<Stat, StatData> stat in StatManager.StatDictionary)
            {
                StatData statData = Player.instance.GetStat(stat.Key);

                if (statData.FlatValue != 0)
                {
                    statsHolder.SpawnDescription(statData.FlatValue + " to " + StatManager.StatDictionary[stat.Key].InGameName);
                }

                if (statData.PercentValue != 0)
                {
                    statsHolder.SpawnDescription(statData.PercentValue + "% increased " + StatManager.StatDictionary[stat.Key].InGameName);
                }

                if (statData.FinalMultiplier != 0)
                {
                    statsHolder.SpawnDescription(statData.FinalMultiplier + "x " + StatManager.StatDictionary[stat.Key].InGameName);
                }
            }
        }
        else
        {
            if (statHolderReference)
            {
                SoundManager.instance.PlaySFX(SoundManager.instance.cancelSFX);
                Destroy(statHolderReference);
            }
        }
    }
}
