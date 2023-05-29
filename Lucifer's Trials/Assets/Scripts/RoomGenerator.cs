using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RoomGenerator
{

    private int _maxObstacles;
    private int _maxObstacleLength;

    private const int MIN_OBSTACLES = 1;
    private const int MIN_COLUMN_HEIGHT = 3;

    private Dictionary<string, Tile> _tiles = new Dictionary<string, Tile>();
    private Dictionary<string, Tile> _nameToTile = new Dictionary<string, Tile>();
    private Dictionary<Tile, string> _tileToName = new Dictionary<Tile, string>();
    private Dictionary<string, Tilemap> _tileMaps;

    private enum ObstacleType
    {

        COLUMN

    }

    public RoomGenerator(int maxObstacles, int maxObstacleLength)
    {

        _maxObstacles = maxObstacles;
        _maxObstacleLength = maxObstacleLength;

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

        ObstacleType[] obstacles = new ObstacleType[Random.Range(MIN_OBSTACLES, _maxObstacles + 1)];
        for (int i = 0; i < obstacles.Length; i ++)
        {
            obstacles[i] = (ObstacleType)(Random.Range(0, System.Enum.GetNames(typeof(ObstacleType)).Length));
        }

        foreach (ObstacleType obstacle in obstacles)
        {

            switch(obstacle)
            {

                case ObstacleType.COLUMN:

                    int columnWidth;
                    int columnHeight;
                    int x;
                    int y;

                    do
                    {

                        columnWidth = Random.Range(1, _maxObstacleLength + 1);
                        columnHeight = Mathf.Max(Random.Range(1, _maxObstacleLength + 1), MIN_COLUMN_HEIGHT);
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

                    } while ((x == 2) || (x + columnWidth == _tileMaps["Collision"].size.x - 2) ||
                            (y == 2) || (y + columnHeight == _tileMaps["Collision"].size.y - 2));

                    PlaceRectangleFilled("Collision", "Black", columnWidth, columnHeight, new Vector2Int(x, y));

                    break;

            }

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

                if (currentTile == null)
                {

                    Tile tileBelow = _tileMaps["Collision"].GetTile(new Vector3Int(x, y - 1, 0)) as Tile;
                    Tile tileToLeft = _tileMaps["Collision"].GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                    Tile tileToRight = _tileMaps["Collision"].GetTile(new Vector3Int(x + 1, y, 0)) as Tile;

                    if ((tileBelow != null) && (tileToRight != null))
                    {

                        if ((_tileToName[tileBelow] == "Black") && (_tileToName[tileToRight] == "Black"))
                        {
                            _tileMaps["Borders"].SetTile(new Vector3Int(x, y, 0), _nameToTile["BlackBorder_BottomRight"]);
                        }

                    }

                    if ((tileBelow != null) && (tileToLeft != null))
                    {

                        if ((_tileToName[tileBelow] == "Black") && (_tileToName[tileToLeft] == "Black"))
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

    private void PairNameAndTile(string name, Tile tile)
    {

        _nameToTile.Add(name, tile);
        _tileToName.Add(tile, name);

    }

    private void PlaceRectangleFilled(string tileMapName, string tileName, int width, int height, Vector2Int location)
    {

        for (int i = location.x; i < location.x + width; i++)
        {
            for (int j = location.y; j < location.y + height; j++)
            {       
                _tileMaps[tileMapName].SetTile(new Vector3Int(i, j, 0), _nameToTile[tileName]);
            }
        }

    }

    private void PlaceRectangleHollow(string tileMapName, string tileName, int width, int height, Vector2Int location)
    {

        Tilemap tileMap = _tileMaps[tileMapName];

        PlaceRectangleFilled(tileMapName, tileName, width, 1, location);
        PlaceRectangleFilled(tileMapName, tileName, 1, height, location);
        PlaceRectangleFilled(tileMapName, tileName, width, 1, new Vector2Int(location.x, location.y + tileMap.size.y - 1));
        PlaceRectangleFilled(tileMapName, tileName, 1, height, new Vector2Int(location.x + tileMap.size.x - 1, location.y));

    }

}
