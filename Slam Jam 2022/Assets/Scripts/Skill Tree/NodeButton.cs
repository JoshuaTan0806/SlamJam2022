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
    

    void Initialise()
    {
        GetComponent<Image>().sprite = node.icon;
        GetComponent<Button>().onClick.AddListener(() => node.ToggleNode());
        
        GetComponent<RectTransform>().localScale = node.size * Vector3.one;
        GetComponent<RectTransform>().anchoredPosition = node.coordinates * 50;

        if (node.IsActive)
            GetComponent<Image>().color = ActiveColor;
        else if(node.CanBeToggledOn())
            GetComponent<Image>().color = AvailableColor;
    }

    [SerializeField] GameObject infoPrefab;
    GameObject info;

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
            txt.text += node.powerUps[i].Name + "\n";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (info)
            Destroy(info);
    }
}
