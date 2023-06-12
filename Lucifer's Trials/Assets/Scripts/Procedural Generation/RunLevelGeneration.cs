using UnityEngine;

public class RunRoomGeneration : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _steeringPerceiver;

    private GameObject _player;

    private IRoomGenerator _roomGenerator;
    private LevelLayoutGenerator _layoutGenerator;
    private LevelGenerator _levelGenerator;
    private LevelManager _levelManager;

    private bool _firstFrame = true;

    public void Start()
    {

        _roomGenerator = new DungeonGenerator(5, 5, new Vector2Int(16, 9));
        _layoutGenerator = new LevelLayoutGenerator(5, 15, 3);
        _levelGenerator = new LevelGenerator(_layoutGenerator, _roomGenerator);

        GameObject perceiver = Object.Instantiate(_steeringPerceiver);

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

    }

    public void Update()
    {

        if (_firstFrame)
        {

            _levelManager.currentNode.room.SpawnEnemies();
            _firstFrame = false;

        }        

    }

}