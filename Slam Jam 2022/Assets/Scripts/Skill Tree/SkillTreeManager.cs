using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;
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
    }

    private void Start()
    {
        InitialiseSkillTree();
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
        else
            Destroy(SkillTree);
    }

    public void RefreshSkillTree()
    {
        if (SkillTree != null)
            SkillTree.GetComponent<SkillTree>().Refresh();
    }

    void ResetSkillTree()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].IsActive = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ToggleSkillTree();

        if (Input.GetKeyDown(KeyCode.R))
            ResetSkillTree();
    }
}
