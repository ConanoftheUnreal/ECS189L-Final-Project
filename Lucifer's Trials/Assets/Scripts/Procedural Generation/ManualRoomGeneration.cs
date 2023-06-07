using UnityEngine;
using UnityEngine.Tilemaps;

public class ManualRoomGeneration : MonoBehaviour
{

    [SerializeField] private Camera _camera;

    private DungeonGenerator _roomGenerator;
    private LevelLayoutGenerator _levelLayerGenerator;
    private Room _currentRoom;

    public void Start()
    {

        _roomGenerator = new DungeonGenerator(5, 5);
        _levelLayerGenerator = new LevelLayoutGenerator(5, 15, 3);

    }

    public void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {

            Destroy(GameObject.Find("Room"));
            _currentRoom = _roomGenerator.Generate(16, 9, Random.Range(1, 4 + 1), new Vector2(0, 0));
            //_currentRoom = _roomGenerator.Generate(16, 9, 11, new Vector2(0, 0));
            _currentRoom.OpenExits();
            
            CameraController cc = _camera.gameObject.GetComponent<CameraController>();
            cc.SnapToTilemap(_currentRoom.roomObject.transform.Find("Collision").GetComponent<Tilemap>());

        }

        if (Input.GetButtonDown("Fire2"))
        {

            _currentRoom.OpenExits();

        }

    }

}
