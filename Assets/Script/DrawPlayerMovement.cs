using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawPlayerMovement : MonoBehaviour
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
}