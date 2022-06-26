using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHUD : MonoBehaviour
{
    public GameObject[] InputImages;

    private void Start()
    {
        SpillInputManager.InputImages = InputImages;
    }
}
