using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tile")]
public class TileData : ScriptableObject
{
    public Tile tile;
    public float price;
    public float income;
}
