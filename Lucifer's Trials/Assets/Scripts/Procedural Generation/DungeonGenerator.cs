using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DungeonGenerator : IRoomGenerator
{

    // Columns are the rectangles that generate in the room
    private int _maxColumns;
    private int _maxColumnLength;
    private Vector2Int _size;

    // The number of exits in the room. The entrance counts as an exit.
    private int _numExits;

    private const int MIN_NUM_COLUMNS = 1;
    private const int MIN_COLUMN_HEIGHT = 3;
    private const int MAX_COLUMN_GENERATION_TRIES = 10;
    private const float ROOM_SCALE = 1f;
    private const float DECORATION_RATE = 0.2f;

    // _tileMaps maps a descriptive name to a tilemap
    private Dictionary<string, Tilemap> _tileMaps;

    // The tileset that this generator uses is the DungeonTileset
    private ITileset _tileset = DungeonTileset.Instance;

    private enum WallDecorationType
    {

        RectangleVent,
        CircleVent,
        Banner,
        SingleTorch,
        DoubleTorch,

    }

    public DungeonGenerator(int maxColumns, int maxColumnLength, Vector2Int size)
    {

        _maxColumns = maxColumns;
        _maxColumnLength = maxColumnLength;
        _size = size;
        
    }

    public Room Generate(int numExits)
    {

        // Reset tilemaps and number of exits because they a unique per-room
        _tileMaps = new Dictionary<string, Tilemap>();
        _numExits = numExits;

        // Create the gameobject for the room
        GameObject room = new GameObject("Room");
        
        // Add a grid component
        Grid grid = room.AddComponent<Grid>();
        grid.cellSize = new Vector3(1, 1, 0);

        // Create "Ground" layer
        GameObject groundLayer = new GameObject("Ground");
        groundLayer.transform.SetParent(room.transform);
        TilemapRenderer groundRenderer = groundLayer.AddComponent<TilemapRenderer>();
        groundRenderer.sortingOrder = 0;
        _tileMaps.Add("Ground", groundLayer.GetComponent<Tilemap>());

        // Create "Collision" layer
        GameObject collisionLayer = new GameObject("Collision");
        collisionLayer.tag = "Wall";
        collisionLayer.transform.SetParent(room.transform);
        TilemapRenderer collisionRenderer = collisionLayer.AddComponent<TilemapRenderer>();
        collisionRenderer.sortingOrder = 1;
        _tileMaps.Add("Collision", collisionLayer.GetComponent<Tilemap>());

        // Create "Borders" Layer
        GameObject bordersLayer = new GameObject("Borders");
        bordersLayer.transform.SetParent(room.transform);
        TilemapRenderer bordersRenderer = bordersLayer.AddComponent<TilemapRenderer>();
        bordersRenderer.sortingOrder = 1;
        _tileMaps.Add("Borders", bordersLayer.GetComponent<Tilemap>());

        // Create "Exits" Layer
        GameObject exitsLayer = new GameObject("Exits");
        exitsLayer.transform.SetParent(room.transform);
        TilemapRenderer exitsRenderer = exitsLayer.AddComponent<TilemapRenderer>();
        exitsRenderer.enabled = false;
        exitsRenderer.sortingOrder = 4;
        _tileMaps.Add("Exits", exitsLayer.GetComponent<Tilemap>());

        // Create "Decorations" Layer
        GameObject decorationsLayer = new GameObject("Decorations");
        decorationsLayer.transform.SetParent(room.transform);
        TilemapRenderer decorationsRenderer = decorationsLayer.AddComponent<TilemapRenderer>();
        decorationsRenderer.sortingOrder = 3;
        _tileMaps.Add("Decorations", decorationsLayer.GetComponent<Tilemap>());

        // Place Ground tiles
        PlaceRectangleFilled("Ground", "Ground", _size.x + 2, _size.y + 4, new Vector2Int(0, 0));
        
        // Get position and size of ground area
        Vector3 groundPos = _tileMaps["Ground"].transform.position;
        Vector3Int groundSize = _tileMaps["Ground"].size;

        // Place the black border around the room
        PlaceRectangleHollow("Collision", "Black", _size.x + 2, _size.y + 4, new Vector2Int(0, 0));
        PlaceRectangleFilled("Collision", "Black", _size.x, 2, new Vector2Int(1, _size.y + 1));

        // Generate a random number of columns
        int numColumns = Random.Range(MIN_NUM_COLUMNS, _maxColumns + 1);
        GenerateColumns(numColumns, room);

        // Place the wall tiles onto the Collision tilemap
        PlaceWalls(_tileMaps["Collision"]);

        /*  
            Place a black border around the edges of the Borders layer so that it is the same size as the Collision layer.
            This is so that we don't need to translate coordinates between the two.
        */
        PlaceRectangleHollow("Borders", "Black", _size.x + 2, _size.y + 4, new Vector2Int(0, 0));

        // Place the border tiles onto the Borders tilemap
        PlaceBorders(_tileMaps["Borders"], _tileMaps["Collision"]);

        //Line up Exits tilemap with the other two, and make it 50% transparent for debugging purposes.
        PlaceRectangleHollow("Exits", "Black", _size.x + 2, _size.y + 4, new Vector2Int(0, 0));

        //Line up Decorations tilemap with the others
        PlaceRectangleHollow("Decorations", "Black", _size.x + 2, _size.y + 4, new Vector2Int(0, 0));
        // Get all possible exit paths in this room
        List<ExitPathRectangle> possibleExitRects = FindPossibleExitPaths(_tileMaps["Collision"]);
        List<ExitPathRectangle> exitRects = new List<ExitPathRectangle>();
        // Grab _numExits number of them at random
        for (int i = 0; i < _numExits; i++)
        {

            ExitPathRectangle randomExit = possibleExitRects[Random.Range(0, possibleExitRects.Count)];
            randomExit.SetID(i);
            exitRects.Add(randomExit);
            possibleExitRects.Remove(randomExit);

            // And draw the exitPath
            DrawExit(randomExit);

        }

        PlaceWallDecorations();

        // Scale room
        room.transform.localScale = new Vector3(ROOM_SCALE, ROOM_SCALE, 0);

        // Add collider to Collision layer
        TilemapCollider2D collisionTilemapCollider = collisionLayer.AddComponent<TilemapCollider2D>();
        collisionLayer.AddComponent<CompositeCollider2D>();
        Rigidbody2D colliderRigidBody = collisionLayer.GetComponent<Rigidbody2D>();
        colliderRigidBody.bodyType = RigidbodyType2D.Static;
        collisionTilemapCollider.usedByComposite = true;

        return new Room(room, exitRects);

    }

    private void PlaceWallDecorations()
    {

        // Place the decorations
        for (int x = 0; x < _tileMaps["Decorations"].size.x; x++)
        {

            for (int y = _tileMaps["Decorations"].size.y - 1; y >= 0; y--)
            {

                Tile currentCollisionTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;
                Tile currentDecoTile = _tileMaps["Decorations"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                // If this is a "Wall_Mid" tile
                if ((currentCollisionTile != null) && (_tileset.GetNameOfTile(currentCollisionTile).Contains("Wall_Mid")))
                {

                    // Don't overwrite if there is already someting on this decoration tile
                    if ((Random.Range(0f, 1f) < DECORATION_RATE) && (currentDecoTile == null))
                    {

                        // Make sure only generate on walls that are not edges
                        if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Bottom"))
                        {

                            Tile collisionTileAbove = _tileMaps["Collision"].GetTile(new Vector3Int(x, y + 1, 0)) as Tile;

                            if (!(_tileset.GetNameOfTile(collisionTileAbove).Contains("Mid")))
                            {
                                continue;
                            }

                        }
                        else if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Top"))
                        {

                            Tile collisionTileBelow = _tileMaps["Collision"].GetTile(new Vector3Int(x, y - 1, 0)) as Tile;

                            if (!(_tileset.GetNameOfTile(collisionTileBelow).Contains("Mid")))
                            {
                                continue;
                            }

                        }

                        // Generate a random decoration type
                        int numDifferentDecorations = System.Enum.GetNames(typeof(WallDecorationType)).Length;
                        WallDecorationType decoType = (WallDecorationType)Random.Range(0, numDifferentDecorations + 1);

                        // Place the decoration, or the first half of it if it is a two-tile decoration
                        if (decoType == WallDecorationType.RectangleVent)
                        {
                            _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("RectangleVent"));
                        }
                        else if (decoType == WallDecorationType.CircleVent)
                        {

                            if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Top"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("CircleVent_Top"));
                            }
                            else if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Bottom"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("CircleVent_Bottom"));
                            }

                        }
                        else if (decoType == WallDecorationType.Banner)
                        {

                            if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Top"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("WallBanner_Top"));
                            }
                            else if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Bottom"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("WallBanner_Bottom"));
                            }

                        }
                        else if (decoType == WallDecorationType.SingleTorch)
                        {

                            if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Top"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("SingleWallTorch_Top"));
                            }
                            else if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Bottom"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("SingleWallTorch_Bottom"));
                            }

                        }
                        else if (decoType == WallDecorationType.DoubleTorch)
                        {

                            if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Top"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("DoubleWallTorch_Top"));
                            }
                            else if (_tileset.GetNameOfTile(currentCollisionTile).Contains("Bottom"))
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("DoubleWallTorch_Bottom"));
                            }

                        }

                    }

                }

            }

        }

        // Place the other half of the decorations if they take up two tiles
        for (int x = 0; x < _tileMaps["Decorations"].size.x; x++)
        {

            for (int y = _tileMaps["Decorations"].size.y - 1; y >= 0; y--)
            {

                Tile currentDecoTile = _tileMaps["Decorations"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentDecoTile != null)
                {

                    if (_tileset.GetNameOfTile(currentDecoTile) == "CircleVent_Top")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y - 1, 0), _tileset.GetTileByName("CircleVent_Bottom"));
                    }
                    else if (_tileset.GetNameOfTile(currentDecoTile) == "CircleVent_Bottom")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y + 1, 0), _tileset.GetTileByName("CircleVent_Top"));
                    }
                    else if (_tileset.GetNameOfTile(currentDecoTile) == "WallBanner_Top")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y - 1, 0), _tileset.GetTileByName("WallBanner_Bottom"));
                    }
                    else if (_tileset.GetNameOfTile(currentDecoTile) == "WallBanner_Bottom")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y + 1, 0), _tileset.GetTileByName("WallBanner_Top"));
                    }
                    else if (_tileset.GetNameOfTile(currentDecoTile) == "SingleWallTorch_Top")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y - 1, 0), _tileset.GetTileByName("SingleWallTorch_Bottom"));
                    }
                    else if (_tileset.GetNameOfTile(currentDecoTile) == "SingleWallTorch_Bottom")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y + 1, 0), _tileset.GetTileByName("SingleWallTorch_Top"));
                    }
                    else if (_tileset.GetNameOfTile(currentDecoTile) == "DoubleWallTorch_Top")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y - 1, 0), _tileset.GetTileByName("DoubleWallTorch_Bottom"));
                    }
                    else if (_tileset.GetNameOfTile(currentDecoTile) == "DoubleWallTorch_Bottom")
                    {
                        _tileMaps["Decorations"].SetTile(new Vector3Int(x, y + 1, 0), _tileset.GetTileByName("DoubleWallTorch_Top"));
                    }

                }  

            }

        }

    }

    private void DrawExit(ExitPathRectangle rect)
    {

        int pathWidth = rect.topRight.x - rect.bottomLeft.x + 1;
        int pathHeight = rect.topRight.y - rect.bottomLeft.y + 1;

        for (int x = rect.bottomLeft.x; x <= rect.topRight.x; x++)
        {

            for (int y = rect.topRight.y; y >= rect.bottomLeft.y; y--)
            {

                // Logic for drawing the doors to the back wall if the exit leads up
                if (rect.direction == ExitDirection.UP)
                {

                    Tile currentTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                    if (currentTile != null)
                    {

                        if (_tileset.GetNameOfTile(currentTile) != "Black")
                        {

                            Tile collisionTileAbove = _tileMaps["Collision"].GetTile(new Vector3Int(x, y + 1, 0)) as Tile;
                            Tile decorationsTileAbove = _tileMaps["Decorations"].GetTile(new Vector3Int(x, y + 1, 0)) as Tile;

                            if (x == rect.bottomLeft.x && _tileset.GetNameOfTile(collisionTileAbove) == "Black")
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("BigDoor_TopLeft"));
                            }
                            else if (x == rect.topRight.x && _tileset.GetNameOfTile(collisionTileAbove) == "Black")
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("BigDoor_TopRight"));
                            }
                            else if (_tileset.GetNameOfTile(decorationsTileAbove) == "BigDoor_TopLeft")
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("BigDoor_MiddleLeft"));
                            }
                            else if (_tileset.GetNameOfTile(decorationsTileAbove) == "BigDoor_TopRight")
                            {
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("BigDoor_MiddleRight"));
                            }

                        }

                    }
                    else if (currentTile == null)
                    {

                        Tile decorationsTileAbove = _tileMaps["Decorations"].GetTile(new Vector3Int(x, y + 1, 0)) as Tile;

                        if (decorationsTileAbove != null)
                        {

                            if (_tileset.GetNameOfTile(decorationsTileAbove) == "BigDoor_MiddleLeft")
                            {

                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("BigDoor_BottomLeft"));
                                _tileMaps["Exits"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("Exit_Up"));

                            }
                            else if (_tileset.GetNameOfTile(decorationsTileAbove) == "BigDoor_MiddleRight")
                            {
                                
                                _tileMaps["Decorations"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("BigDoor_BottomRight"));
                                _tileMaps["Exits"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("Exit_Up"));

                            }

                        }

                    }

                }
                else if ((rect.direction == ExitDirection.LEFT) && (x == rect.bottomLeft.x))
                {
                    _tileMaps["Exits"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("Exit_Left"));
                }
                else if ((rect.direction == ExitDirection.RIGHT) && (x == rect.topRight.x))
                {
                    _tileMaps["Exits"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("Exit_Right"));
                }
                else if ((rect.direction == ExitDirection.DOWN) && (y == rect.bottomLeft.y))
                {
                    _tileMaps["Exits"].SetTile(new Vector3Int(x, y, 0), _tileset.GetTileByName("Exit_Down"));
                }

            }

        }      

    }

    private void GenerateColumns(int numColumns, GameObject room)
    {

        GameObject collisionLayer = room.transform.Find("Collision").gameObject;

        // Generate all the columns
        for (int i = 0; i < numColumns; i++)
        {

            int columnWidth;
            int columnHeight;
            int x;
            int y;
            int numTries = 0;

            // Generate a column
            do
            {

                /*
                    When a column is generated, we check to make sure that the placement of it was valid. If it wasn't, we 
                    remove the column and generate a new one. To avoid generation possibly taking too long due to too many
                    failed column placements, we set a maximum of MAX_COLUMN_GENERATION_TRIES for how many times we can attempt
                    to generate the current column. If it fails all its placements, we just skip it.
                */
                numTries++;

                if (numTries <= MAX_COLUMN_GENERATION_TRIES)
                {

                    // Make a copy of the Collision layer
                    Vector3 collisionLayerPosition = collisionLayer.transform.position;
                    GameObject collLayerCopy = Object.Instantiate(collisionLayer, collisionLayerPosition, Quaternion.identity);
                    collLayerCopy.SetActive(false);
                    collLayerCopy.transform.SetParent(room.transform);
                    Tilemap collTileMapCopy = collLayerCopy.GetComponent<Tilemap>();

                    // Generate a random size and location for the column
                    columnWidth = Random.Range(1, _maxColumnLength + 1);
                    /*
                        Column height is always at least MIN_COLUMN_HEIGHT = 3 tiles tall because columns that are shorter than
                        that just don't look quite right
                    */
                    columnHeight = Mathf.Max(Random.Range(1, _maxColumnLength + 1), MIN_COLUMN_HEIGHT);
                    x = Random.Range(1, _size.x + 1);
                    y = Random.Range(1, _size.y + 1);

                    // Truncate columns that are too long and generate off the edges of the room
                    if (x + columnWidth > _tileMaps["Collision"].size.x)
                    {
                        columnWidth -= (x + columnWidth - _tileMaps["Collision"].size.x);
                    }
                    if (y + columnHeight > _tileMaps["Collision"].size.y)
                    {
                        columnHeight -= (y + columnHeight - _tileMaps["Collision"].size.y);
                    }

                    /*
                        Place the column on our copy of the Collision tilemap. This is because it is easier to just only place
                        known-valid columns on our actual Collision tilemap than it is to attempt to remove ones that are
                        invalid.
                    */
                    PlaceRectangleFilled(collTileMapCopy, "Black", columnWidth, columnHeight, new Vector2Int(x, y));

                    // Now check our copy to see if the placement was valid
                    if (IsValidColumn(collTileMapCopy, columnWidth, columnHeight, new Vector2Int(x, y)))
                    {

                        // And place the column on our actual Collision tilemap if it was
                        PlaceRectangleFilled("Collision", "Black", columnWidth, columnHeight, new Vector2Int(x, y));
                        // And destroy our copy
                        Object.Destroy(collLayerCopy);
                        break;

                    }

                    // Also destroy the copy if it wasn't valid
                    Object.Destroy(collLayerCopy);

                }
                else
                {
                    // Break if we reached our limit of column generation tries
                    break;
                }

            } while (true);

        }

    }

    // Get a list of Rectangles that represent the possible exit paths from the given collision tilemap
    private List<ExitPathRectangle> FindPossibleExitPaths(Tilemap tileMap)
    {

        List<ExitPathRectangle> exitRects = new List<ExitPathRectangle>();

        // Make copy of the tilemap
        Vector3 objectPosition = tileMap.gameObject.transform.position;
        GameObject objectCopy = Object.Instantiate(tileMap.gameObject, objectPosition, Quaternion.identity);
        objectCopy.SetActive(false);
        objectCopy.transform.SetParent(tileMap.gameObject.transform.parent);
        Tilemap tileMapCopy = objectCopy.GetComponent<Tilemap>();
        objectCopy.GetComponent<TilemapRenderer>().sortingOrder = 4;

        for (int x = 0; x < tileMapCopy.size.x; x++)
        {

            for (int y = 0; y < tileMapCopy.size.y; y++)
            {

                // If we are at a point on the left or right edges of the room
                if (((x == 0) || (x == tileMapCopy.size.x - 1)) && (y >= 1) && (y < tileMapCopy.size.y - 2))
                {

                    ExitPathRectangle currentRect = new ExitPathRectangle();

                    /*
                        Pick the current point and the point on top of the current point. These two points make up the
                        start of our exit path.
                    */
                    Vector3Int lowerPoint = new Vector3Int(x, y, 0);
                    Vector3Int upperPoint = new Vector3Int(x, y + 1, 0);

                    // currentRect will store the Rectangle that we will actually return representing this path
                    currentRect.SetBottomLeft(new Vector2Int(lowerPoint.x, lowerPoint.y));
                    currentRect.SetTopRight(new Vector2Int(upperPoint.x, upperPoint.y));

                    // Also look at the tiles above and below our path
                    Tile belowTile = tileMapCopy.GetTile(lowerPoint - new Vector3Int(0, 1, 0)) as Tile;
                    Tile aboveTile = tileMapCopy.GetTile(upperPoint + new Vector3Int(0, 1, 0)) as Tile;

                    // We don't want to create paths next to paths that already exist
                    if ((_tileset.GetNameOfTile(belowTile) == "Carpet_Center") || _tileset.GetNameOfTile(aboveTile) == "Carpet_Center")
                    {
                        continue;
                    }

                    // Get the actual tiles at our two points
                    Tile lowerTile = tileMapCopy.GetTile(lowerPoint) as Tile;
                    Tile upperTile = tileMapCopy.GetTile(upperPoint) as Tile;

                    // A flag for checking if the path would create a column that is too short and doesn't look right
                    bool createsShortColumn = false;

                    // Extend the path from the edge of the room towards the center, until it breaks out into the room
                    while ((lowerTile != null) && (upperTile != null))
                    {
                        
                        belowTile = tileMapCopy.GetTile(lowerPoint - new Vector3Int(0, 1, 0)) as Tile;

                        // Once we hit a tile that is not Black, we know we have broken into the room
                        if ((_tileset.GetNameOfTile(lowerTile) != "Black") || (_tileset.GetNameOfTile(upperTile) != "Black"))
                        {
                            break;
                        }
                        
                        // If the tile below our path is not Black, that is a sign that the path would create a short column
                        if ((belowTile != null) && (_tileset.GetNameOfTile(belowTile) != "Black"))
                        {
                            createsShortColumn = true;
                        }

                        // Extend the path towards the room by one tile
                        if (x == 0)
                        {

                            lowerPoint += new Vector3Int(1, 0, 0);
                            upperPoint += new Vector3Int(1, 0, 0);
                        
                        }
                        else if (x == tileMapCopy.size.x - 1)
                        {

                            lowerPoint -= new Vector3Int(1, 0, 0);
                            upperPoint -= new Vector3Int(1, 0, 0);

                        }
                        lowerTile = tileMapCopy.GetTile(lowerPoint) as Tile;
                        upperTile = tileMapCopy.GetTile(upperPoint) as Tile;

                    }

                    // Disregard the path if it is not valid, like if the path would come out on a non-flat part of wall
                    if ((lowerTile != upperTile) || (lowerPoint.x > tileMapCopy.size.x - 1) || (lowerPoint.x < 0))
                    {
                        continue;
                    }
                    else if (createsShortColumn)
                    {
                        continue;
                    }

                    // Otherwise, the path is valid. So update the corners of our rect

                    // So update our Rectangle
                    if (upperPoint.x > currentRect.topRight.x)
                    {
                        currentRect.SetTopRight(new Vector2Int(upperPoint.x, upperPoint.y));
                    }
                    else if (lowerPoint.x < currentRect.bottomLeft.x)
                    {
                        currentRect.SetBottomLeft(new Vector2Int(lowerPoint.x, lowerPoint.y));
                    }

                    if (x == 0)
                    {
                        currentRect.SetDirection(ExitDirection.LEFT);
                    }
                    else if (x == tileMapCopy.size.x - 1)
                    {
                        currentRect.SetDirection(ExitDirection.RIGHT);
                    }

                    // Add it to our list of Rectangles
                    exitRects.Add(currentRect);

                    // And update our local tilemap
                    lowerPoint = new Vector3Int(x, y, 0);
                    upperPoint = new Vector3Int(x, y + 1, 0);
                    tileMapCopy.SetTile(lowerPoint, _tileset.GetTileByName("Carpet_Center"));
                    tileMapCopy.SetTile(upperPoint, _tileset.GetTileByName("Carpet_Center"));

                }
                // If we are at a point on the top or bottom of the room
                else if (((y == 0) || (y == tileMapCopy.size.y - 1)) && (x >= 1) && (x < tileMapCopy.size.x - 2))
                {

                    ExitPathRectangle currentRect = new ExitPathRectangle();

                    // Pick the current point and the point to the right of it to mark our current path
                    Vector3Int leftPoint = new Vector3Int(x, y, 0);
                    Vector3Int rightPoint = new Vector3Int(x + 1, y, 0);

                    // Initialize our rectangle
                    currentRect.SetBottomLeft(new Vector2Int(leftPoint.x, leftPoint.y));
                    currentRect.SetTopRight(new Vector2Int(rightPoint.x, rightPoint.y));

                    // Check the tiles to the left and right of our path
                    Tile tileToleft = tileMapCopy.GetTile(leftPoint - new Vector3Int(1, 0, 0)) as Tile;
                    Tile tileToRight = tileMapCopy.GetTile(rightPoint + new Vector3Int(1, 0, 0)) as Tile;

                    // We don't want to create paths right next to paths that already exist
                    if ((_tileset.GetNameOfTile(tileToleft) == "Carpet_Center") || _tileset.GetNameOfTile(tileToRight) == "Carpet_Center")
                    {
                        continue;
                    }

                    // Get the actual tiles at our two points
                    Tile leftTile = tileMapCopy.GetTile(leftPoint) as Tile;
                    Tile rightTile = tileMapCopy.GetTile(rightPoint) as Tile;

                    // Extend the path until it reaches into the room
                    while ((leftTile != null) && (rightTile != null))
                    {

                        // Extend it in the right direction depending on what side we are starting at
                        if (y == 0)
                        {

                            leftPoint += new Vector3Int(0, 1, 0);
                            rightPoint += new Vector3Int(0, 1, 0);
                        
                        }
                        else if (y == tileMapCopy.size.y - 1)
                        {

                            leftPoint -= new Vector3Int(0, 1, 0);
                            rightPoint -= new Vector3Int(0, 1, 0);

                        }

                        leftTile = tileMapCopy.GetTile(leftPoint) as Tile;
                        rightTile = tileMapCopy.GetTile(rightPoint) as Tile;

                    }

                    // Disregard that path if it is not valid.
                    if (((leftTile == null) && (rightTile != null)) || ((rightTile == null) && (leftTile != null)))
                    {
                        continue;
                    }
                    else if ((leftPoint.y > tileMapCopy.size.y - 1) || (leftPoint.y < 0))
                    {
                        continue;
                    }

                    // If it is valid, updapte our rectangle
                    if (rightPoint.y > currentRect.topRight.y)
                    {
                        currentRect.SetTopRight(new Vector2Int(rightPoint.x, rightPoint.y));
                    }
                    else if (leftPoint.y < currentRect.bottomLeft.y)
                    {
                        currentRect.SetBottomLeft(new Vector2Int(leftPoint.x, leftPoint.y));
                    }

                    if (y == 0)
                    {
                        currentRect.SetDirection(ExitDirection.DOWN);
                    }
                    else if (y == tileMapCopy.size.y - 1)
                    {
                        currentRect.SetDirection(ExitDirection.UP);
                    }

                    // And add the rectangle to our list
                    exitRects.Add(currentRect);

                    // And update the local tilemap
                    leftPoint = new Vector3Int(x, y, 0);
                    rightPoint = new Vector3Int(x + 1, y, 0);
                    tileMapCopy.SetTile(leftPoint, _tileset.GetTileByName("Carpet_Center"));
                    tileMapCopy.SetTile(rightPoint, _tileset.GetTileByName("Carpet_Center"));

                }
                
            }

        }

        // Destory the copy we made
        Object.Destroy(objectCopy);

        return exitRects;

    }

    // Check if the specified column creates a valid room in the given tilemap
    private bool IsValidColumn(Tilemap tileMap, int width, int height, Vector2Int location)
    {

        // For optimization, we only need to check the tiles that surround the newly-placed column
        int startX = Mathf.Max(location.x - 1, 0);
        int endX = Mathf.Min(location.x + width, tileMap.size.x - 1);
        int startY = Mathf.Max(location.y - 1, 0);
        int endY = Mathf.Min(location.y + height, tileMap.size.y - 1);

        for (int x = startX; x <= endX; x++)
        {

            for (int y = startY; y <= endY; y++)
            {

                if ((x == startX) || (x == endX) || (y == startY) || (y == endY))
                {

                    Tile currentTile = tileMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                    Tile tileTopLeft = tileMap.GetTile(new Vector3Int(x - 1, y + 1, 0)) as Tile;
                    Tile tileTopRight = tileMap.GetTile(new Vector3Int(x + 1, y + 1, 0)) as Tile;
                    Tile tileBottomLeft = tileMap.GetTile(new Vector3Int(x - 1, y - 1, 0)) as Tile;
                    Tile tileBottomRight = tileMap.GetTile(new Vector3Int(x + 1, y - 1, 0)) as Tile;
                    Tile tileBelow = tileMap.GetTile(new Vector3Int(x, y - 1, 0)) as Tile;
                    Tile tileAbove = tileMap.GetTile(new Vector3Int(x, y + 1, 0)) as Tile;
                    Tile tileToLeft = tileMap.GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileToRight = tileMap.GetTile(new Vector3Int(x + 1, y, 0)) as Tile;

                    // This whole block basically disallows single-tile-wide gaps in the room
                    if (currentTile == null)
                    {

                        if (((tileBelow != null) && (tileAbove != null)) || ((tileToLeft != null) && (tileToRight != null)))
                        {
                            return false;
                        }
                        else if ((tileAbove == null) && (tileBelow == null) && (tileToLeft == null) && (tileToRight == null))
                        {
                            
                            if ((tileBottomLeft != null) && (tileTopRight != null))
                            {
                                return false;
                            }
                            else if ((tileBottomRight != null) && (tileTopLeft != null))
                            {
                                return false;
                            }

                        }
                        else if ((tileToRight == null) && (tileAbove == null) && (tileBelow != null) && (tileTopRight != null))
                        {
                            return false;
                        }
                        else if ((tileToRight == null) && (tileBelow == null) && (tileAbove != null) && (tileBottomRight != null))
                        {
                            return false;
                        }
                        else if ((tileAbove == null) && (tileToRight == null) && (tileToLeft != null) && (tileTopRight != null))
                        {
                            return false;
                        }
                        else if ((tileAbove == null) && (tileToLeft == null) && (tileToRight != null) && (tileTopLeft != null))
                        {
                            return false;
                        }

                    }
                    else if (currentTile != null)
                    {

                        if ((tileAbove == null) && (tileToLeft == null) && (tileTopLeft != null))
                        {
                            return false;
                        }
                        else if ((tileAbove == null) && (tileToRight == null) && (tileTopRight != null))
                        {
                            return false;
                        }
                        else if ((tileBelow == null) && (tileToRight == null) && (tileBottomRight != null))
                        {
                            return false;
                        }
                        else if ((tileBelow == null) && (tileToLeft == null) && (tileBottomLeft != null))
                        {
                            return false;
                        }

                    }

                    // This section disallows the creation of unreachable sections of the room
                    if (currentTile == null)
                    {

                        // Make a copy of the tilemap
                        Vector3 objectPosition = tileMap.gameObject.transform.position;
                        GameObject objectCopy = Object.Instantiate(tileMap.gameObject, objectPosition, Quaternion.identity);
                        objectCopy.SetActive(false);
                        objectCopy.transform.SetParent(tileMap.gameObject.transform.parent);
                        Tilemap tileMapCopy = objectCopy.GetComponent<Tilemap>();

                        // And use the flood fill tool at the current tile to attempt to fill the room with black tiles
                        tileMapCopy.FloodFill(new Vector3Int(x, y, 0), _tileset.GetTileByName("Black"));

                        // And then check to make sure the entire room is now filled
                        for (int i = 0; i < tileMapCopy.size.x; i++)
                        {

                            for (int j = 0; j < tileMapCopy.size.y; j++)
                            {

                                // If there is a section that is not filled, that means the room contains non-connected sections
                                if (tileMapCopy.GetTile(new Vector3Int(i, j, 0)) == null)
                                {

                                    Object.Destroy(objectCopy);
                                    return false;
                                
                                }

                            }

                        }

                        Object.Destroy(objectCopy);

                    }

                }

            }

        }

        // The room must have enough possible exists in order to be valid
        if (FindPossibleExitPaths(tileMap).Count < _numExits)
        {
            return false;
        }

        return true;

    }

    public static void PlaceWalls(Tilemap collisionMap)
    {

        ITileset tileset = DungeonTileset.Instance;

        // Iterate through the Collision layer and place brick walls where appropriate
        for (int x = 1; x < collisionMap.size.x - 1; x++)
        {

            for (int y = 1; y < collisionMap.size.y - 1; y++)
            {

                Tile currentTile = collisionMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile != null)
                {

                    Tile tileInFront = collisionMap.GetTile(new Vector3Int(x, y - 1, 0)) as Tile;

                    if ((tileset.GetNameOfTile(currentTile) == "Black") && (tileInFront == null) && (y != 0))
                    {
                        collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BottomWall_Mid"));
                    }
                    else if (tileInFront != null)
                    {
                        if ((tileset.GetNameOfTile(currentTile) == "Black") && (tileset.GetNameOfTile(tileInFront) == "BottomWall_Mid"))
                        {
                            collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Mid"));
                        }
                    }

                }

            }

        }

        // Iterate through the Collision layer and replace edges of brick walls with the appropriate "edge of wall" tiles
        for (int x = 1; x < collisionMap.size.x - 1; x++)
        {

            for (int y = 1; y < collisionMap.size.y - 1; y++)
            {

                Tile currentTile = collisionMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile != null)
                {

                    Tile tileToRight = collisionMap.GetTile(new Vector3Int(x + 1, y, 0)) as Tile;
                    Tile tileToLeft = collisionMap.GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileBottomLeft = collisionMap.GetTile(new Vector3Int(x - 1, y - 1, 0)) as Tile;
                    Tile tileBottomRight = collisionMap.GetTile(new Vector3Int(x + 1, y - 1, 0)) as Tile;

                    if (tileToRight != null)
                    {

                        if ((tileset.GetNameOfTile(currentTile) == "BottomWall_Mid") && (tileset.GetNameOfTile(tileToRight) == "Black"))
                        {
                            collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BottomWall_Right"));
                        }
                        else if ((tileset.GetNameOfTile(currentTile) == "TopWall_Mid") && (tileset.GetNameOfTile(tileToRight) == "Black"))
                        {
                            collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Right"));
                        }

                    }

                    if (tileToLeft != null)
                    {

                        if ((tileset.GetNameOfTile(currentTile) == "BottomWall_Mid") && (tileset.GetNameOfTile(tileToLeft) == "Black"))
                        {
                            collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BottomWall_Left"));
                        }
                        else if ((tileset.GetNameOfTile(currentTile) == "TopWall_Mid") && (tileset.GetNameOfTile(tileToLeft) == "Black"))
                        {
                            collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Left"));
                        }

                    }

                    if ((tileToLeft == null) && (tileBottomLeft != null) && tileset.GetNameOfTile(currentTile) == "TopWall_Mid")
                    {
                        collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Left"));
                    }
                    else if ((tileToRight == null) && (tileBottomRight != null) && tileset.GetNameOfTile(currentTile) == "TopWall_Mid")
                    {
                        collisionMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Right"));
                    }

                }

            }

        }

    }

    public static void PlaceBorders(Tilemap bordersMap, Tilemap collisionMap)
    {

        ITileset tileset = DungeonTileset.Instance;

        // Iterate through the Collision layer and place the correct border tiles in the Borders layer where appropriate
        for (int x = 0; x < collisionMap.size.x; x++)
        {

            for (int y = 0; y < collisionMap.size.y; y++)
            {

                Tile currentTile = collisionMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile == null)
                {

                    Tile tileBelow = collisionMap.GetTile(new Vector3Int(x, y - 1, 0)) as Tile;
                    Tile tileAbove = collisionMap.GetTile(new Vector3Int(x, y + 1, 0)) as Tile;
                    Tile tileToLeft = collisionMap.GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileToRight = collisionMap.GetTile(new Vector3Int(x + 1, y, 0)) as Tile;

                    if ((tileBelow != null) && (tileset.GetNameOfTile(tileBelow) == "Black"))
                    {
                        bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorder_Top"));
                    }
                    else if ((tileAbove != null) && (tileset.GetNameOfTile(tileAbove) == "Black"))
                    {
                        bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorder_Bottom"));
                    }

                    currentTile = collisionMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                    if ((tileToLeft != null) && (currentTile == null))
                    {

                        if (tileset.GetNameOfTile(tileToLeft) == "Black")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorder_Right"));
                        }
                        else if (tileset.GetNameOfTile(tileToLeft) == "BottomWall_Mid")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Right"));
                        }
                        else if (tileset.GetNameOfTile(tileToLeft) == "TopWall_Mid")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Right"));
                        }
                        else if (tileset.GetNameOfTile(tileToLeft) == "BottomWall_Left")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Right"));
                        }
                        else if (tileset.GetNameOfTile(tileToLeft) == "TopWall_Left")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Right"));
                        }

                    }

                    currentTile = collisionMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                    if ((tileToRight != null)  && (currentTile == null))
                    {

                        if (tileset.GetNameOfTile(tileToRight) == "Black")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorder_Left"));
                        }
                        else if (tileset.GetNameOfTile(tileToRight) == "BottomWall_Mid")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Left"));
                        }
                        else if (tileset.GetNameOfTile(tileToRight) == "TopWall_Mid")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Left"));
                        }
                        else if (tileset.GetNameOfTile(tileToRight) == "BottomWall_Right")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Left"));
                        }
                        else if (tileset.GetNameOfTile(tileToRight) == "TopWall_Right")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("WallBorder_Left"));
                        }

                    }

                }

            }

        }        

        // Iterate through the Collision layer and replace certain border tiles in the Borders layer with "corner" borders
        for (int x = 0; x < collisionMap.size.x; x++)
        {

            for (int y = 0; y < collisionMap.size.y; y++)
            {

                // Place inner-BlackBorder-tiles
                Tile currentTile = collisionMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile == null || tileset.GetNameOfTile(currentTile) != "Black")
                {

                    Tile tileBelow = collisionMap.GetTile(new Vector3Int(x, y - 1, 0)) as Tile;
                    Tile tileToLeft = collisionMap.GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileToRight = collisionMap.GetTile(new Vector3Int(x + 1, y, 0)) as Tile;

                    if ((tileBelow != null) && (tileToRight != null))
                    {

                        if (tileset.GetNameOfTile(tileBelow) == "Black")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorder_BottomRight"));
                        }

                    }

                    if ((tileBelow != null) && (tileToLeft != null))
                    {

                        if (tileset.GetNameOfTile(tileBelow) == "Black")
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorder_BottomLeft"));
                        }

                    }

                }

                // Place outter-BlackBorder-tiles
                currentTile = bordersMap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile == null)
                {

                    Tile tileBelow = bordersMap.GetTile(new Vector3Int(x, y - 1, 0)) as Tile;
                    Tile tileAbove = bordersMap.GetTile(new Vector3Int(x, y + 1, 0)) as Tile;
                    Tile tileToLeft = bordersMap.GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileToRight = bordersMap.GetTile(new Vector3Int(x + 1, y, 0)) as Tile;          

                    if ((tileBelow != null) && (tileset.GetNameOfTile(tileBelow) == "BlackBorder_Left"))
                    {
                        if ((tileToRight != null) && (tileset.GetNameOfTile(tileToRight) == "BlackBorder_Top"))
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorderCorner_TopLeft"));
                        }
                    }
                    else if ((tileBelow != null) && (tileset.GetNameOfTile(tileBelow) == "BlackBorder_Right"))
                    {
                        if ((tileToLeft != null) && (tileset.GetNameOfTile(tileToLeft) == "BlackBorder_Top"))
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorderCorner_TopRight"));
                        }
                    }          
                    else if ((tileAbove != null) && (tileset.GetNameOfTile(tileAbove) == "BlackBorder_Left"))
                    {
                        if ((tileToRight != null) && (tileset.GetNameOfTile(tileToRight) == "BlackBorder_Bottom"))
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorderCorner_BottomLeft"));
                        }
                    } 
                    else if ((tileAbove != null) && (tileset.GetNameOfTile(tileAbove) == "BlackBorder_Right"))
                    {
                        if ((tileToLeft != null) && (tileset.GetNameOfTile(tileToLeft) == "BlackBorder_Bottom"))
                        {
                            bordersMap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BlackBorderCorner_BottomRight"));
                        }
                    } 

                }

            }

        }  

    }

    // Draw a filled rectangle of the specified tile onto the specified tilemap
    private void PlaceRectangleFilled(Tilemap tileMap, string tileName, int width, int height, Vector2Int location)
    {

        for (int i = location.x; i < location.x + width; i++)
        {
            for (int j = location.y; j < location.y + height; j++)
            {       
                tileMap.SetTile(new Vector3Int(i, j, 0), _tileset.GetTileByName(tileName));
            }
        }

    }

    // Draw a hollow rectangle of the specified tile onto the specified tilemap
    private void PlaceRectangleHollow(Tilemap tileMap, string tileName, int width, int height, Vector2Int location)
    {

        PlaceRectangleFilled(tileMap, tileName, width, 1, location);
        PlaceRectangleFilled(tileMap, tileName, 1, height, location);
        PlaceRectangleFilled(tileMap, tileName, width, 1, new Vector2Int(location.x, location.y + height - 1));
        PlaceRectangleFilled(tileMap, tileName, 1, height, new Vector2Int(location.x + width - 1, location.y));

    }

    private void PlaceRectangleFilled(string tileMapName, string tileName, int width, int height, Vector2Int location)
    {
        PlaceRectangleFilled(_tileMaps[tileMapName], tileName, width, height, location);
    }

    private void PlaceRectangleHollow(string tileMapName, string tileName, int width, int height, Vector2Int location)
    {
        PlaceRectangleHollow(_tileMaps[tileMapName], tileName, width, height, location);
    }

}