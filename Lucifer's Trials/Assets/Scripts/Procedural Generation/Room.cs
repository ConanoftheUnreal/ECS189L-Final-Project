using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Room
{

    private GameObject _roomObject;
    private List<ExitPathRectangle> _exitPaths;

    private ITileset tileset = DungeonTileset.Instance;

    private GameObject _groundObject;
    private GameObject _collisionObject;
    private GameObject _bordersObject;
    private GameObject _exitsObject;
    private GameObject _decorationsObject;

    private Tilemap _collisionTilemap;
    private Tilemap _bordersTilemap;
    private Tilemap _decorationsTilemap;
    private Transform _exitsTransform;

    public List<ExitPathRectangle> exitPaths
    {
        get
        {
            return _exitPaths;
        }
    }

    public Room(GameObject roomObject, List<ExitPathRectangle> exitPaths)
    {

        _roomObject = roomObject;
        _exitPaths = exitPaths;

        _groundObject = _roomObject.transform.Find("Ground").gameObject;
        _collisionObject = _roomObject.transform.Find("Collision").gameObject;
        _bordersObject = _roomObject.transform.Find("Borders").gameObject;
        _exitsObject = _roomObject.transform.Find("Exits").gameObject;
        _decorationsObject = _roomObject.transform.Find("Decorations").gameObject;

        _collisionTilemap = _collisionObject.GetComponent<Tilemap>();
        _bordersTilemap = _bordersObject.GetComponent<Tilemap>();
        _decorationsTilemap = _decorationsObject.GetComponent<Tilemap>();
        _exitsTransform = _exitsObject.transform;

    }

    public GameObject roomObject
    {
        get
        {
            return _roomObject;
        }
    }

    public void DisableRoom()
    {
   
        _groundObject.SetActive(false);
        _collisionObject.SetActive(false);
        _bordersObject.SetActive(false);
        _exitsObject.SetActive(false);
        _decorationsObject.SetActive(false);

    }

    public void EnableRoom()
    {

        _groundObject.SetActive(true);
        _collisionObject.SetActive(true);
        _bordersObject.SetActive(true);
        _exitsObject.SetActive(true);
        _decorationsObject.SetActive(true);

    }

    public void OpenAllExits()
    {
 
        for (int i = 0; i < _exitPaths.Count; i++)
        {
            OpenExit(i);
        }
    }

    public void OpenExit(int exitID)
    {

        ExitPathRectangle exitPath = _exitPaths[exitID];

        if (exitPath.direction != ExitDirection.UP)
        {

            for (int x = exitPath.bottomLeft.x; x <= exitPath.topRight.x; x++)
            {
                for (int y = exitPath.bottomLeft.y; y <= exitPath.topRight.y; y++)
                {     

                    _collisionTilemap.SetTile(new Vector3Int(x, y, 0), null);
                    _bordersTilemap.SetTile(new Vector3Int(x, y, 0), null);
                    _decorationsTilemap.SetTile(new Vector3Int(x, y, 0), null);

                }
            }

        }

        GameObject exitObject = new GameObject("Exit #" + exitPath.id.ToString());
        exitObject.transform.SetParent(_exitsTransform);
        BoxCollider2D exitCollider = exitObject.AddComponent<BoxCollider2D>();

        if (exitPath.direction == ExitDirection.LEFT)
        {

            Vector3 objectPosition = _collisionTilemap.CellToWorld(new Vector3Int(exitPath.bottomLeft.x, exitPath.bottomLeft.y + 1));
            exitObject.transform.position = objectPosition;
            exitCollider.size = new Vector2(0.5f, 2) * _roomObject.transform.localScale;

            exitPath.SetEntranceLocation(exitObject.transform.position + new Vector3(0.5f, 0, 0)); 

        }
        else if (exitPath.direction == ExitDirection.RIGHT)
        {

            Vector3 objectPosition = _collisionTilemap.CellToWorld(new Vector3Int(exitPath.topRight.x + 1, exitPath.topRight.y));
            exitObject.transform.position = objectPosition;
            exitCollider.size = new Vector2(0.5f, 2) * _roomObject.transform.localScale;                

            exitPath.SetEntranceLocation(exitObject.transform.position + new Vector3(-0.5f, 0, 0)); 

        }
        else if (exitPath.direction == ExitDirection.UP)
        {

            Vector3 objectPosition = _collisionTilemap.CellToWorld(new Vector3Int(exitPath.bottomLeft.x + 1, exitPath.bottomLeft.y + 1));
            exitObject.transform.position = objectPosition;
            exitCollider.size = new Vector2(2, 0.5f) * _roomObject.transform.localScale;             

            exitPath.SetEntranceLocation(exitObject.transform.position + new Vector3(0, -0.5f, 0));    

        }
        else if (exitPath.direction == ExitDirection.DOWN)
        {

            Vector3 objectPosition = _collisionTilemap.CellToWorld(new Vector3Int(exitPath.bottomLeft.x + 1, exitPath.bottomLeft.y));
            exitObject.transform.position = objectPosition;
            exitCollider.size = new Vector2(2, 0.5f) * _roomObject.transform.localScale;  

            exitPath.SetEntranceLocation(exitObject.transform.position + new Vector3(0, 0.5f, 0));              

        }

        exitObject.AddComponent<ExitManager>();

        _bordersTilemap.ClearAllTiles();

        // If clearing the way for the exit caused some "middle" bicks to become "side" bricks, update them.
        for (int x = 0; x < _collisionTilemap.size.x; x++)
        {

            for (int y = 0; y < _collisionTilemap.size.y; y++)
            {

                Tile currentTile = _collisionTilemap.GetTile(new Vector3Int(x, y, 0)) as Tile;
                Tile tileToRight = _collisionTilemap.GetTile(new Vector3Int(x + 1, y, 0)) as Tile;
                Tile tileToLeft = _collisionTilemap.GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                Tile tileBottomLeft = _collisionTilemap.GetTile(new Vector3Int(x - 1, y - 1, 0)) as Tile;
                Tile tileBottomRight = _collisionTilemap.GetTile(new Vector3Int(x + 1, y - 1, 0)) as Tile;
                
                if (currentTile != null)
                {

                    if ((tileset.GetNameOfTile(currentTile) == "BottomWall_Right") && (tileToRight == null))
                    {
                        _collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BottomWall_Mid"));
                    }
                    else if ((tileset.GetNameOfTile(currentTile) == "BottomWall_Left") && (tileToLeft == null))
                    {
                        _collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BottomWall_Mid"));
                    }
                    else if ((tileset.GetNameOfTile(currentTile) == "TopWall_Right") && (tileToRight == null))
                    {
                        if (tileBottomRight == null)
                        {
                            _collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Mid"));
                        }
                    }
                    else if ((tileset.GetNameOfTile(currentTile) == "TopWall_Left") && (tileToLeft == null))
                    {
                        if (tileBottomLeft == null)
                        {
                            _collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Mid"));
                        }
                    }

                }

            }

        }

        DungeonGenerator.PlaceBorders(_bordersTilemap, _collisionTilemap);

    }

}

public enum ExitDirection
{

    UP,
    RIGHT,
    DOWN,
    LEFT

}

public class ExitPathRectangle
{

    private Vector2Int _bottomLeft;
    private Vector2Int _topRight;
    private ExitDirection _direction; 
    private int _id;
    private Vector2 _entranceLocation;

    public void SetEntranceLocation(Vector2 location)
    {
        _entranceLocation = location;
    }

    public Vector2 entranceLocation
    {
        get
        {
            return _entranceLocation;
        }
    }

    public ExitDirection direction
    {
        get
        {
            return _direction;
        }
    }

    public void SetDirection(ExitDirection direction)
    {
        _direction = direction;
    }

    public Vector2Int bottomLeft
    {
        get
        {
            return _bottomLeft;
        }
    }

    public void SetBottomLeft(Vector2Int bottomLeft)
    {
        _bottomLeft = bottomLeft;
    }

    public Vector2Int topRight
    {
        get
        {
            return _topRight;
        }
    }

    public void SetTopRight(Vector2Int topRight)
    {
        _topRight = topRight;
    }

    public int id
    {
        get
        {
            return _id;
        }
    }

    public void SetID(int id)
    {
        _id = id;
    }

}