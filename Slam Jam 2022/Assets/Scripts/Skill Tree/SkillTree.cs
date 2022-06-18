using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] GameObject Node;
    [SerializeField] GameObject NodeHolder;

    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float scrollMultiplier;

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

    private void Update()
    {
        Zoom();
    }

    void Zoom()
    {
        if (Input.mouseScrollDelta.y == 0)
            return;

        float scale = NodeHolder.transform.localScale.x + (Input.mouseScrollDelta.y * scrollMultiplier);
        scale = Mathf.Clamp(scale, minScale, maxScale);

        NodeHolder.transform.localScale = scale * Vector3.one;
    }
}
