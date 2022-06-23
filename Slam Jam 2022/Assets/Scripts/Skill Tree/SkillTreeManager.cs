using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;
    [SerializeField] Button openSkillTree;
    public List<Node> nodes;
    public static List<Node> allNodes;
    static List<Node> startingNodes;
    public static bool showCoordinates = false;

    public static Dictionary<NodeType, float> NodeTypeToSize = new Dictionary<NodeType, float>()
    {
        { NodeType.Minor, 0.5f },
        { NodeType.Notable, 1f },
        { NodeType.Keystone, 2f }

    };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        allNodes = new List<Node>();
        allNodes = nodes;

        openSkillTree.onClick.AddListener(() => ToggleSkillTree());

        startingNodes = new List<Node>();

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i].isStartingNode)
                startingNodes.Add(allNodes[i]);
        }
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

    public static bool StartingNodeTaken()
    {
        for (int i = 0; i < startingNodes.Count; i++)
        {
            if (startingNodes[i].IsActive)
                return true;
        }

        return false;
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

#if UNITY_EDITOR
    [Button("Reinitialise Nodes")]
    public void ReinitialiseNodes()
    {
        nodes.Clear();
        List<Node> nodesInProject = EditorExtensionMethods.GetAllInstances<Node>();
        nodes = nodesInProject;
    }

    [Button("Hook up all nodes")]
    public void HookUpAllNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes[i].connectedNodes.Count; j++)
            {
                if(!nodes[i].connectedNodes[j].connectedNodes.Contains(nodes[i]))
                {
                    nodes[i].connectedNodes[j].connectedNodes.Add(nodes[i]);
                }
            }

            EditorExtensionMethods.SaveAsset(nodes[i]);
        }
    }
#endif
}
