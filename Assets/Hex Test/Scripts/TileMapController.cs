using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    [SerializeField] Tilemap _tMap;
    [SerializeField] TileMapGenerator _gen;

    List<Vector3Int> _opened;
    List<Vector3Int> _calculated;

    private void Awake()
    {
        _opened = new List<Vector3Int>();
        _calculated = new List<Vector3Int>();
    }

    private void Start()
    {
        CalcFog();
    }

    public void OpenHex(Vector3Int pos)
    {
        if (_opened.Contains(pos))
            return;

        _opened.Add(pos);
        CalcFog();
    }

    public bool IsNear(Vector3Int one, Vector3Int two)
    {
        foreach (Vector3Int c in NearCells(one))
            if (c == two)
                return true;
        return false;
    }

    public Tile GetCell(Vector3Int pos)
    {
        return _tMap.GetTile(pos) as Tile;
    }

    public Vector3Int GetCellPos(Vector3 pos)
    {
        return _tMap.WorldToCell(pos);
    }

    void CalcFog()
    {
        _calculated.Clear();
        CalcCellColor(Vector3Int.zero);
    }

    void CalcCellColor(Vector3Int cell)
    {
        if (_tMap.GetTile(cell) == null || _calculated.Contains(cell))
            return;

        _tMap.SetTileFlags(cell, TileFlags.None);

        Color c = _tMap.GetColor(cell);
        c.a = 0;

        if (_opened.Contains(cell))
            c.a = 1;
        else
        {
            foreach (Vector3Int pos in NearCells(cell))
                if (_opened.Contains(pos))
                    c.a = .25f;
        }

        _tMap.SetColor(cell, c);
        _calculated.Add(cell);

        foreach (Vector3Int pos in NearCells(cell))
            CalcCellColor(pos);
    }

    Vector3Int[] NearCells(Vector3Int pos)
    {
        bool odd = pos.y % 2 != 0;
        Vector3Int[] res = new Vector3Int[]
        {
            new Vector3Int(pos.x - 1, pos.y, pos.z),
            new Vector3Int(pos.x + 1, pos.y, pos.z),

            new Vector3Int(pos.x, pos.y - 1, pos.z),
            new Vector3Int(pos.x, pos.y + 1, pos.z),

            new Vector3Int(pos.x + (odd ? 1 : -1), pos.y - 1, pos.z),
            new Vector3Int(pos.x + (odd ? 1 : -1), pos.y + 1, pos.z),
        };
        return res;
    }
}
