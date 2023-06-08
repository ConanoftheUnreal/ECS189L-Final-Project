using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private LevelLayoutNode _currentNode;
    private bool _justEnteredNewRoom = false;
    private GameObject _player;
    private int _recentlyUsedEntrance = 0;

    private int EXIT_REENTER_DISTANCE = 1;

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
    }

    public LevelLayoutNode currentNode
    {
        get
        {
            return _currentNode;
        }
    }

    public void Update()
    {

        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {

            Vector2 entrancePosition = _currentNode.room.exitPaths[_recentlyUsedEntrance].entranceLocation;
            if (Distance(_player.transform.position, entrancePosition) > EXIT_REENTER_DISTANCE)
            {
                _justEnteredNewRoom = false;
            }

        }

    }

    private float Distance(Vector2 v1, Vector2 v2)
    {
        return Mathf.Abs(-(v1 - v2).magnitude);
    }

}
