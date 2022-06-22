using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    RawImage img;
    [SerializeField] float x, y;

    private void Awake()
    {
        img = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(x, y) * Time.deltaTime, img.uvRect.size);
    }
}
