using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;
    [SerializeField] Button openSkillTree;
    [SerializeField] List<Node> nodes;
    public static List<Node> allNodes;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        allNodes = new List<Node>();
        allNodes = nodes;

        openSkillTree.onClick.AddListener(() => ToggleSkillTree());
    }

    private void Start()
    {
        InitialiseSkillTree();
        ToggleSkillTree();
        ToggleSkillTree();
    }

    void InitialiseSkillTree()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].IsActive)
                nodes[i].ApplyPowerUps(true);
        }
    }

    [SerializeField] GameObject SkillTreePrefab;
    GameObject SkillTree;

    public void ToggleSkillTree()
    {
        if (SkillTree == null)
            SkillTree = Instantiate(SkillTreePrefab);
        else if (SkillTree.activeInHierarchy)
            SkillTree.SetActive(false);
        else
            SkillTree.SetActive(true);
    }

    public void RefreshSkillTree()
    {
        if (SkillTree != null)
            SkillTree.GetComponent<SkillTree>().Refresh();
    }

    public void ResetSkillTree()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].IsActive)
            {
                Player.instance.skillPoints++;
                nodes[i].IsActive = false;
            }
        }
    }
}
