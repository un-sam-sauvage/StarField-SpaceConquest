using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnPlanet : MonoBehaviour
{
    public Tilemap instantiatePlanet;
    public List<Tile> planets;
    public List<ZonesToInstantiatesPlanet> zonesToInstantiatePlanets;

    List<Vector3> GetTileForPlanet(Vector3 start, Tilemap tilemap)
    {
        List<Vector3> tiles = new List<Vector3>();
        bool isAnotherTile = true;
        tiles.Add(start);
        while (isAnotherTile)
        {
            foreach (var tile in tiles)
            {
                if (!CheckIfComplete(tile, tiles, tilemap))
                {
                    return tiles;
                }

                tiles = tiles.Union(DrawPlayerMovement.GetAdjacentTiles(tile)).ToList();
            }
        }

        return null;
    }

    bool CheckIfComplete(Vector3 currentTile, List<Vector3> tiles, Tilemap tilemap)
    {
        List<Vector3> checkIfTile = DrawPlayerMovement.GetAdjacentTiles(currentTile);
        List<Vector3> tileToRemove = new List<Vector3>();
        foreach (var tile in tiles)
        {
            foreach (var tileToCheck in checkIfTile)
            {
                if (tile == tileToCheck || tilemap.GetTile(tilemap.WorldToCell(tileToCheck)) != null)
                {
                    tileToRemove.Add(tileToCheck);
                }
            }
        }

        if (tileToRemove.Count == checkIfTile.Count)
        {
            return false;
        }

        return true;
    }

    List<Vector3> RemoveBadPos(List<Vector3> listToClean , Tilemap tilemap)
    {
        List<Vector3> listCleaned = new List<Vector3>();
        foreach (var tile in listToClean)
        {
            if (tilemap.GetTile(tilemap.WorldToCell(tile)) != null)
            {
                Debug.Log("jajoute");
                listCleaned.Add(tile);
            }
        }

        return listCleaned;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var zone in zonesToInstantiatePlanets)
        {
            List<Vector3> whereToInstantiate = GetTileForPlanet(zone.start, zone.zoneToInstantiatePlanet);
            List<Vector3> whereToInstantiateClean = RemoveBadPos(whereToInstantiate, zone.zoneToInstantiatePlanet);
            int index = Random.Range(0, whereToInstantiateClean.Count);
            Debug.Log($"{whereToInstantiateClean.Count} {whereToInstantiate.Count} {zone.zoneToInstantiatePlanet.name}");
            Vector3Int tile = instantiatePlanet.WorldToCell(whereToInstantiateClean[index]);
            instantiatePlanet.SetTile(tile, planets[Random.Range(0, planets.Count)]);
        }
    }
}

[System.Serializable]
public class ZonesToInstantiatesPlanet
{
    public Tilemap zoneToInstantiatePlanet;
    public Vector3 start;
}