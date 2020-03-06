using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Pipe Asset", menuName = "Custom Assets/Puzzle/Pipe Tile")]
public class PipeAsset : ScriptableObject
{
    //public bool canRotate;
    [SerializeField] public Sprite sprite = null;
    [SerializeField] public Sprite activeSprite = null;

    public bool top = false;
    public bool right = false;
    public bool bottom = false;
    public bool left = false;

    public int ValidateFlowMask(bool top, bool right, bool bottom, bool left)
    {
        int rotation = 0;
        bool maskTop = top;
        bool maskRight = right;
        bool maskBottom = bottom;
        bool maskLeft = left;

        for (int i = 0; i < 5; i++)
        {
            //Check flow
            if (maskTop == this.top & maskRight == this.right & maskBottom == this.bottom & maskLeft == this.left)
            {
                return rotation;
            }

            bool temp = maskTop;
            maskTop = maskRight;
            maskRight = maskBottom;
            maskBottom = maskLeft;
            maskLeft = temp;

            rotation += 90;
        }
        return -1;
    }

    public GameObject InstantiatePipe(Vector3 position)
    {
        GameObject newPipe = new GameObject(string.Format("PipeTile ({0},{1})", position.x, position.y));
        newPipe.transform.position = position;
        SpriteRenderer spriteRenderer = newPipe.AddComponent<SpriteRenderer>();

        BoxCollider2D collider = newPipe.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1.0f, 1.0f);

        PipeTile pipeTile = newPipe.AddComponent<PipeTile>();
        pipeTile.SetRotationMask(this.top, this.right, this.bottom, this.left);

        pipeTile.sprite = sprite;
        pipeTile.activeSprite = activeSprite;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = pipeTile.sprite;
        }
        return newPipe;
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(PipeAsset))]
public class PipeAssetEditor : Editor
{
    private PipeAsset targetTile;
    public override void OnInspectorGUI()
    {
        if(targetTile == null)
        {
            targetTile = (PipeAsset)target;
        }

        targetTile.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite:", targetTile.sprite, typeof(Sprite), false);
        targetTile.activeSprite = (Sprite)EditorGUILayout.ObjectField("Active Sprite:", targetTile.activeSprite, typeof(Sprite), false);

        GUILayout.Label("Flow configuration");
        GUILayout.BeginHorizontal();
        GUILayout.Toggle(false, "");
        targetTile.top = GUILayout.Toggle(targetTile.top, "");
        GUILayout.Toggle(false, "");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        targetTile.left = GUILayout.Toggle(targetTile.left, "");
        GUILayout.Toggle(false, "");
        targetTile.right = GUILayout.Toggle(targetTile.right, "");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Toggle(false, "");
        targetTile.bottom = GUILayout.Toggle(targetTile.bottom, "");
        GUILayout.Toggle(false, "");
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
    }
}
#endif
#endregion