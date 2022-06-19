using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTree : MonoBehaviour
{
    [SerializeField] GameObject Node;
    [SerializeField] RectTransform NodeHolder;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TextMeshProUGUI skillPointsLabel;
    public static TMP_InputField inputField;

    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float scrollMultiplier;

    float minX, maxX, minY, maxY;

    private void Awake()
    {
        Populate();
        InitialiseBoundaries();
        SetSkillPointsText();
        inputField = GetComponentInChildren<TMP_InputField>();
    }

    private void OnDestroy()
    {
        Player.OnSkillPointsChanged -= SetSkillPointsText;
    }

    void SetSkillPointsText()
    {
        skillPointsLabel.SetText("Spill points: " + Player.instance.skillPoints.ToString());
    }

    void InitialiseBoundaries()
    {
        minX = Mathf.Infinity;
        maxX = Mathf.NegativeInfinity;
        minY = Mathf.Infinity;
        maxY = Mathf.NegativeInfinity;

        for (int i = 0; i < SkillTreeManager.allNodes.Count; i++)
        {
            if (SkillTreeManager.allNodes[i].coordinates.x < minX)
                minX = SkillTreeManager.allNodes[i].coordinates.x;
            if (SkillTreeManager.allNodes[i].coordinates.x > maxX)
                maxX = SkillTreeManager.allNodes[i].coordinates.x;

            if (SkillTreeManager.allNodes[i].coordinates.y < minY)
                minY = SkillTreeManager.allNodes[i].coordinates.y;
            if (SkillTreeManager.allNodes[i].coordinates.y > maxY)
                maxY = SkillTreeManager.allNodes[i].coordinates.y;
        }

        minX *= 1080/30;
        maxX *= 1080/30;
        minY *= 1920/30;
        maxY *= 1920/30;
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
