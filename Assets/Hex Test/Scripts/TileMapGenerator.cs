using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGenerator : MonoBehaviour
{
    [SerializeField] Tilemap _tMap;

    [SerializeField] float randomSize = 1;
    [SerializeField] Vector3 boundsSize = Vector3.one * 10;

    MapData _data;

    public void Init(MapData data)
    {
        _data = data;
    }

    public void Create(uint seed)
    {
        Clear();

        Vector3 pos = Camera.main.transform.position;
        CreateLand(seed, randomSize, pos);
    }

    void Clear()
    {
        _tMap.ClearAllTiles();
    }

    void CreateLand(uint seed, float randomSize, Vector3 pos)
    {
        pos.z = _tMap.transform.position.z;
        Vector3Int centerTile = _tMap.WorldToCell(pos);
        Bounds bounds = new Bounds(pos, boundsSize);
        bounds.center = pos;

        CreateTile(seed, randomSize, bounds, centerTile);
    }

    void CreateTile(uint seed, float randomSize, Bounds bounds, Vector3Int tile)
    {
        if (_tMap.GetTile(tile) != null)
            return;

        Vector3 pos = _tMap.CellToWorld(tile);
        if (!bounds.Contains(_tMap.CellToWorld(tile)))
            return;

        float res = Mathf.PerlinNoise(pos.x * randomSize + seed, pos.y * randomSize + seed);
        int num = (int)((_data.GetTilesCount() + 1) * res);
        _tMap.SetTile(tile, _data.GetTileData(num).tile);

        CreateTile(seed, randomSize, bounds, new Vector3Int(tile.x, tile.y + 1, tile.z));
        CreateTile(seed, randomSize, bounds, new Vector3Int(tile.x, tile.y - 1, tile.z));

        CreateTile(seed, randomSize, bounds, new Vector3Int(tile.x + 1, tile.y, tile.z));
        CreateTile(seed, randomSize, bounds, new Vector3Int(tile.x - 1, tile.y, tile.z));
    }
}
