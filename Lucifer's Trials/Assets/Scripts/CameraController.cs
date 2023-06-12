using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    private Camera _camera;

    public void Start()
    {

        _camera = this.gameObject.GetComponent<Camera>();

    }

    public void SnapToRoom(Room room)
    {
        
        Vector3 roomScale = room.roomObject.transform.localScale;
        Vector3 roomPos = room.roomObject.transform.position;
        Tilemap collisionMap = room.roomObject.transform.Find("Collision").GetComponent<Tilemap>();
        Vector2 roomSize = new Vector2((float)(collisionMap.size.x), (float)(collisionMap.size.y)) * new Vector2(roomScale.x, roomScale.y);
        Vector3 camPos = this.gameObject.transform.position;
    
        _camera.orthographicSize = roomSize.y / 2;
        _camera.transform.position = new Vector3(roomPos.x + (roomSize.x / 2), roomPos.y + (roomSize.y / 2), camPos.z);

    }

}
