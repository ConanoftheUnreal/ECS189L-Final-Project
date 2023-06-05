using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Room
{

    public GameObject roomObject;
    public List<ExitPathRectangle> exitPaths;

    public Room(GameObject roomObject, List<ExitPathRectangle> exitPaths)
    {

        this.roomObject = roomObject;
        this.exitPaths = exitPaths;

    }

    public void OpenExits()
    {

        Tilemap collisionTilemap = roomObject.transform.Find("Collision").GetComponent<Tilemap>();
        Tilemap bordersTilemap = roomObject.transform.Find("Borders").GetComponent<Tilemap>();
        Tilemap decorationsTilemap = roomObject.transform.Find("Decorations").GetComponent<Tilemap>();

        foreach(ExitPathRectangle exitPath in exitPaths)
        {

            if (exitPath.direction != ExitDirection.UP)
            {

                for (int x = exitPath.bottomLeft.x; x <= exitPath.topRight.x; x++)
                {
                    for (int y = exitPath.bottomLeft.y; y <= exitPath.topRight.y; y++)
                    {     

                        collisionTilemap.SetTile(new Vector3Int(x, y, 0), null);
                        bordersTilemap.SetTile(new Vector3Int(x, y, 0), null);
                        decorationsTilemap.SetTile(new Vector3Int(x, y, 0), null);

                    }
                }

            }

        }

        bordersTilemap.ClearAllTiles();
        DungeonGenerator.PlaceBorders(bordersTilemap, collisionTilemap);

    }

}

public enum ExitDirection
{

    UP,
    RIGHT,
    DOWN,
    LEFT

}

public class ExitPathRectangle
{

    public Vector2Int bottomLeft;
    public Vector2Int topRight;
    public ExitDirection direction;    

}