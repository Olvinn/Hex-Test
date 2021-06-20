using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/MapData")]
public class MapData : ScriptableObject
{
    [SerializeField] List<TileData> _tiles;

    public int GetTilesCount()
    {
        return _tiles.Count;
    }

    public TileData GetTileData(Tile num)
    {
        foreach (TileData tile in _tiles)
            if (tile.tile == num)
                return tile;
        return null;
    }
    public TileData GetTileData(int num)
    {
        if (num < 0 || num >= _tiles.Count)
            return null;
        return _tiles[num];
    }
}
