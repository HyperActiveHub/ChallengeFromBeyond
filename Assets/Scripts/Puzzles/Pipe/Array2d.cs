using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Array2D
{
    [SerializeField] private InternalArray[] externalArray;

    public Array2D(int xResolution, int yResolution)
    {
        externalArray = new InternalArray[xResolution];

        for (int i = 0; i < externalArray.Length; i++)
        {
            externalArray[i] = new InternalArray(yResolution);
        }
    }

    [System.Serializable]
    private class InternalArray
    {
        public PipeTile[] pipeTiles;

        public InternalArray(int yResolution)
        {
            pipeTiles = new PipeTile[yResolution];
        }
    }

    public PipeTile GetTile(int x, int y)
    {
        if (x < 0 || x > externalArray.Length - 1)
        {
            return null;
        }

        if (y < 0 || y > externalArray[0].pipeTiles.Length - 1)
        {
            return null;
        }

        return externalArray[x].pipeTiles[y];
    }

    public PipeTile SetTile(int x, int y, PipeTile pipeTile)
    {
        return externalArray[x].pipeTiles[y] = pipeTile;
    }

    public Vector2Int GetPosition(PipeTile pipeTile)
    {
        for (int x = 0; x < externalArray.Length; x++)
        {
            for (int y = 0; y < externalArray[x].pipeTiles.Length; y++)
            {
                if(pipeTile == externalArray[x].pipeTiles[y])
                {
                    return new Vector2Int(x,y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }
}
