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

    private Vector2 _centerPoint;

    private Dictionary<Vector2Int, GameObject> _wallObjects = new Dictionary<Vector2Int, GameObject>();

    private List<GameObject> _enemyObjects = new List<GameObject>();
    private List<Vector2Int> _possibleSpawnLocations;

    private const int WALL_LAYER = 3;

    private const float MIN_SPAWN_DISTANCE_FROM_ENTRANCE = 5.0f;
    private const int MIN_ENEMIES_SPAWN = 3;
    private const int INITIAL_MAX_ENEMIES = 7;
    private const float SLINGER_SPAWN_RATE = 0.3f;

    public List<ExitPathRectangle> exitPaths
    {
        get
        {
            return _exitPaths;
        }
    }
    
    public GameObject roomObject
    {
        get
        {
            return _roomObject;
        }
    }

    public GameObject collisionObject
    {
        get
        {
            return _collisionObject;
        }
    }

    public GameObject decorationsObject
    {
        get
        {
            return _decorationsObject;
        }
    }

    public Vector2 centerPoint
    {
        get
        {
            return _centerPoint;
        }
    }

    public int numAliveEnemies
    {
        get
        {
            return _enemyObjects.Count;
        }
    }

    public Room(GameObject roomObject, List<ExitPathRectangle> exitPaths)
    {

        // A room is parameterized with a game object for the room and a list of exit paths
        _roomObject = roomObject;
        _exitPaths = exitPaths;

        // Get all of the relevant child gameobjects of this room
        _groundObject = _roomObject.transform.Find("Ground").gameObject;
        _collisionObject = _roomObject.transform.Find("Collision").gameObject;
        _bordersObject = _roomObject.transform.Find("Borders").gameObject;
        _exitsObject = _roomObject.transform.Find("Exits").gameObject;
        _decorationsObject = _roomObject.transform.Find("Decorations").gameObject;

        _collisionTilemap = _collisionObject.GetComponent<Tilemap>();
        _bordersTilemap = _bordersObject.GetComponent<Tilemap>();
        _decorationsTilemap = _decorationsObject.GetComponent<Tilemap>();
        _exitsTransform = _exitsObject.transform;

        // Create Wall objects for AI pathfinding
        CreateWallObjects();

        // Open the entrance to this room
        OpenExit(LevelGenerator.ENTRANCE_EXIT_ID);

        // Find the possible locations for spawning enemies
        _possibleSpawnLocations = FindPossibleEnemySpawns();

        // Find the center of the room
        Vector3 roomSize = new Vector3((float)(_collisionTilemap.size.x), (float)(_collisionTilemap.size.y), 0);
        // Subtract 1 from the y value to account for the back wall which is thicker than all the other surrounding walls
        _centerPoint = roomObject.transform.position + roomSize / 2 - new Vector3(0, 1, 0);

    }

    // Remove dead enemies from the list of enemies in the room
    public void RemoveEnemy(GameObject enemy)
    {
        _enemyObjects.Remove(enemy);
    }

    public void SpawnEnemies()
    {

        GameObject parentObject = GameObject.Find("Enemies");
        FactoryGoblinBeserker beserkerSpawner = parentObject.GetComponent<FactoryGoblinBeserker>();
        FactoryGoblinSlinger slingerSpawner = parentObject.GetComponent<FactoryGoblinSlinger>();

        // Increase the maximum number of enemies in the romo as the player completes levels
        int numEnemies = Random.Range(MIN_ENEMIES_SPAWN, (INITIAL_MAX_ENEMIES + PlayerStatsContainer.Instance.levelsFinished) + 1);
        List<Vector2Int> possibleSpawns = new List<Vector2Int>(_possibleSpawnLocations);

        for (int i = 0; i < numEnemies; i++)
        {

            Vector2Int randomSpawn = possibleSpawns[Random.Range(0, possibleSpawns.Count)];
            possibleSpawns.Remove(randomSpawn);
            
            //Offset the spawn location a bit to avoid issue with enemies spawning inside walls
            Vector3 spawnLocation = _collisionTilemap.CellToWorld(new Vector3Int(randomSpawn.x, randomSpawn.y, 0));
            spawnLocation = spawnLocation + new Vector3(0.5f, 1f, 0);

            Factory spawner;
            if (Random.Range(0f, 1f) < SLINGER_SPAWN_RATE)
            {
                spawner = slingerSpawner;
            }
            else
            {
                spawner = beserkerSpawner;
            }

            GameObject enemyObject = ((GoblinBeserker)(spawner.GetEnemy(spawnLocation))).gameObject;
            enemyObject.AddComponent<DeathManager>();
            _enemyObjects.Add(enemyObject);

        }

    }

    private List<Vector2Int> FindPossibleEnemySpawns()
    {

        List<Vector2Int> possibleSpawns = new List<Vector2Int>();
        Vector2 entranceLocation = exitPaths[LevelGenerator.ENTRANCE_EXIT_ID].entranceLocation;

        for (int x = 0; x < _collisionTilemap.size.x; x++)
        {

            for (int y = 0; y < _collisionTilemap.size.y; y++)
            {

                float distanceFromEntrance = LevelManager.Distance(entranceLocation, new Vector2Int(x, y));

                if ((_collisionTilemap.GetTile(new Vector3Int(x, y, 0)) == null) && (distanceFromEntrance >= MIN_SPAWN_DISTANCE_FROM_ENTRANCE))
                {
                    possibleSpawns.Add(new Vector2Int(x, y));
                }

            }

        }

        return possibleSpawns;

    }

    // Function for destroying the wall object at a given location, usually for clearing out exits
    private void DestroyWallObject(Vector2Int location)
    {
        Object.Destroy(_wallObjects[location]);
    }

    private void CreateWallObjects()
    {

        // Create game objects at every collision tile to help with enemy AI
        for (int x = 0; x < _collisionTilemap.size.x; x++)
        {

            for (int y = 0; y < _collisionTilemap.size.y; y++)
            {

                Tile currentTile = _collisionTilemap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                if (currentTile != null)
                {

                    // Create Wall object at coordinate
                    GameObject wallObject = new GameObject("Wall");
                    wallObject.transform.SetParent(_collisionObject.transform);
                    Vector3 offset = new Vector3(0.5f, 0.5f, 0);
                    wallObject.transform.position = _collisionTilemap.CellToWorld(new Vector3Int(x, y, 0)) + offset;
                    wallObject.layer = WALL_LAYER;

                    // Store Wall object in dictionary
                    _wallObjects.Add(new Vector2Int(x, y), wallObject);

                }

            }

        }        

    }

    public void DisableRoom()
    {
   
        // To disable room, basically just disable all of the tilemap-containing objects inside of it
        _groundObject.SetActive(false);
        _collisionObject.SetActive(false);
        _bordersObject.SetActive(false);
        _exitsObject.SetActive(false);
        _decorationsObject.SetActive(false);

        foreach (GameObject enemy in _enemyObjects)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }

    }

    public void EnableRoom()
    {

        // And do the reverse for enabling the room
        _groundObject.SetActive(true);
        _collisionObject.SetActive(true);
        _bordersObject.SetActive(true);
        _exitsObject.SetActive(true);
        _decorationsObject.SetActive(true);

        foreach (GameObject enemy in _enemyObjects)
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }

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

        // Don't allow opening an exit that has already been opened
        if (exitPath.isOpen)
        {
            return;
        }

        // If the exit does not exit UP, we need to clear away that wall for the exit
        if (exitPath.direction != ExitDirection.UP)
        {

            for (int x = exitPath.bottomLeft.x; x <= exitPath.topRight.x; x++)
            {
                for (int y = exitPath.bottomLeft.y; y <= exitPath.topRight.y; y++)
                {     

                    Tile currentCollisionTile = _collisionTilemap.GetTile(new Vector3Int(x, y, 0)) as Tile;

                    _collisionTilemap.SetTile(new Vector3Int(x, y, 0), null);
                    _bordersTilemap.SetTile(new Vector3Int(x, y, 0), null);
                    _decorationsTilemap.SetTile(new Vector3Int(x, y, 0), null);

                    // Destroy the wall objects at this exit so that the AIs don't think that there is still a wall there
                    if (currentCollisionTile != null)
                    {
                        DestroyWallObject(new Vector2Int(x, y));
                    }

                }
            }

        }

        // Create the object that the player will collide with in order to use the exit
        GameObject exitObject = new GameObject("Exit #" + exitPath.id.ToString());
        exitObject.transform.SetParent(_exitsTransform);
        BoxCollider2D exitCollider = exitObject.AddComponent<BoxCollider2D>();

        // Set the locations for these exitObjects depending on the direction of the exit, as well as where the player will come out from
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

        // Each exitObject needs an ExitManager component
        exitObject.AddComponent<ExitManager>();

        // Now, clear the border tilemaps because we need to re-create them since we modified the level by clearing out walls
        _bordersTilemap.ClearAllTiles();

        // If clearing the way for the exit caused some "middle" bricks to become "side" bricks, update them.
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

        // Now, replace the borders
        DungeonGenerator.PlaceBorders(_bordersTilemap, _collisionTilemap);

        // And set the open status of this exit path
        exitPath.SetIsOpen(true);

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
    private bool _isOpen;

    public void SetIsOpen(bool value)
    {
        _isOpen = value;
    }

    public bool isOpen
    {
        get
        {
            return _isOpen;
        }
    }

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