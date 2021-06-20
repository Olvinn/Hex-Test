using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    [SerializeField] TileMapGenerator _gen;
    [SerializeField] TileMapController _con;
    [SerializeField] UIController ui;

    [SerializeField] MapData _data;

    [SerializeField] uint _seed = 123456;

    Dictionary<Vector2Int, TileData> _buyed;
    float _money = 0;
    float _income = 0;

    private void Awake()
    {
        _buyed = new Dictionary<Vector2Int, TileData>();
    }

    private void Start()
    {
        _seed = (uint)Random.Range(0, 1000000);

        _gen.Init(_data);
        _gen.Create(_seed);

        Tile tile = _con.GetCell(Vector3Int.zero);
        BuyCell(tile, Vector2Int.zero);
        _con.OpenHex(Vector3Int.zero);

        _money += 200;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Open((Vector2Int)_con.GetCellPos(pos));
        }

        ProceedIncome();
        ui.SetInfo((int)_money + " +" + _income);
    }

    void Open(Vector2Int pos)
    {
        if (_buyed.ContainsKey(pos))
            return;
        Tile tile = _con.GetCell((Vector3Int)pos);
        if (CanBuyCell(tile))
            foreach (Vector2Int c in _buyed.Keys)
                if (_con.IsNear((Vector3Int)pos, (Vector3Int)c))
                {
                    BuyCell(tile, pos);
                    _con.OpenHex((Vector3Int)pos);
                    break;
                }
    }

    bool CanBuyCell(Tile tile)
    {
        if (tile == null)
            return false;
        return _data.GetTileData(tile).price <= _money;
    }

    void BuyCell(Tile tile, Vector2Int pos)
    {
        TileData t = _data.GetTileData(tile);
        _buyed.Add(pos, t);
        _income += t.income;
        _money -= t.price;
    }

    void ProceedIncome()
    {
        _money += _income * Time.deltaTime;
    }
}
