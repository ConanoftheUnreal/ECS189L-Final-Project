using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

// Singletone pattern implementation for the class which stores the Dungeon tileset, that way it only needs to be loaded once
public sealed class DungeonTileset : ITileset
{

    // _tiles maps a filename to a tile object
    private Dictionary<string, Tile> _tiles = new Dictionary<string, Tile>();
    // _nameToTile maps a descriptive name to a tile object
    private Dictionary<string, Tile> _nameToTile = new Dictionary<string, Tile>();
    // _tileToName maps a tile object to a descriptive name
    private Dictionary<Tile, string> _tileToName = new Dictionary<Tile, string>();

    private static readonly DungeonTileset instance = new DungeonTileset();
    static DungeonTileset() {}
    private DungeonTileset() {}

    public static DungeonTileset Instance
    {
        get
        {
            return instance;
        }
    }

    public Tile GetTileByName(string tileName)
    {
        return _nameToTile[tileName];
    }

    public string GetNameOfTile(Tile tile)
    {
        return _tileToName[tile];
    }
    
    // Load the tiles at the start of the game
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void LoadTiles()
    {

        // Load all tiles from tilset
        Tile[] allTiles = Resources.LoadAll<Tile>("Tiles/Dungeon_Tiles");
        foreach(Tile tile in allTiles)
        {
            instance._tiles.Add(tile.name, tile);
        }

        // Pair some descriptive names with tiles that are going to be used during generation
        instance.PairNameAndTile("Ground", instance.GetTileByFilename("Dungeon_Tileset_71"));
        instance.PairNameAndTile("Black", instance.GetTileByFilename("Dungeon_Tileset_0"));

        instance.PairNameAndTile("BottomWall_Mid", instance.GetTileByFilename("Dungeon_Tileset_15"));
        instance.PairNameAndTile("BottomWall_Left", instance.GetTileByFilename("Dungeon_Tileset_14"));
        instance.PairNameAndTile("BottomWall_Right", instance.GetTileByFilename("Dungeon_Tileset_16"));

        instance.PairNameAndTile("TopWall_Mid", instance.GetTileByFilename("Dungeon_Tileset_2"));
        instance.PairNameAndTile("TopWall_Left", instance.GetTileByFilename("Dungeon_Tileset_1"));
        instance.PairNameAndTile("TopWall_Right", instance.GetTileByFilename("Dungeon_Tileset_3"));

        instance.PairNameAndTile("BlackBorder_Top", instance.GetTileByFilename("Dungeon_Tileset_43"));
        instance.PairNameAndTile("BlackBorder_Bottom", instance.GetTileByFilename("Dungeon_Tileset_110"));
        instance.PairNameAndTile("BlackBorder_Right", instance.GetTileByFilename("Dungeon_Tileset_26"));
        instance.PairNameAndTile("BlackBorder_Left", instance.GetTileByFilename("Dungeon_Tileset_27"));
        instance.PairNameAndTile("BlackBorder_BottomLeft", instance.GetTileByFilename("Dungeon_Tileset_42"));
        instance.PairNameAndTile("BlackBorder_BottomRight", instance.GetTileByFilename("Dungeon_Tileset_44"));

        instance.PairNameAndTile("WallBorder_Right", instance.GetTileByFilename("Dungeon_Tileset_93"));
        instance.PairNameAndTile("WallBorder_Left", instance.GetTileByFilename("Dungeon_Tileset_91"));

        instance.PairNameAndTile("Carpet_Center", instance.GetTileByFilename("Dungeon_Tileset_95"));

        instance.PairNameAndTile("BigDoor_TopLeft", instance.GetTileByFilename("Dungeon_Tileset_53"));
        instance.PairNameAndTile("BigDoor_TopRight", instance.GetTileByFilename("Dungeon_Tileset_54"));
        instance.PairNameAndTile("BigDoor_MiddleLeft", instance.GetTileByFilename("Dungeon_Tileset_69"));
        instance.PairNameAndTile("BigDoor_MiddleRight", instance.GetTileByFilename("Dungeon_Tileset_70"));
        instance.PairNameAndTile("BigDoor_BottomLeft", instance.GetTileByFilename("Dungeon_Tileset_85"));
        instance.PairNameAndTile("BigDoor_BottomRight", instance.GetTileByFilename("Dungeon_Tileset_86"));

        instance.PairNameAndTile("Exit_Left", instance.GetTileByFilename("Dungeon_Tileset_112"));
        instance.PairNameAndTile("Exit_Right", instance.GetTileByFilename("Dungeon_Tileset_118"));
        instance.PairNameAndTile("Exit_Up", instance.GetTileByFilename("Dungeon_Tileset_116"));
        instance.PairNameAndTile("Exit_Down", instance.GetTileByFilename("Dungeon_Tileset_117"));

        instance.PairNameAndTile("BlackBorderCorner_TopLeft", instance.GetTileByFilename("Dungeon_Tileset_59"));
        instance.PairNameAndTile("BlackBorderCorner_TopRight", instance.GetTileByFilename("Dungeon_Tileset_61"));
        instance.PairNameAndTile("BlackBorderCorner_BottomLeft", instance.GetTileByFilename("Dungeon_Tileset_103"));
        instance.PairNameAndTile("BlackBorderCorner_BottomRight", instance.GetTileByFilename("Dungeon_Tileset_104"));

        instance.PairNameAndTile("RectangleVent", instance.GetTileByFilename("Dungeon_Tileset_36"));

        instance.PairNameAndTile("CircleVent_Top", instance.GetTileByFilename("Dungeon_Tileset_35"));
        instance.PairNameAndTile("CircleVent_Bottom", instance.GetTileByFilename("Dungeon_Tileset_52"));

        instance.PairNameAndTile("WallBanner_Top", instance.GetTileByFilename("Dungeon_Tileset_33"));
        instance.PairNameAndTile("WallBanner_Bottom", instance.GetTileByFilename("Dungeon_Tileset_50"));

        instance.PairNameAndTile("SingleWallTorch_Top", instance.GetTileByFilename("Dungeon_Tileset_9"));
        instance.PairNameAndTile("SingleWallTorch_Bottom", instance.GetTileByFilename("Dungeon_Tileset_22"));

        instance.PairNameAndTile("DoubleWallTorch_Top", instance.GetTileByFilename("Dungeon_Tileset_11"));
        instance.PairNameAndTile("DoubleWallTorch_Bottom", instance.GetTileByFilename("Dungeon_Tileset_24"));

    }

    // Map a descriptive name to a tile object and vice versa
    private void PairNameAndTile(string name, Tile tile)
    {

        _nameToTile.Add(name, tile);
        _tileToName.Add(tile, name);

    }

    private Tile GetTileByFilename(string fileName)
    {
        return _tiles[fileName];
    }

}
