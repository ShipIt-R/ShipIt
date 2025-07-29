using NUnit.Framework;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.ReorderableList;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerator : MonoBehaviour
{
    public enum Grid
    {
        PATH,
        GRASS,
        EMPTY
    }
    public Grid[,] gridHandler;
    public Tilemap floortilemap;
    public RuleTile pathTile;
    public RuleTile grassTile;
    public int MAPHEIGHT;
    public int MAPWIDTH;
    public int leftisland = 4;
    public int rightisland = 7;
    private WalkerObject walker;
    public Vector2Int walkerdirection = Vector2Int.left;
    public int detour = 10;
    public float curve_Change = 0.2f;
    public float curve_change_increase = 0.1f;
    public List<WalkerObject> Walkers;
    public List<WalkerObject> toRemove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initializeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (Walkers.Count != 0)
        {
            foreach (WalkerObject walker in Walkers)
            {
                if (walker.moving)
                {
                    walkerwalk(walker);
                }
            }
        }

        if (toRemove.Count != 0)
        {
            foreach (WalkerObject walker2 in toRemove)
            {
                Walkers.Remove(walker2);
            }
            toRemove.Clear();
        }
    }

    // floortilemap.SetTile(new Vector3Int(x - gridHandler.GetLength(0) / 2, y - gridHandler.GetLength(1) / 2, 0), pathTile);
    void initializeGrid()
    {
        Walkers = new List<WalkerObject>();
        toRemove = new List<WalkerObject>();
        gridHandler = new Grid[MAPWIDTH, MAPHEIGHT];

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                if (x > MAPWIDTH - rightisland || (x < leftisland - 1 && y < leftisland - 1))
                {
                    gridHandler[x, y] = Grid.PATH;
                } else {
                    gridHandler[x, y] = Grid.EMPTY;
                }
            }
        }
        // walker = new Vector2Int((MAPWIDTH - rightisland), 17);
        for (int i=3; i<18; i+=14)
        {
            walker = new WalkerObject(new Vector2Int(MAPWIDTH - rightisland, i), curve_Change, detour);
            Walkers.Add(walker);
            floortilemap.SetTile(new Vector3Int((MAPWIDTH - rightisland) - gridHandler.GetLength(0) / 2, i - gridHandler.GetLength(1) / 2, 0), pathTile);
        }
    }

    private void walkerwalk(WalkerObject walker)
    {
        walker.position += walker.direction;
        settiles(walker);
        gridHandler[walker.position.x, walker.position.y] = Grid.PATH;
        Vector2Int should_direction;
        do {
            if (UnityEngine.Random.value < walker.curve_change_value)
            {
                should_direction = getDirection(walker);
                if (should_direction.Equals(walker.direction * -1))
                {
                    should_direction *= -1;
                }
                walker.curve_change_value = curve_Change;
            } else
            {
                should_direction = walker.direction;
                walker.curve_change_value += curve_change_increase;
            }

            if ((walker.position + should_direction).x < leftisland && (walker.position + should_direction).y < leftisland)
            {
                walker.moving = false;
                toRemove.Add(walker);
                if (Walkers.Count - toRemove.Count == 0) { setgrass(); }
            }
        } while (checkOutsideBox(walker.position + should_direction));
        walker.direction = should_direction;
    }

    private Vector2Int getDirection(WalkerObject walker)
    {
        if (walker.detour <= 0)
        {
            walker.range_value = 2;
        } else
        {
            walker.range_value = 4;
        }
            switch (UnityEngine.Random.Range(0, walker.range_value))
            {
                case 0:
                    walker.detour += 1;
                    return Vector2Int.left;


                case 1:
                    walker.detour += 1;
                    return Vector2Int.down;

                case 2:
                    walker.detour -= 2;
                    return Vector2Int.right;

                case 3:
                    walker.detour -= 2;
                    return Vector2Int.up;

                default: return Vector2Int.zero;
            }
    }

    private bool checkOutsideBox(Vector2Int pos)
    {
        return !(pos.x > 1 && pos.x < MAPWIDTH - 1 - rightisland && pos.y > 1 && pos.y < MAPHEIGHT - 1);
    }

    private void settiles(WalkerObject walker)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (gridHandler[walker.position.x + x, walker.position.y + y].Equals(Grid.EMPTY))
                {
                    floortilemap.SetTile(new Vector3Int(walker.position.x + x - MAPWIDTH / 2, walker.position.y + y - MAPHEIGHT / 2, 0), pathTile);
                    gridHandler[walker.position.x + x, walker.position.y + y] = Grid.PATH;
                }
            }
        }
    }

    private void setgrass()
    {
        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                if (gridHandler[x, y].Equals(Grid.EMPTY))
                {
                    gridHandler[x, y] = Grid.GRASS;
                    floortilemap.SetTile(new Vector3Int(x - gridHandler.GetLength(0) / 2, y - gridHandler.GetLength(1) / 2, 0), grassTile);
                }
            }
        }
    }
}