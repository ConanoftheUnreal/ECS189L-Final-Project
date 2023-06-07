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

        ITileset tileset = DungeonTileset.Instance;

        Tilemap collisionTilemap = roomObject.transform.Find("Collision").GetComponent<Tilemap>();
        Tilemap bordersTilemap = roomObject.transform.Find("Borders").GetComponent<Tilemap>();
        Tilemap decorationsTilemap = roomObject.transform.Find("Decorations").GetComponent<Tilemap>();

        Transform exitsTransform = roomObject.transform.Find("Exits");

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

            GameObject exitObject = new GameObject("Exit #" + exitPath.id.ToString());
            exitObject.transform.SetParent(exitsTransform);
            BoxCollider2D exitCollider = exitObject.AddComponent<BoxCollider2D>();

            if (exitPath.direction == ExitDirection.LEFT)
            {

                Vector3 objectPosition = collisionTilemap.CellToWorld(new Vector3Int(exitPath.bottomLeft.x, exitPath.bottomLeft.y + 1));
                exitObject.transform.position = objectPosition;
                exitCollider.size = new Vector2(0.5f, 2) * roomObject.transform.localScale;
    
            }
            else if (exitPath.direction == ExitDirection.RIGHT)
            {

                Vector3 objectPosition = collisionTilemap.CellToWorld(new Vector3Int(exitPath.topRight.x + 1, exitPath.topRight.y));
                exitObject.transform.position = objectPosition;
                exitCollider.size = new Vector2(0.5f, 2) * roomObject.transform.localScale;                

            }
            else if (exitPath.direction == ExitDirection.UP)
            {

                Vector3 objectPosition = collisionTilemap.CellToWorld(new Vector3Int(exitPath.bottomLeft.x + 1, exitPath.bottomLeft.y + 1));
                exitObject.transform.position = objectPosition;
                exitCollider.size = new Vector2(2, 0.5f) * roomObject.transform.localScale;                

            }
            else if (exitPath.direction == ExitDirection.DOWN)
            {

                Vector3 objectPosition = collisionTilemap.CellToWorld(new Vector3Int(exitPath.bottomLeft.x + 1, exitPath.bottomLeft.y));
                exitObject.transform.position = objectPosition;
                exitCollider.size = new Vector2(2, 0.5f) * roomObject.transform.localScale;                

            }

            exitObject.AddComponent<ExitManager>();

        }

        bordersTilemap.ClearAllTiles();

        // If clearing the way for the exit caused some "middle" bicks to become "side" bricks, update them.
        for (int x = 0; x < collisionTilemap.size.x; x++)
        {

            for (int y = 0; y < collisionTilemap.size.y; y++)
            {

                Tile currentTile = collisionTilemap.GetTile(new Vector3Int(x, y, 0)) as Tile;
                Tile tileToRight = collisionTilemap.GetTile(new Vector3Int(x + 1, y, 0)) as Tile;
                Tile tileToLeft = collisionTilemap.GetTile(new Vector3Int(x - 1, y, 0)) as Tile;
                Tile tileBottomLeft = collisionTilemap.GetTile(new Vector3Int(x - 1, y - 1, 0)) as Tile;
                Tile tileBottomRight = collisionTilemap.GetTile(new Vector3Int(x + 1, y - 1, 0)) as Tile;
                
                if (currentTile != null)
                {

                    if ((tileset.GetNameOfTile(currentTile) == "BottomWall_Right") && (tileToRight == null))
                    {
                        collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BottomWall_Mid"));
                    }
                    else if ((tileset.GetNameOfTile(currentTile) == "BottomWall_Left") && (tileToLeft == null))
                    {
                        collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("BottomWall_Mid"));
                    }
                    else if ((tileset.GetNameOfTile(currentTile) == "TopWall_Right") && (tileToRight == null))
                    {
                        if (tileBottomRight == null)
                        {
                            collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Mid"));
                        }
                    }
                    else if ((tileset.GetNameOfTile(currentTile) == "TopWall_Left") && (tileToLeft == null))
                    {
                        if (tileBottomLeft == null)
                        {
                            collisionTilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTileByName("TopWall_Mid"));
                        }
                    }

                }

            }

        }

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
    public int id;

}