using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    private Camera _camera;
    private RoomGenerator _roomGenerator;

    public void Start()
    {

        _camera = this.gameObject.GetComponent<Camera>();
        _roomGenerator = new RoomGenerator(5, 5);

    }

    public void SnapToTilemap(Tilemap map)
    {
        
        Vector3 mapPos = map.transform.position;
        Vector2 mapSize = new Vector2((float)(map.size.x), (float)(map.size.y));
        Vector3 camPos = this.gameObject.transform.position;

        _camera.orthographicSize = mapSize.y / 2;
        _camera.transform.position = new Vector3(mapPos.x + (mapSize.x / 2), mapPos.y + (mapSize.y / 2), camPos.z);

    }

    public void Update()
    {

        if (Input.GetButtonDown("Jump"))
        {

            Destroy(GameObject.Find("Room"));

            GameObject room = _roomGenerator.Generate(16, 9, new Vector2(0, 0));
            SnapToTilemap(room.transform.Find("Collision").GetComponent<Tilemap>());

        }

    }

}
