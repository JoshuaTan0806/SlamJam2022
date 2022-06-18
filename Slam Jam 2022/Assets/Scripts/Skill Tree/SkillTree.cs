using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] GameObject Node;
    [SerializeField] GameObject NodeHolder;

    private void Awake()
    {
        Populate();
    }

    public void Refresh()
    {
        Destroy();
        Populate();
    }

    void Populate()
    {
        for (int i = 0; i < SkillTreeManager.allNodes.Count; i++)
        {
            GameObject g = Instantiate(Node, NodeHolder.transform);
            g.GetComponentInChildren<NodeButton>().Node = SkillTreeManager.allNodes[i];
        }
    }

    void Destroy()
    {
        for (int i = NodeHolder.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(NodeHolder.transform.GetChild(i).gameObject);
        }
    }
}
