using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTree : MonoBehaviour
{
    public static float coordinateMultiplier = 100;
    [SerializeField] GameObject Node;
    [SerializeField] RectTransform NodeHolder;
    [SerializeField] GameObject line;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TextMeshProUGUI skillPointsLabel;
    public static TMP_InputField inputField;
    [SerializeField] Button close;
    [SerializeField] Button reset;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float scrollMultiplier;

    List<NodeButton> NodeButtons = new List<NodeButton>();
    float minX, maxX, minY, maxY;

    private void Awake()
    {
        Populate();
        InitialiseBoundaries();
        SetSkillPointsText();
        inputField = GetComponentInChildren<TMP_InputField>();
        Player.OnSkillPointsChanged += SetSkillPointsText;
        reset.onClick.AddListener(() => SkillTreeManager.instance.ResetSkillTree());
        close.onClick.AddListener(() => SkillTreeManager.instance.ToggleSkillTree());
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

    void Populate()
    {
        for (int i = 0; i < SkillTreeManager.allNodes.Count; i++)
        {
            GameObject g = Instantiate(Node, NodeHolder.transform);
            NodeButton n = g.GetComponentInChildren<NodeButton>();
            n.Node = SkillTreeManager.allNodes[i];
            NodeButtons.Add(n);

            for (int j = 0; j < SkillTreeManager.allNodes[i].connectedNodes.Count; j++)
            {
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

    public void Refresh()
    {
        for (int i = 0; i < NodeButtons.Count; i++)
        {
            NodeButtons[i].ChangeHighlight();
        }
    }
}
