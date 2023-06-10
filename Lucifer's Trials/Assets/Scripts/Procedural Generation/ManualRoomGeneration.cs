using UnityEngine;

public class ManualRoomGeneration : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _playerPrefab;

    private DungeonGenerator _roomGenerator;
    private LevelLayoutGenerator _levelLayoutGenerator;
    private LevelGenerator _levelGenerator;
    
    private LevelManager _levelManager;

    public void Start()
    {

        _roomGenerator = new DungeonGenerator(5, 5, new Vector2Int(16, 9));
        _levelLayoutGenerator = new LevelLayoutGenerator(5, 15, 3);
        _levelGenerator = new LevelGenerator(_levelLayoutGenerator, _roomGenerator);

    }

    public void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {

            if (GameObject.Find("Root") != null)
            {
                Destroy(GameObject.Find("Root"));
            }
            else
            {

                _levelManager = _levelGenerator.Generate().roomObject.GetComponent<LevelManager>();
                
                CameraController cc = _camera.gameObject.GetComponent<CameraController>();
                cc.SnapToRoom(_levelManager.currentNode.room);

                Vector2 entranceLocation = _levelManager.currentNode.room.exitPaths[LevelGenerator.ENTRANCE_EXIT_ID].entranceLocation;
                GameObject player = Instantiate(_playerPrefab, new Vector3(entranceLocation.x, entranceLocation.y, 0), Quaternion.identity);
                player.transform.SetParent(_levelManager.currentNode.room.roomObject.transform);

            }

        }

        if (Input.GetButtonDown("Fire2"))
        {

            _levelManager.currentNode.room.OpenAllExits();

        }

    }

}
