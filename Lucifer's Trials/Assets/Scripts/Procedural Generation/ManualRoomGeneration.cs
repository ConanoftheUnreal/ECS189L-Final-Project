using UnityEngine;

public class ManualRoomGeneration : MonoBehaviour
{

    [SerializeField] private Camera _camera;

    private DungeonGenerator _roomGenerator;
    private LevelLayoutGenerator _levelLayoutGenerator;
    private Room _currentRoom;
    private LevelGenerator _levelGenerator;

    public void Start()
    {

        _roomGenerator = new DungeonGenerator(5, 5, new Vector2Int(16, 9));
        _levelLayoutGenerator = new LevelLayoutGenerator(3, 9, 3);
        _levelGenerator = new LevelGenerator(_levelLayoutGenerator, _roomGenerator);

    }

    public void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {

            Destroy(GameObject.Find("Root"));
            _currentRoom = _levelGenerator.Generate();

            //_currentRoom = _roomGenerator.Generate(16, 9, Random.Range(1, 4 + 1), new Vector2(0, 0));
            //_currentRoom = _roomGenerator.Generate(16, 9, 11, new Vector2(0, 0));
            //_currentRoom.OpenExits();
            
            CameraController cc = _camera.gameObject.GetComponent<CameraController>();
            cc.SnapToRoom(_currentRoom);

        }

        if (Input.GetButtonDown("Fire2"))
        {

            //_currentRoom.OpenExits();

        }

    }

}
