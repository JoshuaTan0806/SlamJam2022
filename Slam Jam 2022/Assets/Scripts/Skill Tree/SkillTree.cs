using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    [SerializeField] GameObject Node;
    [SerializeField] RectTransform NodeHolder;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float scrollMultiplier;

    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

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
        float zoomSquared = zoom * zoom;

        Vector3 newPos = new Vector3();

        newPos.x = Mathf.Clamp(oldPos.x, minX * zoomSquared, maxX * zoomSquared);
        newPos.y = Mathf.Clamp(oldPos.y, minY * zoomSquared, maxY * zoomSquared);

        if (newPos.x != oldPos.x)
            scrollRect.velocity = new Vector3(0, scrollRect.velocity.y);

        if (newPos.y != oldPos.y)
            scrollRect.velocity = new Vector3(scrollRect.velocity.x, 0);


        NodeHolder.anchoredPosition = newPos;
    }
}
