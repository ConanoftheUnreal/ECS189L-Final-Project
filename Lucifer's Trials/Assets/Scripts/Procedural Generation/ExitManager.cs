using UnityEngine;

public class ExitManager : MonoBehaviour
{

    private int _id;
    private LevelManager _levelManager;

    void Start()
    {

        _id = int.Parse(this.gameObject.name.Substring(this.gameObject.name.IndexOf("#") + 1));
        _levelManager = GameObject.Find("Root").GetComponent<LevelManager>();
    }   

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            
            if (!((_levelManager.currentNode.room.roomObject.name == "Root") && _id == (LevelGenerator.ENTRANCE_EXIT_ID)))
            {

                if (_levelManager.justEnteredNewRoom == false)
                {

                    _levelManager.currentNode.room.DisableRoom();
                    LevelLayoutNode newNode;
                    Vector2 entranceLocation;

                    if (_id == LevelGenerator.ENTRANCE_EXIT_ID)
                    {

                        newNode = _levelManager.currentNode.parent;

                        string currentRoomName = _levelManager.currentNode.room.roomObject.name;
                        int entranceID = int.Parse(currentRoomName.Substring(currentRoomName.IndexOf("#") + 1)) + 1;

                        entranceLocation = newNode.room.exitPaths[entranceID].entranceLocation;
                        _levelManager.SetRecentlyUsedEntrance(entranceID);

                    }
                    else
                    {

                        newNode = _levelManager.currentNode.children[_id - 1];

                        entranceLocation = newNode.room.exitPaths[LevelGenerator.ENTRANCE_EXIT_ID].entranceLocation;
                        _levelManager.SetRecentlyUsedEntrance(LevelGenerator.ENTRANCE_EXIT_ID);

                    }

                    _levelManager.SetCurrentNode(newNode);
                    _levelManager.currentNode.room.EnableRoom();

                    collision.gameObject.transform.position = new Vector3(entranceLocation.x, entranceLocation.y, 0);
                    _levelManager.SetJustEntered(true);

                }

            }

        }

    }

}
