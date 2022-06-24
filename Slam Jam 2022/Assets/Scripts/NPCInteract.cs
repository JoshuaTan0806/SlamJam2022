using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    [SerializeField] private GameObject NPCPanel;
    [SerializeField] private GameObject player;
    [SerializeField] private Outline npcoutline;

    private void OnMouseDown()
    {
        float distance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);
        Debug.Log("distance" + distance);
        if (distance < 2.5f)
            NPCPanel.SetActive(true);
        else
        {
            NPCPanel.SetActive(false);
        }
    }
    private void OnMouseOver()
    {
        if(npcoutline.enabled==false)
        npcoutline.enabled = true;
    }

    private void OnMouseExit()
    {
        npcoutline.enabled = false;
    }
}
