using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipePuzzle : MonoBehaviour
{
    public Vector2Int boardGridSize;
    private int[,] gameBoard;

    [Header("References")]
    [SerializeField] private Camera puzzleCamera = null;
    [SerializeField] private PipeTile inflow = null;
    [SerializeField] private PipeTile outflow;

    [SerializeField] private Array2D pipeTiles;

    private void OnValidate()
    {
        if(puzzleCamera == null){
            Debug.LogError("Please attach a camera to this pipe puzzle!", this);
        }

        if (inflow == null){
            Debug.LogError("Please attach a inflow pipe tile to this pipe puzzle!", this);
        }
    }

    private void ValidateFlow()
    {
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

        if(inflow != null)
        {
            inflow.SetFlowState(true);
        }

        RecursiveValidation(inflow);
    }

    private void RecursiveValidation(PipeTile pipeTile)
    {
        PipeTile[,] surroundingTiles = GetSurroundingTiles(pipeTiles.GetPosition(pipeTile));

        //top
        if (pipeTile.top)
        {
            if (surroundingTiles[1, 2] != null)
            {
                if (surroundingTiles[1, 2].ValidateConnection("bottom"))
                {
                    RecursiveValidation(surroundingTiles[1, 2]);
                }
            }
        }
        //right
        if (pipeTile.right)
        {
            if (surroundingTiles[2, 1] != null)
            {
                if (surroundingTiles[2, 1].ValidateConnection("left"))
                {
                    RecursiveValidation(surroundingTiles[2, 1]);
                }
            }
        }
        //bottom
        if (pipeTile.bottom)
        {
            if (surroundingTiles[1, 0] != null)
            {
                if (surroundingTiles[1, 0].ValidateConnection("top"))
                {
                    RecursiveValidation(surroundingTiles[1, 0]);
                }
            }
        }
        //left
        if (pipeTile.left)
        {
            if (surroundingTiles[0, 1] != null)
            {
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
        //if (pipeTiles == null)
        //{
        //    this.pipeTiles = new PipeTile[gridSize.x, gridSize.y];
        //    Debug.Log("pipeTiles Initialized!");
        //}

        pipeTiles = new Array2D(gridSize.x, gridSize.y);
    }

    public void InsertPipeTile(PipeTile pipeTile, Vector2Int position)
    {
        pipeTiles.SetTile(position.x, position.y, pipeTile);
    }

    public void PrintInfo()
    {
        //Debug.Log(pipeTiles2[0].pipeTileArray[0]);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(puzzleCamera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            Debug.DrawRay(puzzleCamera.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Color.red, 4.0f);

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
