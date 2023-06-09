using UnityEngine;

public class LevelManager : MonoBehaviour
{

    // Level Manager keeps track of the current room and the exits that the player uses
    private LevelLayoutNode _currentNode;
    private bool _justEnteredNewRoom = false;
    private GameObject _player;
    private int _recentlyUsedEntrance = 0;

    private float EXIT_REENTER_DISTANCE = 1.0f;

    public bool justEnteredNewRoom
    {
        get
        {
            return _justEnteredNewRoom;
        }
    }

    public void SetJustEntered(bool value)
    {
        _justEnteredNewRoom = value;
    }

    public void SetRecentlyUsedEntrance(int entranceID)
    {
        _recentlyUsedEntrance = entranceID;
    }

    public void SetCurrentNode(LevelLayoutNode node)
    {

        _currentNode = node;

        // Add the collision object for the current room to the AI sensor system
        GameObject perceiver = GameObject.FindWithTag("SteeringPerciever").transform.Find("WallEnv").gameObject;
        Polarith.AI.Package.EnvironmentUpdater environmentUpdater = perceiver.GetComponent<Polarith.AI.Package.EnvironmentUpdater>();
        environmentUpdater.GameObjectCollections[0] = _currentNode.room.collisionObject;

    }

    public Vector2 GetCenterOfCurrentRoom()
    {
        return _currentNode.room.centerPoint;
    }

    public LevelLayoutNode currentNode
    {
        get
        {
            return _currentNode;
        }
    }

    public bool isCurrentRoomCleared
    {
        get
        {
            return (_currentNode.room.numAliveEnemies == 0);
        }
    }

    public void Update()
    {

        // Get the player object
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {

            // Don't allow the player to leave through the same door they just entered from without stepping away first
            Vector2 entrancePosition = _currentNode.room.exitPaths[_recentlyUsedEntrance].entranceLocation;
            if (Distance(_player.transform.position, entrancePosition) > EXIT_REENTER_DISTANCE)
            {
                _justEnteredNewRoom = false;
            }

        }

    }

    public static float Distance(Vector2 v1, Vector2 v2)
    {
        return Mathf.Abs((v1 - v2).magnitude);
    }

}
