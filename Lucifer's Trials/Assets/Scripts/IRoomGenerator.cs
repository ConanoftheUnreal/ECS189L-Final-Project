using UnityEngine;

public interface IRoomGenerator
{

    public GameObject Generate(int width, int height, int numExits, Vector2 location);

}