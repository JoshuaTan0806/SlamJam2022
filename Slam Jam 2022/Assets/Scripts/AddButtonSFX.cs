using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonSFX : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);   
    }

    void PlaySound()
    {
        SoundManager.instance.PlaySound(clip);
    }
}
