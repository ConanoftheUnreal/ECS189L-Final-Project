using UnityEngine.Tilemaps;

public interface ITileset
{

    public Tile GetTileByName(string tileName);
    public string GetNameOfTile(Tile tile);

}
