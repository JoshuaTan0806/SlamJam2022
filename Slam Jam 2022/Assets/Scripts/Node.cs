using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Skill Tree/Node")]
public class Node : ScriptableObject
{
    public Sprite icon;
    public Vector2 coordinates;
    public float size = 1;
    public bool isStartingNode = false;
    public List<PowerUp> powerUps = new List<PowerUp>();
    [SerializeField] List<Node> connectedNodes;

    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            if(value != isActive)
            {
                isActive = value;
                ApplyPowerUps(value);
                SkillTreeManager.instance.RefreshSkillTree();
            }
        }
    }
    bool isActive;

    public bool CanBeToggledOn()
    {
        if (IsActive)
            return false;

        if (isStartingNode)
            return true;

        for (int i = 0; i < connectedNodes.Count; i++)
        {
            if (connectedNodes[i].isActive)
                return true;
        }

        return false;
    }

    public bool CanBeToggledOff()
    {
        if (!IsActive)
            return false;

        //add active connected nodes to a list
        List<Node> activeConnectedNodes = new List<Node>();

        for (int i = 0; i < connectedNodes.Count; i++)
        {
            if (connectedNodes[i].isActive)
                activeConnectedNodes.Add(connectedNodes[i]);
        }

        //loop through all active connected nodes to see if they will still be connected to the start
        for (int i = activeConnectedNodes.Count - 1; i >= 0; i--)
        {
            bool didTurnOff = false;

            List<Node> checkedNodes = new List<Node>();
            List<Node> nodesToCheck = new List<Node>();

            nodesToCheck.Add(activeConnectedNodes[i]);

            while (nodesToCheck.Count > 0)
            {
                Node nodeBeingChecked = nodesToCheck[0];

                nodesToCheck.Remove(nodeBeingChecked);
                checkedNodes.Add(nodeBeingChecked);

                if (nodeBeingChecked.isStartingNode && nodeBeingChecked.isActive)
                {
                    activeConnectedNodes.Remove(activeConnectedNodes[i]);
                    didTurnOff = true;

                    if (activeConnectedNodes.Count == 0)
                        return true;

                    break;
                }

                for (int j = 0; j < nodeBeingChecked.connectedNodes.Count; j++)
                {
                    if (nodeBeingChecked.connectedNodes[j] == this)
                        continue;
                    if (!nodeBeingChecked.connectedNodes[j].isActive)
                        continue;
                    if (checkedNodes.Contains(nodeBeingChecked.connectedNodes[j]))
                        continue;
                    if (nodesToCheck.Contains(nodeBeingChecked.connectedNodes[j]))
                        continue;

                    nodesToCheck.Add(nodeBeingChecked.connectedNodes[j]);
                }
            }

            if (!didTurnOff)
                return false;
        }

        if (activeConnectedNodes.Count == 0)
            return true;
        else
            return false;
    }

    public void ToggleNode()
    {
        if (CanBeToggledOn())
            IsActive = true;
        else if (CanBeToggledOff())
            IsActive = false;
    }

    void ApplyPowerUps(bool toggle)
    {
        if (toggle)
        {
            for (int i = 0; i < powerUps.Count; i++)
            {
                powerUps[i].ApplyPowerUp();
            }
        }
        else
        {
            for (int i = 0; i < powerUps.Count; i++)
            {
                powerUps[i].UnapplyPowerUp();
            }
        }
    }
}
