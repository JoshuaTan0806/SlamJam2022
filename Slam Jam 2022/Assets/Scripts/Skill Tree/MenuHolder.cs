using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHolder : MonoBehaviour
{
    [SerializeField] GameObject Title;
    [SerializeField] GameObject Description;

    public void SpawnTitle(string str)
    {
        GameObject g = Instantiate(Title, transform);
        g.GetComponentInChildren<TextMeshProUGUI>().SetText(str);
    }

    public void SpawnDescription(string str)
    {
        GameObject g = Instantiate(Description, transform);
        g.GetComponentInChildren<TextMeshProUGUI>().SetText(str);
    }
}
