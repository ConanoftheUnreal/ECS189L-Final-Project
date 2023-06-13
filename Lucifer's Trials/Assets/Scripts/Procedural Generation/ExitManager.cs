using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour
{

    private int _id;
    private LevelManager _levelManager;

    void Start()
    {

        // Get the ID for this room as well as the LevelManager for the current level
        _id = int.Parse(this.gameObject.name.Substring(this.gameObject.name.IndexOf("#") + 1));
        _levelManager = GameObject.FindWithTag("Root").GetComponent<LevelManager>();
        
    }   

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (_levelManager == null)
        {
            _levelManager = GameObject.FindWithTag("Root").GetComponent<LevelManager>();
        }
        else{

            // If the player collides with this exit
            if ((collision.gameObject.tag == "Player") && (_levelManager.isCurrentRoomCleared))
            {
                
                // Make sure they are not trying to go out the entrance in the Root room
                if (!((_levelManager.currentNode.room.roomObject.name == "Root") && (_id == LevelGenerator.ENTRANCE_EXIT_ID)))
                {

                    // Also make sure they don't accidentally leave the room they just entered
                    if (_levelManager.justEnteredNewRoom == false)
                    {

                        // Disable the current room
                        _levelManager.currentNode.room.DisableRoom();
                        LevelLayoutNode newNode;
                        Vector2 entranceLocation;

                        // If the player is leaving the room through the entrance
                        if (_id == LevelGenerator.ENTRANCE_EXIT_ID)
                        {

                            // That means they are moving into the room that is the parent of the current room
                            newNode = _levelManager.currentNode.parent;

                            // Get the child ID of the current room in relation to its parent
                            string currentRoomName = _levelManager.currentNode.room.roomObject.name;
                            int entranceID = int.Parse(currentRoomName.Substring(currentRoomName.IndexOf("#") + 1)) + 1;

                            // And get the location of the exit that leads from the parent to the current room
                            entranceLocation = newNode.room.exitPaths[entranceID].entranceLocation;
                            _levelManager.SetRecentlyUsedEntrance(entranceID);

                        }
                        else
                        {

                            if (_levelManager.currentNode.type == NodeType.BOSS)
                            {

                                SceneManager.LoadScene("RunWin");
                                return;

                            }

                            // If the player is entering a child room, just get the node for that child
                            newNode = _levelManager.currentNode.children[_id - 1];

                            // And get the location for the entrance of that child
                            entranceLocation = newNode.room.exitPaths[LevelGenerator.ENTRANCE_EXIT_ID].entranceLocation;
                            _levelManager.SetRecentlyUsedEntrance(LevelGenerator.ENTRANCE_EXIT_ID);

                        }

                        // Change the current room in the LevelManager
                        _levelManager.SetCurrentNode(newNode);
                        _levelManager.currentNode.room.EnableRoom();

                        // And move the player to the currect location for the taken exit/entrance
                        collision.gameObject.transform.position = new Vector3(entranceLocation.x, entranceLocation.y, 0);
                        _levelManager.SetJustEntered(true);

                    }

                }

            }

        }

    }

}
