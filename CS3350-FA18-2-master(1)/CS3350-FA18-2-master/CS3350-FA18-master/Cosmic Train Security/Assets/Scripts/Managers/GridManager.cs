using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// This class manages the pathfinding grid for all of the enemy agents
/// </summary>
public class GridManager : Singleton<GridManager>
{
    /// <summary>
    /// The grid represented by a Tilemap
    /// </summary>
    public GameObject TilemapGrid;
    GameObject[] entrances;

    /// <summary>
    /// Size of the map, +x is "right", +y is "up", z is ignored
    /// </summary>
    public Vector2Int MapSize { get; private set; }
    Vector3Int tilemapOrigin;

    Tilemap mainTilemap;
    PathfindingGrid pathfindingGrid;
    GameObject[] walls;
    GameObject[] tables;

    protected GridManager() { }

    public void Init()
    {
        // Initialize the mapsize to zero
        MapSize = Vector2Int.zero;
        tilemapOrigin = Vector3Int.zero;

        TilemapGrid = GameObject.Find("Grid");
        if (TilemapGrid)
        {
            walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject wall in walls)
            {
                wall.transform.SetParent(TilemapGrid.transform);
            }

            // If there are no tilemaps to process, produce an error
            if (TilemapGrid.GetComponentsInChildren<Tilemap>().Length == 0)
            {
                Debug.Log("ERROR: no tilemaps");
                return;
            }

            // Determine the maximum bounds of any one tilemap to determine overall map size
            // This assumes that ONE of the tilemaps represents the floor and therefore
            // is the size of the entire map
            foreach (Tilemap tilemap in TilemapGrid.GetComponentsInChildren<Tilemap>())
            {
                // Ignores any deleted tiles from the tilemap, tightening the bounds
                // to the actual tilemaps size
                tilemap.CompressBounds();

                // If the current tilemap's size is larger than any previously found, update mapsize
                if (tilemap.size.x > MapSize.x)
                {
                    MapSize = new Vector2Int(tilemap.size.x, MapSize.y);
                    tilemapOrigin = tilemap.origin;
                }
                if (tilemap.size.y > MapSize.y)
                {
                    MapSize = new Vector2Int(MapSize.x, tilemap.size.y);
                    tilemapOrigin = tilemap.origin;
                }
            }

            // Use the first tilemap as the map size and locations of tiles
            // This assumes that the first tilemap is the floor
            mainTilemap = TilemapGrid.GetComponentsInChildren<Tilemap>()[0];

            // Create the nodes
            pathfindingGrid = new PathfindingGrid();
            pathfindingGrid.Initialize(tilemapOrigin, MapSize);

            // Set all of the unbuildable nodes by iterating through the Tilemaps
            // Ignore tilemap 0 as that is the "floor" tilemap
            for (int t = 1; t < TilemapGrid.GetComponentsInChildren<Tilemap>().Length; ++t)
            {
                Tilemap tilemap = TilemapGrid.GetComponentsInChildren<Tilemap>()[t];

                for (int i = 0; i < MapSize.x; ++i)
                {
                    for (int j = 0; j < MapSize.y; ++j)
                    {
                        Vector3Int position = new Vector3Int(i, j, 0) + tilemapOrigin;

                        TileBase tile = tilemap.GetTile(position);

                        if (tile != null)
                        {
                            pathfindingGrid.SetWalkability(new Vector2Int(i, j), false);
                        }
                    }
                }
            }
        }

    }

    void Awake()
    {
        if (TilemapGrid == null)
        {
            Init();
        }
    }

    /// <summary>
    /// Convert a world position into a zero-based grid position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        return TilemapGrid.GetComponent<GridLayout>().WorldToCell(worldPosition) - tilemapOrigin;
    }

    public Vector3 GridPositiontoWorld(Vector3Int gridPosition)
    {
        return TilemapGrid.GetComponent<GridLayout>().CellToWorld(gridPosition + tilemapOrigin);
    }

    /// <summary>
    /// Find a path between the start and end positions in world space
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        List<Vector3> pathPositions = new List<Vector3>();

        if (TilemapGrid == null)
            return pathPositions;

        List<PathfindingGrid.Node> path = pathfindingGrid.FindPath(WorldToGridPosition(start), WorldToGridPosition(end));

        foreach (PathfindingGrid.Node n in path)
        {
            pathPositions.Add(GridPositiontoWorld(new Vector3Int(n.position.x, n.position.y, 0))
                              + TilemapGrid.GetComponent<GridLayout>().cellSize * 0.5f);
        }
        return pathPositions;
    }

    /// <summary>
    /// Sets how to draw the tile gizmos
    /// </summary>
    public void OnDrawGizmos()
    {
        if (TilemapGrid)
        {
            if (pathfindingGrid == null)
            {
                return;
            }

            foreach (PathfindingGrid.Node n in pathfindingGrid.grid)
            {
                Gizmos.color = (n.walkable) ? new Color(1, 1, 1, .5f) : new Color(1, 0, 0, .5f);

                Gizmos.DrawCube(GridPositiontoWorld(new Vector3Int(n.position.x, n.position.y, 0))
                                + TilemapGrid.GetComponent<GridLayout>().cellSize * 0.5f, TilemapGrid.GetComponent<Grid>().cellSize);

                Gizmos.color = Color.white;
                // Draw the edges as lines
                if (n.walkable)
                {
                    foreach (PathfindingGrid.Node neighbor in n.Neighbors.Where(neighbor => neighbor.walkable))
                    {
                        Gizmos.DrawLine(GridPositiontoWorld(new Vector3Int(n.position.x, n.position.y, 0))
                                        + TilemapGrid.GetComponent<GridLayout>().cellSize * 0.5f,
                                        GridPositiontoWorld(new Vector3Int(neighbor.position.x, neighbor.position.y, 0))
                                        + TilemapGrid.GetComponent<GridLayout>().cellSize * 0.5f);
                    }
                }
            }
        }

    }

    public void reInit()
    {
        // Initialize the mapsize to zero
        MapSize = Vector2Int.zero;
        tilemapOrigin = Vector3Int.zero;

        TilemapGrid = GameObject.Find("Grid");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        entrances = GameObject.FindGameObjectsWithTag("Entrance");
        foreach (GameObject wall in walls)
        {
            wall.transform.SetParent(TilemapGrid.transform);
        }
        foreach (GameObject entrance in entrances)
        {
            entrance.transform.SetParent(TilemapGrid.transform);
        }

        // If there are no tilemaps to process, produce an error
        if (TilemapGrid.GetComponentsInChildren<Tilemap>().Length == 0)
        {
            Debug.Log("ERROR: no tilemaps");
            return;
        }

        // Determine the maximum bounds of any one tilemap to determine overall map size
        // This assumes that ONE of the tilemaps represents the floor and therefore
        // is the size of the entire map
        foreach (Tilemap tilemap in TilemapGrid.GetComponentsInChildren<Tilemap>())
        {
            // Ignores any deleted tiles from the tilemap, tightening the bounds
            // to the actual tilemaps size
            tilemap.CompressBounds();

            // If the current tilemap's size is larger than any previously found, update mapsize
            if (tilemap.size.x > MapSize.x)
            {
                MapSize = new Vector2Int(tilemap.size.x, MapSize.y);
                tilemapOrigin = tilemap.origin;
            }
            if (tilemap.size.y > MapSize.y)
            {
                MapSize = new Vector2Int(MapSize.x, tilemap.size.y);
                tilemapOrigin = tilemap.origin;
            }
        }

        // Use the first tilemap as the map size and locations of tiles
        // This assumes that the first tilemap is the floor
        mainTilemap = TilemapGrid.GetComponentsInChildren<Tilemap>()[0];

        // Create the nodes
        pathfindingGrid = new PathfindingGrid();
        pathfindingGrid.Initialize(tilemapOrigin, MapSize);

        // Set all of the unbuildable nodes by iterating through the Tilemaps
        // Ignore tilemap 0 as that is the "floor" tilemap
        for (int t = 1; t < TilemapGrid.GetComponentsInChildren<Tilemap>().Length; ++t)
        {
            Tilemap tilemap = TilemapGrid.GetComponentsInChildren<Tilemap>()[t];

            for (int i = 0; i < MapSize.x; ++i)
            {
                for (int j = 0; j < MapSize.y; ++j)
                {
                    Vector3Int position = new Vector3Int(i, j, 0) + tilemapOrigin;

                    TileBase tile = tilemap.GetTile(position);

                    if (tile != null)
                    {
                        pathfindingGrid.SetWalkability(new Vector2Int(i, j), false);
                    }
                }
            }
        }
    }
}

