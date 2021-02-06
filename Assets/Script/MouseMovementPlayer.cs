using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseMovementPlayer : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tileToMove;
    public int range;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = tilemap.WorldToCell(mouseWorldPos);
        Vector3 positionPlayer = tilemap.GetCellCenterLocal(coordinate);
        //move the player to the cell which was clicked
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.transform.DOMove(positionPlayer,.7f);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var tile in DrawPlayerMovement.GetMovableTile(range,tilemap.WorldToCell(gameObject.transform.position)))
            {
                tilemap.SetTile(tilemap.WorldToCell(tile),tileToMove);
            }
        }
    }
}
