using UnityEngine;

public class ManualRoomGeneration : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _SteeringPerceiver;

    private DungeonGenerator _roomGenerator;
    private LevelLayoutGenerator _levelLayoutGenerator;
    private LevelGenerator _levelGenerator;
    
    private LevelManager _levelManager;

    private bool firstFrame = true;

    public void Start()
    {

        _roomGenerator = new DungeonGenerator(5, 5, new Vector2Int(16, 9));
        _levelLayoutGenerator = new LevelLayoutGenerator(5, 15, 3);
        _levelGenerator = new LevelGenerator(_levelLayoutGenerator, _roomGenerator);

        GameObject perceiver = Object.Instantiate(_SteeringPerceiver);

        GameObject wallEvn = perceiver.transform.Find("WallEnv").gameObject;
        Polarith.AI.Package.EnvironmentUpdater environmentUpdater = wallEvn.GetComponent<Polarith.AI.Package.EnvironmentUpdater>();
        environmentUpdater.GameObjectCollections[0] = this.gameObject;

        GameObject enemiesEnv = perceiver.transform.Find("EnemiesEnv").gameObject;
        environmentUpdater = enemiesEnv.GetComponent<Polarith.AI.Package.EnvironmentUpdater>();
        environmentUpdater.GameObjectCollections[0] = GameObject.Find("Enemies");

        _levelManager = _levelGenerator.Generate().roomObject.GetComponent<LevelManager>();
        
        CameraController cc = _camera.gameObject.GetComponent<CameraController>();
        cc.SnapToRoom(_levelManager.currentNode.room);

        Vector2 entranceLocation = _levelManager.currentNode.room.exitPaths[LevelGenerator.ENTRANCE_EXIT_ID].entranceLocation;
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(entranceLocation.x, entranceLocation.y, 0);
        player.transform.SetParent(_levelManager.currentNode.room.roomObject.transform);
        player.GetComponent<SpriteRenderer>().enabled = true;

    }

    public void Update()
    {   

        // I have NO idea why, but the Root rooms enemies need to be spawned in the update loop or their AI does not work properly
        if (firstFrame)
        {

            _levelManager.currentNode.room.SpawnEnemies();
            firstFrame = false;

        }

    }

}
