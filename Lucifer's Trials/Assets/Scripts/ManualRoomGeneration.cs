using UnityEngine;
using UnityEngine.Tilemaps;

public class ManualRoomGeneration : MonoBehaviour
{

    [SerializeField] private Camera _camera;

    private DungeonGenerator _roomGenerator;
    private LevelLayoutGenerator _levelLayerGenerator;

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
            GameObject room = _roomGenerator.Generate(16, 9, new Vector2(0, 0));
            
            CameraController cc = _camera.gameObject.GetComponent<CameraController>();
            cc.SnapToTilemap(room.transform.Find("Collision").GetComponent<Tilemap>());

        }

        if (Input.GetButtonDown("Fire1"))
        {

            Destroy(GameObject.Find("Root"));
            GameObject root = _levelLayerGenerator.Generate();

        }

    }

}
