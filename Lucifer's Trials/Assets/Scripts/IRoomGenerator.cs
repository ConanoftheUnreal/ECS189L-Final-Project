using UnityEngine;

public interface IRoomGenerator
{

    public GameObject Generate(int width, int height, Vector2 location);

}