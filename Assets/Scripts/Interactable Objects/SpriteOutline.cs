using UnityEngine;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateOutline(GameManagerScript.instance.GetOutlineMode());
    }

    void OnDisable()
    {
        UpdateOutline(0);
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = new Ray(mousePos, Vector3.back);

        if (spriteRenderer.bounds.IntersectRay(ray))
        {
            UpdateOutline(GameManagerScript.instance.GetOutlineMode());
        }
        else
            UpdateOutline(0);
    }

    void UpdateOutline(float mode)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", mode);
        mpb.SetColor("_OutlineColor", GameManagerScript.instance.spriteOutlineColor);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
