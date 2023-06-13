using UnityEngine;

public class RunRoomGeneration : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _steeringPerceiver;

    private IRoomGenerator _roomGenerator;
    private LevelLayoutGenerator _layoutGenerator;
    private LevelGenerator _levelGenerator;
    private LevelManager _levelManager;

    private int _frameNumber = 0;

    private const int MIN_MAX_DEPTH = 3;
    private const int MAX_ROOMS_MULTIPLIER = 3;

    public void Start()
    {
        
        _roomGenerator = new DungeonGenerator(5, 5, new Vector2Int(16, 9));
        //Increase the size of the levels as the player completes levels
        int maxDepth = PlayerStatsContainer.Instance.levelsFinished + MIN_MAX_DEPTH;
        int maxRooms = maxDepth * MAX_ROOMS_MULTIPLIER;
        _layoutGenerator = new LevelLayoutGenerator(maxDepth, maxRooms, 3);
        _levelGenerator = new LevelGenerator(_layoutGenerator, _roomGenerator);

        GameObject perceiver = Object.Instantiate(_steeringPerceiver);

        GameObject wallEvn = perceiver.transform.Find("WallEnv").gameObject;
        Polarith.AI.Package.EnvironmentUpdater environmentUpdater = wallEvn.GetComponent<Polarith.AI.Package.EnvironmentUpdater>();
        environmentUpdater.GameObjectCollections[0] = this.gameObject;

        GameObject enemiesEnv = perceiver.transform.Find("EnemiesEnv").gameObject;
        environmentUpdater = enemiesEnv.GetComponent<Polarith.AI.Package.EnvironmentUpdater>();
        environmentUpdater.GameObjectCollections[0] = GameObject.Find("Enemies");
    
    }

    public void Update()
    {

        if (_frameNumber == 0)
        {
            
            _levelManager = _levelGenerator.Generate().roomObject.GetComponent<LevelManager>();
            Vector2 entranceLocation = _levelManager.currentNode.room.exitPaths[LevelGenerator.ENTRANCE_EXIT_ID].entranceLocation;
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = new Vector3(entranceLocation.x, entranceLocation.y, 0);
            player.transform.SetParent(_levelManager.currentNode.room.roomObject.transform);
            player.GetComponent<SpriteRenderer>().enabled = true;

            CameraController cc = _camera.gameObject.GetComponent<CameraController>();
            cc.SnapToRoom(_levelManager.currentNode.room);
            
        }
        else if (_frameNumber == 1)
        {
            _levelManager.currentNode.room.SpawnEnemies();
        }  

        _frameNumber++;

    }

}