using UnityEngine;

//[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    float mode;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameManagerScript.Instance != null)
        {
            mode = GameManagerScript.Instance.GetOutlineMode();
        }

        UpdateOutline(mode);
        
    }

    void OnDisable()
    {
        UpdateOutline(0);
    }

    void LateUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = new Ray(mousePos, Vector3.back);

        if (spriteRenderer.bounds.IntersectRay(ray))
        {
            if(GameManagerScript.Instance != null)
            {
                mode = GameManagerScript.Instance.GetOutlineMode();
            }
            UpdateOutline(mode);
        }
        else
            UpdateOutline(0);
    }

    void UpdateOutline(float mode)
    {
        Color color = new Color();
        if(mode != 0 && GameManagerScript.Instance != null)
        {
            color = GameManagerScript.Instance.spriteOutlineColor;
        }

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", mode);
        mpb.SetColor("_OutlineColor", color);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
