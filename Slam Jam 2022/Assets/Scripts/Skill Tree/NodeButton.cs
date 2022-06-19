using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class NodeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Color ActiveColor;
    [SerializeField] Color AvailableColor;
    [SerializeField] Color HighlightedColor;
    [SerializeField] Image Outline;
    [SerializeField] Image Icon;
    [SerializeField] GameObject infoPrefab;
    GameObject info;
    bool isHighlighted
    {
        get
        {
            return _isHighlighted;
        }
        set
        {
            if (_isHighlighted != value)
            {
                _isHighlighted = value;
                ChangeHighlight();
            }
        }
    }

    bool _isHighlighted;


    public Node Node
    {
        get
        {
            return node;
        }
        set
        {
            node = value;
            Initialise();
        }
    }

    [ReadOnly, SerializeField] Node node;

    private void Start()
    {
        SkillTree.inputField.onValueChanged.AddListener(delegate { CheckIfHighlighted(SkillTree.inputField.text); });
        ChangeHighlight();
        node.OnActiveChanged += ChangeHighlight;
    }

    private void OnDestroy()
    {
        node.OnActiveChanged -= ChangeHighlight;
    }

    void Initialise()
    {
        Icon.sprite = node.icon;
        GetComponent<Button>().onClick.AddListener(() => node.ToggleNode());
        
        GetComponent<RectTransform>().localScale = node.size * Vector3.one;
        GetComponent<RectTransform>().anchoredPosition = node.coordinates * 100;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(info)
        {
            info.SetActive(true);
        }
        else
        {
            info = Instantiate(infoPrefab, transform.parent.parent);

            NodeDescription n = info.GetComponent<NodeDescription>();

            n.SpawnTitle(node.Name);

            for (int i = 0; i < node.powerUps.Count; i++)
            {
                n.SpawnDescription(node.powerUps[i].Description);
            }
        }

        ResetInfoPos();
    }

    void ResetInfoPos()
    {
        RectTransform transform = GetComponent<RectTransform>();
        Vector3 nodePos = transform.anchoredPosition;

        if (Input.mousePosition.x < Screen.width * 2 / 3)
            nodePos.x += info.GetChild(0).GetComponent<RectTransform>().rect.width / 2 + transform.rect.width;
        else
            nodePos.x -= info.GetChild(0).GetComponent<RectTransform>().rect.width / 2 + transform.rect.width;

        if (Input.mousePosition.y < Screen.height / 4)
            nodePos.y += info.GetChild(0).GetComponent<RectTransform>().rect.height + ((info.ChildCount() - 1) * info.GetChild(1).GetComponent<RectTransform>().rect.height) - transform.rect.height;

        info.GetComponent<RectTransform>().anchoredPosition = nodePos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (info)
            info.SetActive(false);
    }

    void CheckIfHighlighted(string str)
    {
        str = str.ToUpper();

        bool IsHighlighted = false;


        if (node.Name.ToUpper().Contains(str))
            IsHighlighted = true;

        for (int i = 0; i < node.powerUps.Count; i++)
        {
            if (node.powerUps[i].Description.ToUpper().Contains(str))
                IsHighlighted = true;
        }

        if (str.Length == 0)
            IsHighlighted = false;

        isHighlighted = IsHighlighted;
    }

    public void ChangeHighlight(bool dummyBool = false)
    {
        if (isHighlighted)
            Outline.color = HighlightedColor;
        else if (node.IsActive)
            Outline.color = ActiveColor;
        else if (node.CanBeToggledOn())
            Outline.color = AvailableColor;
        else
            Outline.color = Color.white;
    }
}
