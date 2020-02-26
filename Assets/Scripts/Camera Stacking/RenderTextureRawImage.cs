using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RenderTextureRawImage : MonoBehaviour
{
    [Header("Render Textures")]
    [SerializeField] private RenderTexture rt_low = null;
    [SerializeField] private RenderTexture rt_med = null;
    [SerializeField] private RenderTexture rt_high = null;

    private RawImage image;


    private void Start()
    {
        image = GetComponent<RawImage>();
        SetTargetTexture();
    }

    private void SetTargetTexture()
    {
        if (Mathf.Max(Screen.width, Screen.height) < rt_low.width)
        {
            image.texture = rt_low;
            return;
        }

        if (Mathf.Max(Screen.width, Screen.height) < rt_med.width)
        {
            image.texture = rt_med;
            return;
        }

        image.texture = rt_high;
    }
}
