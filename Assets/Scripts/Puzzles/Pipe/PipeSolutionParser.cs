using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEditor.UI;

[CreateAssetMenu(fileName = "New Puzzle Layout", menuName = "Custom Assets/Puzzle/Pipe Puzzle Layout")]
public class PipeSolutionParser : ScriptableObject
{
    [HideInInspector] public Texture2D selectedTexture;
    [HideInInspector] public Vector2Int imageResolution;
    [HideInInspector] public List<TileColor> tiles = null;
    [HideInInspector] public PipeAsset[,] tileGrid;
    public List<PipeAsset> pipeAssets;

    [HideInInspector] public List<Color> uniqueColors;
    [HideInInspector] public List<TileColor.TILE> tileInfo;

    [HideInInspector] public PipeTile[,] pipeTiles = null;

    private void OnEnable()
    {
        //selectedTexture = new Texture2D(32,32);
    }

}

[System.Serializable]
public class TileColor
{
    public Color color;
    public TILE tileType;

    public enum TILE
    {
        NONE,
        PIPE,
        PIPE_NO_ROTATION,
        INFLOW,
        OUTFLOW_NORMAL,
        OUTFLOW_INSIGHT
    }

    public TileColor(Color color, TILE tileType)
    {
        this.color = color;
        this.tileType = tileType;
    }
}

#region Editor
//#if UNITY_EDITOR
[CustomEditor(typeof(PipeSolutionParser))]
public class PipeSolutionParserEditor : Editor
{
    private PipeSolutionParser psp;
    private Texture2D temporaryTexture;

    private void OnValidate()
    {
        if (psp == null)
        {
            psp = (PipeSolutionParser)target;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (psp == null)
        {
            psp = (PipeSolutionParser)target;
        }

        if (GUILayout.Button("Reset"))
        {
            ClearImage();
        }

        psp.selectedTexture = (Texture2D)EditorGUILayout.ObjectField("Solution Texture:", psp.selectedTexture, typeof(Texture2D), false);

        if(psp.selectedTexture != null && psp.selectedTexture.width != psp.imageResolution.x)
        {
            ReadFile();
        }

        if (psp.imageResolution.x != 0 || psp.imageResolution.y != 0)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Format("Resolution: {0}x{1} px", psp.imageResolution.x, psp.imageResolution.y));
            EditorGUILayout.LabelField(string.Format("Grid Size: {0}x{1} tiles", psp.imageResolution.x/3, psp.imageResolution.y/3));
            GUILayout.EndHorizontal();

            for (int i = 0; i < psp.uniqueColors.Count; i++)
            {
                GUILayout.BeginHorizontal();
                psp.uniqueColors[i] = EditorGUILayout.ColorField(psp.uniqueColors[i]);
                psp.tileInfo[i] = (TileColor.TILE)EditorGUILayout.EnumPopup("",psp.tileInfo[i]);
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
 
            if (GUILayout.Button("Save & Generate grid"))
            {
                Save();
                CreateEmptyTileArray();
                CreateEmptyPipeTiles();
                GeneratePuzzleLayout();
            }
        }

        if(GUILayout.Button("Instantiate GameObjects"))
        {
            GameObject pipePuzzleGO = new GameObject("Pipe Puzzle");
            PipePuzzle pipePuzzleComponent = pipePuzzleGO.AddComponent<PipePuzzle>();
            pipePuzzleComponent.Initialize(new Vector2Int(psp.tileGrid.GetLength(0), psp.tileGrid.GetLength(1)));

            pipePuzzleComponent.boardGridSize = new Vector2Int(psp.imageResolution.x / 3, psp.imageResolution.y / 3);

            for (int y = 0; y < psp.tileGrid.GetLength(1); y++)
            {
                for (int x = 0; x < psp.tileGrid.GetLength(0); x++)
                {
                    if(psp.tileGrid[x,y] == null)
                    {
                        continue;
                    }

                    GameObject newPipe = psp.tileGrid[x, y].InstantiatePipe(new Vector3(x,y,0));
                    newPipe.transform.parent = pipePuzzleGO.transform;
                    PipeTile pipeTile = newPipe.GetComponent<PipeTile>();

                    //TODO: ALLOW MANUAL ROTATION.
                    //for (int i = 0; i < Random.Range(0,5); i++)
                    //{
                    //    pipeTile.Rotate();
                    //}
                    pipePuzzleComponent.InsertPipeTile(pipeTile, new Vector2Int(x,y));
                    psp.pipeTiles[x, y] = pipeTile;
                }
            }
        }

        //DRAW ONLY IF VALUES ARE SAVED
        if (psp.tiles != null)
        {
            EditorGUILayout.Space();
            GUI.enabled = false;
            EditorGUILayout.LabelField("Saved values:");
            for (int i = 0; i < psp.tiles.Count; i++)
            {
                GUILayout.BeginHorizontal();
                psp.tiles[i].color = EditorGUILayout.ColorField(psp.tiles[i].color);
                EditorGUILayout.LabelField(string.Format("Tile type: {0}", psp.tiles[i].tileType));
                GUILayout.EndHorizontal();
            }
            GUI.enabled = true;
        }
    }

    private void GeneratePuzzleLayout()
    {
        for (int y = 0; y < (psp.imageResolution.y / 3); y++)
        {
            for (int x = 0; x < (psp.imageResolution.x / 3); x++)
            {
                Color[] currentTile = psp.selectedTexture.GetPixels(x * 3, y * 3, 3, 3);
                PipeAsset generatedPipeAsset = GenerateTileFromPixels(currentTile);
                //if(generatedPipeAsset == null)
                //{
                //    Debug.LogWarning(string.Format("Tile X:{0} Y:{1} could not be recognized as a pipe. Was this intended? :^)", x, y), this);
                //}
                psp.tileGrid[x, y] = generatedPipeAsset;
            }
        }
    }

    private PipeAsset GenerateTileFromPixels(Color[] pixels)
    {
        //make sure we received a 3x3 grid.
        if(pixels.Length != 9)
        {
            return null;
        }

        bool topFlow = false;
        bool rightFlow = false;
        bool bottomFlow = false;
        bool leftFlow = false;

        for (int i = 0; i < psp.uniqueColors.Count; i++)
        {
            //Bottom middle
            if (pixels[1] == psp.uniqueColors[i] & psp.tileInfo[i] == TileColor.TILE.PIPE)
            {
                bottomFlow = true;
            }

            //Middle left
            if (pixels[3] == psp.uniqueColors[i] & psp.tileInfo[i] == TileColor.TILE.PIPE)
            {
                leftFlow = true;
            }

            //Middle right
            if (pixels[5] == psp.uniqueColors[i] & psp.tileInfo[i] == TileColor.TILE.PIPE)
            {
                rightFlow = true;
            }

            //Top middle
            if (pixels[7] == psp.uniqueColors[i] & psp.tileInfo[i] == TileColor.TILE.PIPE)
            {
                topFlow = true;
            }

            //mask is ok, now time to see what pipe asset should be placed on that tile
            for (int j = 0; j < psp.pipeAssets.Count; j++)
            {
                //ValidateFlowMask will return a rotation. -1 means no valid pipe asset was found.
                int flowMaskValidation = (int)psp.pipeAssets[j].ValidateFlowMask(topFlow, rightFlow, bottomFlow, leftFlow);
                if (flowMaskValidation != -1)
                {
                    return psp.pipeAssets[j];
                }
            }

        }
        return null;
    }

    private void CreateEmptyTileArray()
    {
        if(psp.tileGrid == null)
        {
            psp.tileGrid = new PipeAsset[psp.imageResolution.x/3, psp.imageResolution.y/3];
        }

        if(psp.tileGrid.GetLength(0) != psp.imageResolution.x / 3)
        {
            psp.tileGrid = new PipeAsset[psp.imageResolution.x / 3, psp.imageResolution.y / 3];
        }
        else if (psp.tileGrid.GetLength(1) != psp.imageResolution.y / 3)
        {
            psp.tileGrid = new PipeAsset[psp.imageResolution.x / 3, psp.imageResolution.y / 3];
        }
    }

    private void CreateEmptyPipeTiles()
    {
        if (psp.pipeTiles == null)
        {
            psp.pipeTiles = new PipeTile[psp.imageResolution.x / 3, psp.imageResolution.y / 3];
        }

        if (psp.pipeTiles.GetLength(0) != psp.imageResolution.x / 3)
        {
            psp.pipeTiles = new PipeTile[psp.imageResolution.x / 3, psp.imageResolution.y / 3];
        }
        else if (psp.pipeTiles.GetLength(1) != psp.imageResolution.y / 3)
        {
            psp.pipeTiles = new PipeTile[psp.imageResolution.x / 3, psp.imageResolution.y / 3];
        }
    }

    private void ReadFile()
    {
        psp.imageResolution = new Vector2Int(0, 0);

        if (ValidateInputTexture())
        {
            //psp.selectedTexture = psp.selectedTexture;
            psp.imageResolution = new Vector2Int(psp.selectedTexture.width, psp.selectedTexture.height);

            for (int y = 0; y < psp.selectedTexture.height; y++)
            {
                for (int x = 0; x < psp.selectedTexture.width; x++)
                {
                    Color selectedPixel = psp.selectedTexture.GetPixel(x, y);
                    AddNewColor(selectedPixel);
                }
            }
        }
    }

    private void AddNewColor(Color color)
    {
        if(psp.uniqueColors == null)
        {
            psp.uniqueColors = new List<Color>();
        }

        if (psp.tileInfo == null)
        {
            psp.tileInfo = new List<TileColor.TILE>();
        }

        //Make sure color has not already been added and make sure color is not invisible.
        if (!psp.uniqueColors.Contains(color) && color.a > 0.9f)
        {
            psp.uniqueColors.Add(color);
            psp.tileInfo.Add(TileColor.TILE.NONE);
        }

    }

    private void Save()
    {
        psp.tiles = new List<TileColor>();
        for (int i = 0; i < psp.uniqueColors.Count; i++)
        {
            psp.tiles.Add(new TileColor(psp.uniqueColors[i], psp.tileInfo[i]));
        }
        //psp.imageResolution = psp.imageResolution;
    }

    private bool ValidateInputTexture()
    {
        if (psp == null)
        {
            psp = (PipeSolutionParser)target;
        }

        if (psp.selectedTexture == null)
        {
            Debug.LogError("No texture selected!");
            return false;
        }

        if (psp.selectedTexture.width % 3 != 0 || psp.selectedTexture.height % 3 != 0)
        {
            Debug.LogError("Selected texture height and width must be divisble by 3.");
            ClearImage();
            return false;
        }

        return true;
    }

    private void ClearImage()
    {
        psp.uniqueColors = new List<Color>();
        psp.selectedTexture = null;
        psp.imageResolution = new Vector2Int(0, 0);
    }

}

//#endif
#endregion