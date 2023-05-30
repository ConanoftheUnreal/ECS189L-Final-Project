using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DungeonGenerator : IRoomGenerator
{

    private int _maxColumns;
    private int _maxColumnLength;

    private const int MIN_NUM_COLUMNS = 1;
    private const int MIN_COLUMN_HEIGHT = 3;
    private const int MAX_COLUMN_GENERATION_TRIES = 10;

    private Dictionary<string, Tile> _tiles = new Dictionary<string, Tile>();
    private Dictionary<string, Tile> _nameToTile = new Dictionary<string, Tile>();
    private Dictionary<Tile, string> _tileToName = new Dictionary<Tile, string>();
    private Dictionary<string, Tilemap> _tileMaps;

    public DungeonGenerator(int maxColumns, int maxColumnLength)
    {

        _maxColumns = maxColumns;
        _maxColumnLength = maxColumnLength;

        Tile[] allTiles = Resources.LoadAll<Tile>("Tiles/Dungeon_Tiles");
        foreach (Tile tile in allTiles)
        {
            _tiles.Add(tile.name, tile);
        }

        PairNameAndTile("Ground", _tiles["Dungeon_Tileset_71"]);
        PairNameAndTile("Black", _tiles["Dungeon_Tileset_0"]);

        PairNameAndTile("BottomWall_Mid", _tiles["Dungeon_Tileset_15"]);
        PairNameAndTile("BottomWall_Left", _tiles["Dungeon_Tileset_14"]);
        PairNameAndTile("BottomWall_Right", _tiles["Dungeon_Tileset_16"]);

        PairNameAndTile("TopWall_Mid", _tiles["Dungeon_Tileset_2"]);
        PairNameAndTile("TopWall_Left", _tiles["Dungeon_Tileset_1"]);
        PairNameAndTile("TopWall_Right", _tiles["Dungeon_Tileset_3"]);

        PairNameAndTile("BlackBorder_Top", _tiles["Dungeon_Tileset_43"]);
        PairNameAndTile("BlackBorder_Right", _tiles["Dungeon_Tileset_26"]);
        PairNameAndTile("BlackBorder_Left", _tiles["Dungeon_Tileset_27"]);
        PairNameAndTile("BlackBorder_BottomLeft", _tiles["Dungeon_Tileset_42"]);
        PairNameAndTile("BlackBorder_BottomRight", _tiles["Dungeon_Tileset_44"]);

        PairNameAndTile("WallBorder_Right", _tiles["Dungeon_Tileset_93"]);
        PairNameAndTile("WallBorder_Left", _tiles["Dungeon_Tileset_91"]);

        PairNameAndTile("Carpet_Center", _tiles["Dungeon_Tileset_95"]);

    }

    public GameObject Generate(int width, int height, Vector2 location)
    {

        _tileMaps = new Dictionary<string, Tilemap>();

        GameObject room = new GameObject("Room");
        room.transform.position = location;
        
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

        // Place Ground tiles
        PlaceRectangleFilled("Ground", "Ground", width, height + 2, new Vector2Int(0, 0));
        
        // Get position and size of ground area
        Vector3 groundPos = _tileMaps["Ground"].transform.position;
        Vector3Int groundSize = _tileMaps["Ground"].size;

        PlaceRectangleHollow("Collision", "Black", width + 2, height + 4, new Vector2Int(0, 0));
        PlaceRectangleFilled("Collision", "Black", width, 2, new Vector2Int(1, height + 1));

        int numColumns = Random.Range(MIN_NUM_COLUMNS, _maxColumns + 1);

        for (int i = 0; i < numColumns; i++)
        {

            int columnWidth;
            int columnHeight;
            int x;
            int y;
            int numTries = 0;

            do
            {

                numTries++;

                if (numTries <= MAX_COLUMN_GENERATION_TRIES)
                {

                    GameObject collLayerCopy = Object.Instantiate(collisionLayer, collisionLayer.transform.position, Quaternion.identity);
                    collLayerCopy.SetActive(false);
                    collLayerCopy.transform.SetParent(room.transform);
                    Tilemap collTileMapCopy = collLayerCopy.GetComponent<Tilemap>();

                    columnWidth = Random.Range(1, _maxColumnLength + 1);
                    columnHeight = Mathf.Max(Random.Range(1, _maxColumnLength + 1), MIN_COLUMN_HEIGHT);
                    x = Random.Range(1, width + 1);
                    y = Random.Range(1, height + 1);

                    if (x + columnWidth > _tileMaps["Collision"].size.x)
                    {
                        columnWidth -= (x + columnWidth - _tileMaps["Collision"].size.x);
                    }
                    if (y + columnHeight > _tileMaps["Collision"].size.y)
                    {
                        columnHeight -= (y + columnHeight - _tileMaps["Collision"].size.y);
                    }

                    PlaceRectangleFilled(collTileMapCopy, "Black", columnWidth, columnHeight, new Vector2Int(x, y));

                    if (IsValidColumn(collTileMapCopy, columnWidth, columnHeight, new Vector2Int(x, y)))
                    {

                        PlaceRectangleFilled("Collision", "Black", columnWidth, columnHeight, new Vector2Int(x, y));
                        Object.Destroy(collLayerCopy);
                        break;

                    }

                    Object.Destroy(collLayerCopy);

                }
                else
                {
                    break;
                }

            } while (true);

        }

        for (int x = 0; x < _tileMaps["Collision"].size.x; x++)
        {

            for (int y = 0; y < _tileMaps["Collision"].size.y; y++)
            {

                Tile currentTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile != null)
                {

                    Tile tileInFront = _tileMaps["Collision"].GetTile(new Vector3Int(x, y - 1, 0)) as Tile;

                    if ((_tileToName[currentTile] == "Black") && (tileInFront == null) && (y != 0))
                    {
                        _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BottomWall_Mid"]);
                    }
                    else if (tileInFront != null)
                    {
                        if ((_tileToName[currentTile] == "Black") && (_tileToName[tileInFront] == "BottomWall_Mid"))
                        {
                            _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["TopWall_Mid"]);
                        }
                    }

                }

            }

        }

        for (int x = 0; x < _tileMaps["Collision"].size.x; x++)
        {

            for (int y = 0; y < _tileMaps["Collision"].size.y; y++)
            {

                Tile currentTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile != null)
                {

                    Tile tileToRight = _tileMaps["Collision"].GetTile(new Vector3Int(x + 1, y, 0)) as Tile;
                    Tile tileToLeft = _tileMaps["Collision"].GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileBottomLeft = _tileMaps["Collision"].GetTile(new Vector3Int(x - 1, y - 1, 0)) as Tile;
                    Tile tileBottomRight = _tileMaps["Collision"].GetTile(new Vector3Int(x + 1, y - 1, 0)) as Tile;

                    if (tileToRight != null)
                    {

                        if ((_tileToName[currentTile] == "BottomWall_Mid") && (_tileToName[tileToRight] == "Black"))
                        {
                            _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BottomWall_Right"]);
                        }
                        else if ((_tileToName[currentTile] == "TopWall_Mid") && (_tileToName[tileToRight] == "Black"))
                        {
                            _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["TopWall_Right"]);
                        }

                    }

                    if (tileToLeft != null)
                    {

                        if ((_tileToName[currentTile] == "BottomWall_Mid") && (_tileToName[tileToLeft] == "Black"))
                        {
                            _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BottomWall_Left"]);
                        }
                        else if ((_tileToName[currentTile] == "TopWall_Mid") && (_tileToName[tileToLeft] == "Black"))
                        {
                            _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["TopWall_Left"]);
                        }

                    }

                    if ((tileToLeft == null) && (tileBottomLeft != null) && _tileToName[currentTile] == "TopWall_Mid")
                    {
                        _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["TopWall_Left"]);
                    }
                    else if ((tileToRight == null) && (tileBottomRight != null) && _tileToName[currentTile] == "TopWall_Mid")
                    {
                        _tileMaps["Collision"].SetTile(new Vector3Int(x, y, 0), _nameToTile["TopWall_Right"]);
                    }

                }

            }

        }

        PlaceRectangleHollow("Borders", "Black", width + 2, height + 4, new Vector2Int(0, 0));

        for (int x = 0; x < _tileMaps["Collision"].size.x; x++)
        {

            for (int y = 0; y < _tileMaps["Collision"].size.y; y++)
            {

                Tile currentTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile == null)
                {

                    Tile tileInFront = _tileMaps["Collision"].GetTile(new Vector3Int(x, y - 1, 0)) as Tile;
                    Tile tileToLeft = _tileMaps["Collision"].GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileToRight = _tileMaps["Collision"].GetTile(new Vector3Int(x + 1, y, 0)) as Tile;

                    if (tileInFront != null)
                    {

                        if (_tileToName[tileInFront] == "Black")
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BlackBorder_Top"]);
                        }     

                    }

                    currentTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                    if ((tileToLeft != null) && (currentTile == null))
                    {

                        if (_tileToName[tileToLeft] == "Black")
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BlackBorder_Right"]);
                        }
                        else if ((_tileToName[tileToLeft] == "BottomWall_Mid") || (_tileToName[tileToLeft] == "TopWall_Mid") ||
                                (_tileToName[tileToLeft] == "BottomWall_Left") || (_tileToName[tileToLeft] == "TopWall_Left"))
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["WallBorder_Right"]);
                        }    

                    }

                    currentTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                    if ((tileToRight != null)  && (currentTile == null))
                    {

                        if (_tileToName[tileToRight] == "Black")
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BlackBorder_Left"]);
                        }
                        else if ((_tileToName[tileToRight] == "BottomWall_Mid") || (_tileToName[tileToRight] == "TopWall_Mid") ||
                                (_tileToName[tileToRight] == "BottomWall_Right") || (_tileToName[tileToRight] == "TopWall_Right"))
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["WallBorder_Left"]);
                        }     

                    }

                }

            }

        }        

        for (int x = 0; x < _tileMaps["Collision"].size.x; x++)
        {

            for (int y = 0; y < _tileMaps["Collision"].size.y; y++)
            {

                Tile currentTile = _tileMaps["Collision"].GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile == null || _tileToName[currentTile] != "Black")
                {

                    Tile tileBelow = _tileMaps["Collision"].GetTile(new Vector3Int(x, y - 1, 0)) as Tile;
                    Tile tileToLeft = _tileMaps["Collision"].GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileToRight = _tileMaps["Collision"].GetTile(new Vector3Int(x + 1, y, 0)) as Tile;

                    if ((tileBelow != null) && (tileToRight != null))
                    {

                        if ((_tileToName[tileBelow] == "Black"))
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BlackBorder_BottomRight"]);
                        }

                    }

                    if ((tileBelow != null) && (tileToLeft != null))
                    {

                        if ((_tileToName[tileBelow] == "Black"))
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BlackBorder_BottomLeft"]);
                        }

                    }

                }

            }

        }  

        // Place Collision Tilemap at the bottom left of the Ground Tilemap so that it goes around it 
        _tileMaps["Collision"].transform.position = new Vector3(groundPos.x - 1, groundPos.y - 1, groundPos.z);

        _tileMaps["Borders"].transform.position = new Vector3(groundPos.x - 1, groundPos.y - 1, groundPos.z);

        return room;

    }

    private bool IsValidColumn(Tilemap tileMap, int width, int height, Vector2Int location)
    {

        int startX = Mathf.Max(location.x - 1, 0);
        int endX = Mathf.Min(location.x + width, tileMap.size.x - 1);
        int startY = Mathf.Max(location.y - 1, 0);
        int endY = Mathf.Min(location.y + height, tileMap.size.y - 1);

        for (int x = startX; x <= endX; x++)
        {

            for (int y = startY; y <= endY; y++)
            {

                if (x == startX || x == endX || y == startY || y == endY)
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

                    if (currentTile == null)
                    {

                        Vector3 objectPosition = tileMap.gameObject.transform.position;
                        GameObject objectCopy = Object.Instantiate(tileMap.gameObject, objectPosition, Quaternion.identity);
                        objectCopy.SetActive(false);
                        objectCopy.transform.SetParent(tileMap.gameObject.transform.parent);
                        Tilemap tileMapCopy = objectCopy.GetComponent<Tilemap>();

                        tileMapCopy.FloodFill(new Vector3Int(x, y, 0), _nameToTile["Black"]);

                        for (int i = 0; i < tileMapCopy.size.x; i++)
                        {

                            for (int j = 0; j < tileMapCopy.size.y; j++)
                            {

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

        return true;

    }

    private void PairNameAndTile(string name, Tile tile)
    {

        _nameToTile.Add(name, tile);
        _tileToName.Add(tile, name);

    }

    private void PlaceRectangleFilled(string tileMapName, string tileName, int width, int height, Vector2Int location)
    {
        PlaceRectangleFilled(_tileMaps[tileMapName], tileName, width, height, location);
    }

    private void PlaceRectangleFilled(Tilemap tileMap, string tileName, int width, int height, Vector2Int location)
    {

        for (int i = location.x; i < location.x + width; i++)
        {
            for (int j = location.y; j < location.y + height; j++)
            {       
                tileMap.SetTile(new Vector3Int(i, j, 0), _nameToTile[tileName]);
            }
        }

    }

    private void PlaceRectangleHollow(string tileMapName, string tileName, int width, int height, Vector2Int location)
    {
        PlaceRectangleHollow(_tileMaps[tileMapName], tileName, width, height, location);
    }

    private void PlaceRectangleHollow(Tilemap tileMap, string tileName, int width, int height, Vector2Int location)
    {

        PlaceRectangleFilled(tileMap, tileName, width, 1, location);
        PlaceRectangleFilled(tileMap, tileName, 1, height, location);
        PlaceRectangleFilled(tileMap, tileName, width, 1, new Vector2Int(location.x, location.y + height - 1));
        PlaceRectangleFilled(tileMap, tileName, 1, height, new Vector2Int(location.x + width - 1, location.y));

    }

}
