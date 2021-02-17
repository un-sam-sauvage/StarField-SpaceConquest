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

    bool IsInMyTeam()
    {
        foreach (var unit in _gm.currentPlayerUnits)
        {
            if (_gm.unitSelectedForAttack.GetComponent<Unit>() == unit)
            {
                return true;
            }
        }
        return false;
    }
    
    public override void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = _gm.tilemap.WorldToCell(mouseWorldPos);

        if (Input.GetMouseButtonDown(0) && _gm.tilemap.GetTile(coordinate) == _gm.attackTile && _gm.tilemap.WorldToCell(_gm.unitSelectedForAttack.transform.position) == coordinate && !IsInMyTeam())
        {
            Debug.Log("je peux attaquer cette unité");
        }

        base.Update();
    }

    public override void Exit()
    {
        foreach (var tile in _tileToClear)
        {
            _gm.tilemap.SetTile(_gm.tilemap.WorldToCell(tile), null);
        }
        _tileToClear.Clear();
        base.Exit();
    }
}