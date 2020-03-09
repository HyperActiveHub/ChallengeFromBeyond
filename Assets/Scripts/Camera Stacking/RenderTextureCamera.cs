using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderTextureCamera : MonoBehaviour
{
    [Header("Render Textures")]
    [SerializeField] private RenderTexture rt_low = null;
    [SerializeField] private RenderTexture rt_med = null;
    [SerializeField] private RenderTexture rt_high = null;

    private Camera puzzleCamera;


    private void Start()
    {
        Debug.Log(string.Format("{0}x{1}",Screen.width, Screen.height));
        puzzleCamera = GetComponent<Camera>();
        SetTargetTexture();
    }

    private void SetTargetTexture()
    {
        if (Mathf.Max(Screen.width, Screen.height) < rt_low.width)
        {
            puzzleCamera.targetTexture = rt_low;
            Debug.Log("Setting render texture to low res");
            return;
        }

        if (Mathf.Max(Screen.width, Screen.height) < rt_med.width)
        {
            puzzleCamera.targetTexture = rt_med;
            Debug.Log("Setting render texture to medium res");
            return;
        }

        Debug.Log("Setting render texture to high res");
        puzzleCamera.targetTexture = rt_high;
    }
}
