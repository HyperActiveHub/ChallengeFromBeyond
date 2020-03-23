using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PipePuzzle : MonoBehaviour
{
    [HideInInspector] public Vector2Int boardGridSize;

    [Header("References")]
    [SerializeField] private Camera puzzleCamera = null;
    [SerializeField] private PipeTile inflowPipe = null;
    [SerializeField] private List<PipeTile> outflowPipes = null;
    [SerializeField] private Array2D pipeTiles;

    [SerializeField] public UnityEvent onWin;

    private void OnEnable()
    {
        ValidateFlow();
    }

    private void OnValidate()
    {
        if(puzzleCamera == null){
            Debug.LogError("Please attach a camera to this pipe puzzle!", this);
        }

        if (inflowPipe == null){
            Debug.LogError("Please attach an inflow pipe tile to this pipe puzzle!", this);
        }

        if (outflowPipes == null)
        {
            Debug.LogError("Please attach an outflow pipe tile to this pipe puzzle!", this);
        }
    }

    private void ValidateFlow()
    {
        //Reset previous solution
        for (int y = 0; y < boardGridSize.y; y++)
        {
            for (int x = 0; x < boardGridSize.x; x++)
            {
                PipeTile pipeTile = pipeTiles.GetTile(x, y);
                if(pipeTile != null)
                {
                    pipeTile.SetFlowState(false);
                    pipeTile.alreadyActive = false;
                }
            }
        }

        //Do it all again. TODO: Only validate relevant parts. No need to validate everything again.

        inflowPipe.SetFlowState(true);
        RecursiveValidation(inflowPipe);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        for (int i = 0; i < outflowPipes.Count; i++)
        {
            if (outflowPipes[i].activeFlow)
            {
                if (InsightGlobal.InsightValue >= outflowPipes[i].requiredInsightLevel)
                {
                    onWin.Invoke();
                }
            }
        }
    }
        

    private void RecursiveValidation(PipeTile pipeTile)
    {
        PipeTile[,] surroundingTiles = GetSurroundingTiles(pipeTiles.GetPosition(pipeTile));

        //The tile on TOP of the current tile.
        if (pipeTile.top)
        {
            if (surroundingTiles[1, 2] != null)
            {
                //If the tile above is able to make a connection DOWN.
                if (surroundingTiles[1, 2].ValidateConnection("bottom"))
                {
                    RecursiveValidation(surroundingTiles[1, 2]);
                }
            }
        }
        //The tile to the RIGHT of the current tile.
        if (pipeTile.right)
        {
            if (surroundingTiles[2, 1] != null)
            {
                //If the tile to the right is able to make a connection to the LEFT.
                if (surroundingTiles[2, 1].ValidateConnection("left"))
                {
                    RecursiveValidation(surroundingTiles[2, 1]);
                }
            }
        }
        //The tile on the BOTTOM of the current tile.
        if (pipeTile.bottom)
        {
            if (surroundingTiles[1, 0] != null)
            {
                //If the tile below is able to make a connection UP.
                if (surroundingTiles[1, 0].ValidateConnection("top"))
                {
                    RecursiveValidation(surroundingTiles[1, 0]);
                }
            }
        }
        //The tile to the LEFT of the current tile.
        if (pipeTile.left)
        {
            if (surroundingTiles[0, 1] != null)
            {
                //If the tile to the right is able to make a connection to the RIGHT.
                if (surroundingTiles[0, 1].ValidateConnection("right"))
                {
                    RecursiveValidation(surroundingTiles[0, 1]);
                }
            }
        }
    }

    private PipeTile[,] GetSurroundingTiles(Vector2Int position)
    {
        PipeTile[,] surroundingTiles = new PipeTile[3, 3];

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                PipeTile pipeTile = pipeTiles.GetTile(position.x + i, position.y + j);
                if (pipeTile == null || !pipeTile.alreadyActive)
                {
                    surroundingTiles[i + 1, j + 1] = pipeTile;
                }
                else
                {
                    surroundingTiles[i + 1, j + 1] = null;
                }
            }
        }
        return surroundingTiles;
    }

    public void Initialize(Vector2Int gridSize)
    {
        pipeTiles = new Array2D(gridSize.x, gridSize.y);
    }

    public void InsertPipeTile(PipeTile pipeTile, Vector2Int position)
    {
        pipeTiles.SetTile(position.x, position.y, pipeTile);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(puzzleCamera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);

            if (hit)
            {
                PipeTile pipeTile = hit.collider.gameObject.GetComponent<PipeTile>();
                if(pipeTile != null)
                {
                    pipeTile.Rotate();
                    ValidateFlow();
                }
            }
        }
    }
}
