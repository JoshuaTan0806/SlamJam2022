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
        Vector3 offset;

        if (Input.mousePosition.x < Screen.width/2)
            offset = new Vector3(100, 30, 0);
        else
            offset = new Vector3(-100, 30, 0);

        info = Instantiate(infoPrefab, transform.position + offset, Quaternion.identity, transform);

        TMPro.TextMeshProUGUI txt = info.GetComponent<TMPro.TextMeshProUGUI>();
        txt.text = "";
        for (int i = 0; i < node.powerUps.Count; i++)
        {
            txt.text += node.powerUps[i].Description + "\n";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (info)
            Destroy(info);
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

    void ChangeHighlight(bool dummyBool = false)
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
