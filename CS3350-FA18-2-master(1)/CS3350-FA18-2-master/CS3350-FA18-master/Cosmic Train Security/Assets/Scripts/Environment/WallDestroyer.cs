using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallDestroyer : MonoBehaviour {
    #region Fields
    GameObject grid; // this is the grid in which all tiles are placed
    Tilemap map; // holds locations of each tile
    GridLayout gridLayout;

    [SerializeField]
    GameObject vacuum;//the game object that suckes the player out 

    [SerializeField]
    Tile wallTile;

    [SerializeField]
    Tile destroyedTileLeft;

    [SerializeField]
    Tile destroyedTileRight;

    [SerializeField]
    Tile cornerTile;

    [SerializeField]
    Tile intersectionTile;

    [SerializeField]
    Tile blastProofWallHoriz;

    [SerializeField]
    Tile blastProofWallVert;

    [SerializeField]
    Tile blastProofWallVertFlipped;

    [SerializeField]
    Tile blastProofWallHorizFlipped;

    [SerializeField]
    Tile interiorWallTile;

    [SerializeField]
    Tile interiorWallTileLeftFlipped;

    [SerializeField]
    Tile interiorWallTileRightFlipped;

    //this script holds a timer so that it knows when to repair the wall 
    Timer timer;
    bool timerIsRunning = false;

    // how long area effector is active
    float reinforceTime = 5f;

    TileBase upAndRight; // we need to remember which tiles were damaged
    TileBase downAndLeft; // we need to remember which tiles were damaged

    // We'll use this boolean to determine if a hole currently exists in the wall. We only want one holes to only be able to spawn one at a time.
    bool wallIsDamaged = false;

    float xSize; // the horizontal number of cells in the grid
    float ySize; // the vertical number of cells in the grid

    float leftEdgeofShip = -8f;
    float rightEdgeofShip = 57f;
    float topEdgeofShip = 18f;
    float bottomEdgeofShip = -21f;

    Vector3Int[] neighborTiles; // will hold the tiles on either side of the damaged tile

    Vector3Int cellPosition; // the position of the center tile to be damaged

    // This tells us the orientation of the last destroyed wall
    WallOrientationEnum orientation = WallOrientationEnum.NOTHING;

    #endregion
    #region Methods
    // Use this for initialization
    void Start () {

        // reference to grid
        grid = GameObject.FindGameObjectWithTag("Grid");

        // get the tilemap from grid
        map = grid.GetComponentsInChildren<Tilemap>()[1];

        // get the gridlayout so that we can locate the cell position within the grid
        gridLayout = GetComponentInParent<GridLayout>();

        // Add RemoveTile as a delegated listener for the wall destruction event
        EventManager.AddWallDestructionListeners(RemoveTile);

        // Add timer for blastproof wall instantiation
        timer = gameObject.AddComponent<Timer>();

        // get the horizontal size
        xSize = map.size.x;

        // get the vertical size
        ySize = map.size.y;

    }

    /// <summary>
    /// This method removes the tile given World Space coordinate - ADDING STUFF
    /// </summary>
    /// <param name="pos"></param>
    void RemoveTile(Vector3 pos)
    {
        if (!wallIsDamaged)
        {
            wallIsDamaged = true; // a wall is now damage, don't allow any more holes for now

            
            // convert to grid space
            cellPosition = gridLayout.WorldToCell(pos);


            // Get the positions of neighboring tiles
            neighborTiles = SetNeighborTiles(cellPosition);

            // make copies of neighboring tiles, we're gonna need them again!
            upAndRight = map.GetTile(neighborTiles[0]);
            downAndLeft = map.GetTile(neighborTiles[1]);

            // If the damage is happening at a corner, let's reposition the damage ;-)   Reposition down or left
            TileBase tempTile = map.GetTile(neighborTiles[0]);
            if (tempTile == cornerTile || tempTile == intersectionTile)
            {
                if (neighborTiles[0] == cellPosition + Vector3Int.up)
                {
                    cellPosition += Vector3Int.down;

                }
                else
                {
                    cellPosition += Vector3Int.left;

                }

                neighborTiles = SetNeighborTiles(cellPosition);
            }
            // If the damage is happening at a corner, let's reposition the damage ;-)   Reposition up or right
            else if (map.GetTile(neighborTiles[1]) == cornerTile || map.GetTile(neighborTiles[1]) == intersectionTile)
            {
                if (neighborTiles[1] == cellPosition + Vector3Int.down)
                {
                    cellPosition += Vector3Int.up;

                }
                else
                {
                    cellPosition += Vector3Int.right;

                }

                neighborTiles = SetNeighborTiles(cellPosition);
            }

            //Check if tiles are vertical tiles along the right edge of the ship, if so then we need to flip
            //the orientation of the tiles so that they match the existing wall
            if ((cellPosition.x >= rightEdgeofShip && cellPosition.y != neighborTiles[0].y)) // Check to see if along the right side of the ship, and make sure the neighboring tiles are vertical
            {
                // Set the Up or Right tile
                map.SetTile(neighborTiles[0], destroyedTileLeft);

                // Set the Down or Left tile
                map.SetTile(neighborTiles[1], destroyedTileRight);

                orientation = WallOrientationEnum.VERTICAL_FLIPPED;
            }
            else if (cellPosition.y <= bottomEdgeofShip && cellPosition.x != neighborTiles[1].x) // check to see if along the bottom edge of the ship, and make sure the neighboring tiles are horizontal
            {
                // Set the Up or Right tile
                map.SetTile(neighborTiles[0], destroyedTileLeft);

                // Set the Down or Left tile
                map.SetTile(neighborTiles[1], destroyedTileRight);

                orientation = WallOrientationEnum.HORIZONTAL_FLIPPED;
            }
            else // All other cases
            {
                if (neighborTiles[0].y > cellPosition.y && neighborTiles[1].y < cellPosition.y)
                {
                    orientation = WallOrientationEnum.VERTICAL;
                }

                else if (neighborTiles[0].x > cellPosition.x && neighborTiles[1].x < cellPosition.x)
                {
                    orientation = WallOrientationEnum.HORIZONTAL;
                }
                // Set the Up or Right tile
                map.SetTile(neighborTiles[0], destroyedTileRight);

                // Set the Down or Left tile
                map.SetTile(neighborTiles[1], destroyedTileLeft);

                // This handles interior wall destruction
                if (tempTile == interiorWallTile)
                {
                    // Get the player's position, we always want damaged metal to bend away from the player
                    Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position; 

                    if (orientation == WallOrientationEnum.HORIZONTAL)
                    {
                        if (playerPosition.y > pos.y)
                        {
                            // Set the Up or Right tile
                            map.SetTile(neighborTiles[0], destroyedTileLeft);

                            // Set the Down or Left tile
                            map.SetTile(neighborTiles[1], destroyedTileRight);

                            orientation = WallOrientationEnum.INTERIOR_HORIZONTAL_FLIPPED;

                        }
                        else
                        {
                            // Set the Up or Right tile
                            map.SetTile(neighborTiles[0], interiorWallTileRightFlipped);

                            // Set the Down or Left tile
                            map.SetTile(neighborTiles[1], interiorWallTileLeftFlipped);
                           
                            orientation = WallOrientationEnum.INTERIOR_HORIZONTAL;
                        }

                    }

                    else if (orientation == WallOrientationEnum.VERTICAL)
                    {
                        if (playerPosition.x > pos.x)
                        {
                            // Set the Up or Right tile
                            map.SetTile(neighborTiles[1], interiorWallTileLeftFlipped);

                            // Set the Down or Left tile
                            map.SetTile(neighborTiles[0], interiorWallTileRightFlipped);

                            orientation = WallOrientationEnum.INTERIOR_VERTICAL;
                        }
                        else
                        {
                            // Set the Up or Right tile
                            map.SetTile(neighborTiles[0], destroyedTileLeft);

                            // Set the Down or Left tile
                            map.SetTile(neighborTiles[1], destroyedTileRight);
                            
                            orientation = WallOrientationEnum.INTERIOR_VERTICAL_FLIPPED;
                        }

                    }
                }
            }

            // remove the middle tile
            map.SetTile(cellPosition, null);

            // Check if exterior wall, if so then suck into space
            if (orientation != WallOrientationEnum.INTERIOR_HORIZONTAL_FLIPPED && orientation != WallOrientationEnum.INTERIOR_VERTICAL_FLIPPED
                && orientation != WallOrientationEnum.INTERIOR_HORIZONTAL && orientation != WallOrientationEnum.INTERIOR_VERTICAL)
            {
                GameObject v = Instantiate<GameObject>(vacuum);
                v.transform.position = pos;
            }
           
            timer.Duration = reinforceTime;
            timer.Run();
            timerIsRunning = true;
        }
        

    }

    /// <summary>
    /// This is a helper method that will determine if the provided location is along the exterior wall of the ship
    /// </summary>
    public ExteriorEdgeEnum ExteriorWall(Vector3 pos)
    {
        Vector3Int gridPos = gridLayout.WorldToCell(pos); // convert to world space position to grid space position

        // along top edge?
        if (gridPos.y >= topEdgeofShip)
        {
            return ExteriorEdgeEnum.TOPEDGE;
        }

        // along left edge?
        else if (gridPos.x <= leftEdgeofShip)
        {
            return ExteriorEdgeEnum.LEFTEDGE;
        }

        // along right edge?
        else if (gridPos.x >= rightEdgeofShip)
        {
            return ExteriorEdgeEnum.RIGHTEDGE;
        }

        // along bottom edge?
        else if (gridPos.y <= bottomEdgeofShip)
        {
            return ExteriorEdgeEnum.BOTTOMEDGE;
        }

        // not along an exterior wall
        else
        {
            return ExteriorEdgeEnum.NOEDGE;
        }
    }


    /// <summary>
    /// As of now, this only exists for the repair over time function
    /// </summary>
    private void Update()
    {
        if (timer.Finished && timerIsRunning)
        {
            timerIsRunning = false;
            switch (orientation)
            {
                case WallOrientationEnum.HORIZONTAL:
                    map.SetTile(cellPosition, blastProofWallHoriz);
                    break;

                case WallOrientationEnum.VERTICAL:
                    map.SetTile(cellPosition, blastProofWallVert);
                    break;

                case WallOrientationEnum.VERTICAL_FLIPPED:
                    map.SetTile(cellPosition, blastProofWallVertFlipped);
                    break;

                case WallOrientationEnum.HORIZONTAL_FLIPPED:
                    map.SetTile(cellPosition, blastProofWallHorizFlipped);
                    break;

                case WallOrientationEnum.INTERIOR_VERTICAL_FLIPPED:
                    map.SetTile(cellPosition, blastProofWallVertFlipped);
                    break;

                case WallOrientationEnum.INTERIOR_HORIZONTAL_FLIPPED:
                    map.SetTile(cellPosition, blastProofWallHorizFlipped);
                    break;

                case WallOrientationEnum.INTERIOR_HORIZONTAL:
                    map.SetTile(cellPosition, blastProofWallHoriz);
                    break;

                case WallOrientationEnum.INTERIOR_VERTICAL:
                    map.SetTile(cellPosition, blastProofWallVert);
                    break;


            }

            wallIsDamaged = false; // wall is no longer damaged

            // replace the original tiles on either side of the damaged center tile
            map.SetTile(neighborTiles[0], upAndRight);
            map.SetTile(neighborTiles[1], downAndLeft);

        }
    }

    /// <summary>
    /// This method takes in a position and returns an arTileray of it's neighbors' positions (in grid space)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    Vector3Int[] SetNeighborTiles(Vector3Int pos)  // pos is the tile which will be checked for neighbors
    {
        Vector3Int[] neighbors = new Vector3Int[2]; // this array will hold the two neighbor positions in grid space

        Vector3Int tempPos = pos + Vector3Int.right;
        TileBase tempTile = map.GetTile(tempPos);

        if (tempTile == wallTile || tempTile == cornerTile || tempTile == intersectionTile || tempTile == interiorWallTile) 
        {
            neighbors[0] = tempPos; // neighbor is to the right, set the first element in the array
        }
        else
        {
            // There's no neighbor to the right, let's check above
            tempPos = pos + Vector3Int.up;
            tempTile = map.GetTile(tempPos);
            if (tempTile == wallTile || tempTile == cornerTile || tempTile == intersectionTile || tempTile == interiorWallTile)
            {
                neighbors[0] = tempPos;

            }
        }

        // Now, let's test the neighbor below
        tempPos = pos + Vector3Int.down;
        tempTile = map.GetTile(tempPos);
        if (tempTile == wallTile || tempTile == cornerTile || tempTile == intersectionTile || tempTile == interiorWallTile)
        {
            neighbors[1] = tempPos; // neighbor is to the right, set the first element in the array
            //Debug.Log("Below");
        }
        else
        {
            // There's no neighbor to the right, let's check above
            tempPos = pos + Vector3Int.left;
            tempTile = map.GetTile(tempPos);
            if (tempTile == wallTile || tempTile == cornerTile || tempTile == intersectionTile || tempTile == interiorWallTile)
            {
                neighbors[1] = tempPos;
                //Debug.Log("Left");
            }
        }

        return neighbors;
    }
}
#endregion
