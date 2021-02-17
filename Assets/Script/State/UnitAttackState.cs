using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : State
{
    private GameManager _gm;
    private List<Vector3> _tileToClear = new List<Vector3>();
    private Unit _unit;

    public UnitAttackState(Unit unit)
    {
        _unit = unit;
    }

    public override void Enter()
    {
        _gm = GameManager.instance;
        foreach (var tile in DrawPlayerMovement.GetAdjacentTiles(_unit.GetPos()))
        {
            _gm.tilemap.SetTile(_gm.tilemap.WorldToCell(tile), _gm.attackTile);
            _tileToClear.Add(tile);
        }

        base.Enter();
    }

    public override void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = _gm.tilemap.WorldToCell(mouseWorldPos);
        Vector3 positionPlayer = _gm.tilemap.GetCellCenterLocal(coordinate);
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}