using UnityEngine;

public class DeathManager : MonoBehaviour
{

    private LevelManager _levelManager;

    void OnDestroy()
    {

        // If this object is an enemy
        if (this.gameObject.GetComponent<GoblinBeserker>() != null)
        {

            if (_levelManager != null)
            {

                // Remove it from the current rooms list of enemies when it dies
                _levelManager.currentNode.room.RemoveEnemy(this.gameObject);

                // And then check if all the enemies are dead, so that you can open all the exits in the room
                if (_levelManager.isCurrentRoomCleared)
                {
                    _levelManager.currentNode.room.OpenAllExits();
                }

            }

        }

    }

    void Update()
    {

        if (_levelManager == null)
        {
            _levelManager = GameObject.FindWithTag("Root").GetComponent<LevelManager>();
        }

    }

}