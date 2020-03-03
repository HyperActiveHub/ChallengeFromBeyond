using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    [Tooltip("Should this object be illuminated by 2D-Lighting?")]
    public bool UseLighting = true;

    private SpriteRenderer spriteRenderer;
    float mode;
    bool isLit;

    public void SetMaterial(bool isLit)
    {
        if (isLit)
        {
            spriteRenderer.material = GameManagerScript.Instance.litOutlineMat;
        }
        else
        {
            spriteRenderer.material = GameManagerScript.Instance.unlitOutlineMat;
        }
        Debug.Log("Changed material to '" + spriteRenderer.sharedMaterial.name + "' (sprite outline).", this);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        isLit = UseLighting;
        SetMaterial(isLit);
    }

    void OnEnable()
    {
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
        if (isLit != UseLighting)    //If option changed (for instance when an item is in inevntory, it shouldnt be using a lit mat anymore)
        {
            isLit = UseLighting;
            SetMaterial(isLit);
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = new Ray(mousePos, Vector3.back);

        if (spriteRenderer.bounds.IntersectRay(ray))
        {
            if (GameManagerScript.Instance != null)
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
        if (mode != 0 && GameManagerScript.Instance != null)
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
