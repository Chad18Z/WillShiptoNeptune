using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PathfindingGrid
{
    public class Node : IComparable
    {
        #region fields

        //public Vector2 gridPosition;   // World position of the node
        public bool walkable;           // Boolean to check if an object is walkable
        public Vector2Int position;
        public List<Node> Neighbors;

        // Pathfinding specific variables, reset for each pathfinding request
        public float gCost;             // Current node distance from source node
        public float hCost;             // Current node distance to target node
        public Node parent;             // Parent node of current node

        #endregion

        #region properties

        public float fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        #endregion

        #region methods

        public Node()
        {
            Neighbors = new List<Node>();
            walkable = true;
        }

        public int CompareTo(object other)
        {
            return fCost.CompareTo(((Node)other).fCost);
        }
        #endregion

    }

    #region fields

    public Vector3Int gridOriginPosition;
    public Vector2Int gridWorldSize;    // World size of the grid
    public Vector2Int gridSize;         // Size of the grid in number of nodes
    public Node[,] grid;                // 2D array of nodes for the grid
    float[,,,] Distances;               // Computed "euclidean" distances between nodes
                                        // usage: Distances[node1_x, node1_y, node2_x, node2_y]

    #endregion

    #region methods

    public float GetDistance(Node a, Node b)
    {
        return Distances[a.position.x, a.position.y, b.position.x, b.position.y];
    }

    /// <summary>
    /// Method for initialization
    /// </summary>
    public void Initialize(Vector3Int gridOriginPosition, Vector2Int gridWorldSize)
    {
        // Set the initial size of the grid
        this.gridWorldSize = gridWorldSize;
        this.gridOriginPosition = gridOriginPosition;

        // Initialize variables and create grid
        gridSize = new Vector2Int(gridWorldSize.x, gridWorldSize.y);
        grid = new Node[gridSize.x, gridSize.y];

        CreateGrid();
        ComputeDistances();
        ConnectNeighbors();
    }

    /// <summary>
    /// Change the walkability of all the nodes under a tile
    /// </summary>
    /// <param name="gridTileIndex"></param>
    /// <param name="walkable"></param>
    public void SetWalkability(Vector2Int gridTileIndex, bool walkable)
    {
        // Get the first node associated with the gridTileIndex
        Vector2Int firstNode = gridTileIndex;

        grid[firstNode.x, firstNode.y].walkable = false;
    }
    
    /// <summary>
    /// Method which creates a grid, filling the 2D array of nodes
    /// </summary>
    private void CreateGrid()
    {
        grid = new Node[gridSize.x, gridSize.y];

        // Compute the position relative to the grid's world position
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                grid[i, j] = new Node();
                grid[i, j].position.x = i;
                grid[i, j].position.y = j;
                //grid[i, j].gridPosition = gridOriginPosition + new Vector3(i, j, 0);
            }
        }
    }

     /// <summary>
    /// Compute the euclidean distance between each pair of nodes
    /// </summary>
    private void ComputeDistances()
    {
        Distances = new float[gridSize.x, gridSize.y, gridSize.x, gridSize.y];
        
        // for each pair of nodes, store their actual distance
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    for (int y = 0; y < gridSize.y; y++)
                    {
                        Distances[i, j, x, y] = Vector2Int.Distance(grid[i, j].position, grid[x, y].position);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Connect each node to its neighbors
    /// </summary>
    private void ConnectNeighbors()
    {
        // Find all Neighbors of each node
        for (int a = 0; a < gridSize.x; a++)
        {
            for (int b = 0; b < gridSize.y; b++)
            {
                int checkX;     // int to check the x grid location
                int checkY;     // int to check the y grid location

                // Iterate over each neighboring node
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        // Skip case of "self"
                        if (x == 0 && y == 0)
                        {
                            continue;
                        }

                        // Make sure the neighbor exists and then add it
                        checkX = a + x;
                        checkY = b + y;
                        if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                        {
                            // Add all neighbour nodes to the list of neighbours
                            grid[a, b].Neighbors.Add(grid[checkX, checkY]);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Reset all of the parameters for pathfinding
    /// </summary>
    public void ResetPathfinding()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                // Reset the parent and costs associated with this node
                grid[i, j].parent = null;
                grid[i, j].gCost = 0;
                grid[i, j].hCost = Int32.MaxValue;
            }
        }
    }

    /// <summary>
    /// Finds the shortest path between two positions
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns>list of nodes in path or empty list if path not found</returns>
    public List<Node> FindPath(Vector3Int startPos, Vector3Int endPos)
    {
        ResetPathfinding();

        Node startNode = grid[startPos.x, startPos.y];            // Location of starting node
        Node endNode = grid[endPos.x, endPos.y];                // Location of ending node

        List<Node> finalList = new List<Node>();                    // the final path the enemy should take
        PriorityQueue<Node> openSet = new PriorityQueue<Node>();    // List of open nodes
        HashSet<Node> closedSet = new HashSet<Node>();              // Hash set of closed nodes

        // Initialize startNode's fCost
        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, endNode);
        openSet.Enqueue(new PriorityNode<Node>(startNode, startNode.fCost));

        // Whill the open set contains nodes
        while (openSet.Count > 0)
        {
            Node node = openSet.Dequeue().item;     // set stored node to the first in the list

            // Remove node from open set so no duplicates are calculated
            closedSet.Add(node);

            // If end node is reached...
            if (node == endNode)
            {
                // Retrace path to starting node
                finalList = RetracePath(startNode, endNode);
                return finalList;
            }

            // Iterate over each neighbour node
            foreach (Node neighbour in node.Neighbors)
            {
                // If the neighbour is not walkable or closed...
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    // skip
                    continue;
                }

                // New cost to neighbour is gCost of current node + distance to neighbour
                float newCostToNeighbour = node.gCost + GetDistance(node, neighbour);

                // If new cost to neighbour is less the the gCost of the current neighbour, or the neighbour is not open...
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    // Current gCost = new gCost, hCost is distance from neighbour to end node, and parent of neighbour = current node
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = node;

                    // If neighbour is not open...
                    if (!openSet.Contains(neighbour))
                    {
                        // Add the neighbour to the open set
                        openSet.Enqueue(new PriorityNode<Node>(neighbour, neighbour.fCost));
                    }
                    else
                    {
                        openSet.ChangePriority(openSet.Find(neighbour), neighbour.fCost);
                    }
                }
            }
        }
        return finalList;
    }

    /// <summary>
    /// Sets the grid path to a reversed one
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="endNode"></param>
    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();     // List of nodes for the path
        Node currentNode = endNode;             // Node to start at

        // While the current node isn't the start node
        while (currentNode != startNode)
        {
            // Add node to the path
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    #endregion
}
