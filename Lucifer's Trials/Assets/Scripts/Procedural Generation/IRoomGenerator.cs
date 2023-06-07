using UnityEngine;

public interface IRoomGenerator
{

    public Room Generate(int width, int height, int numExits, Vector2 location);

}