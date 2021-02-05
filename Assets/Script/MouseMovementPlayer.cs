using DG.Tweening;
using UnityEngine;

public class MouseMovementPlayer : MonoBehaviour
{
    public Grid grid;

    // Update is called once per frame
    void Update()
    {
        //move the player to the cell which was clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
            Vector3 positionPlayer = grid.GetCellCenterLocal(coordinate);
            
            gameObject.transform.DOMove(positionPlayer,.7f);
        }
    }
}
