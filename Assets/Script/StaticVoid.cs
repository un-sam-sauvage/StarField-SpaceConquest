using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticVoid : MonoBehaviour
{
    //get a unique sample of all tile in a given range
    public static List<Vector3> GetMovableTile(int range, Vector3 start)
    {
        List<Vector3> tiles = new List<Vector3>();
        tiles.Add(start);
        for (int i = 0; i < range; i++)
        {
            foreach (var tile in tiles)
            {
                tiles = tiles.Union(GetAdjacentTiles(tile)).ToList();
            }
        }

        tiles.Remove(start);
        return tiles;
    }

    public static List<Vector3> GetAdjacentTiles(Vector3 start)
    {
        List<Vector3> tiles = new List<Vector3>();
        tiles.Add(start + Vector3.left);
        tiles.Add(start + Vector3.right);
        tiles.Add(start + new Vector3(-.5f, .75f, 0));
        tiles.Add(start + new Vector3(.5f, .75f, 0));
        tiles.Add(start + new Vector3(.5f, -.75f, 0));
        tiles.Add(start + new Vector3(-.5f, -.75f, 0));
        return tiles;
    }
    public static List<Unit> GetAlliesAroundThisUnit(Unit unit)
    {
        GameManager _gm = GameManager.instance;
        List<Vector3> tilesAroundUnit = GetAdjacentTiles(unit.GetPos());
        tilesAroundUnit.Add(unit.GetPos());
        List<Unit> unitAroundThisUnit = new List<Unit>();
        foreach (var playerUnit in _gm.currentPlayerUnits)
        {
            foreach (var tile in tilesAroundUnit)
            {
                if (playerUnit.GetPos() == tile && playerUnit != unit)
                {
                    unitAroundThisUnit.Add(playerUnit);
                }
            }
        }

        return unitAroundThisUnit;
    }

    public static List<Unit> GetEnemiesAroundThisUnit(Unit unit)
    {
        GameManager _gm = GameManager.instance;
        List<Vector3> tilesAroundUnit = GetAdjacentTiles(unit.GetPos());
        List<Unit> enemiesAround = new List<Unit>();
        foreach (var player in _gm.players)
        {
            PlayerInfos playerInfos = player.GetComponent<PlayerInfos>();
            if (playerInfos != _gm.currentPlayer)
            {
                foreach (var enemisUnit in playerInfos.units)
                {
                    foreach (var tile in tilesAroundUnit)
                    {
                        if (enemisUnit.GetPos() == tile)
                        {
                            enemiesAround.Add(enemisUnit);
                        }
                    }
                }
            }
        }

        return enemiesAround;
    }
}