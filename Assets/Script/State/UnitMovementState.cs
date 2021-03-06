﻿using System.Collections.Generic;
using UnityEngine;


public class UnitMovementState : State
{
    private int _range;
    private Unit _unit;
    private List<Vector3> _tileToClear = new List<Vector3>();
    private GameManager _gm;

    public UnitMovementState(Unit unit)
    {
        _range = unit.movement;
        _unit = unit;
    }

    //=~= void start
    public override void Enter()
    {
        _gm = GameManager.instance;
        _gm.ShowUIforUnit(false);
        foreach (var tile in StaticVoid.GetMovableTile(_range, _unit.GetPos()))
        {
            if (_gm.boardTilemap.GetTile(_gm.boardTilemap.WorldToCell(tile)) != null)
            {
                _gm.moveTilemap.SetTile(_gm.moveTilemap.WorldToCell(tile), _gm.movementTile);
                _tileToClear.Add(tile);
            }
        }

        base.Enter();
    }
    
    int CanGoToTile(Vector3 tile)
    {
        int nbUnitOnTile = 1;
        foreach (var player in _gm.players)
        {
            foreach (var unit in player.GetComponent<PlayerInfos>().units)
            {
                if (unit.GetPos() == tile)
                {
                    nbUnitOnTile++;
                }
            }
        }

        return nbUnitOnTile;
    }

    
//=~= void Update 
    public override void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = _gm.boardTilemap.WorldToCell(mouseWorldPos);
        Vector3 positionPlayer = _gm.boardTilemap.GetCellCenterLocal(coordinate);
        //move the player to the cell which was clicked
        if (Input.GetMouseButtonDown(0) && _gm.moveTilemap.GetTile(coordinate) == _gm.movementTile &&
            CanGoToTile(positionPlayer) <= 4)
        {
            _unit.unitAnimator.SetBool("IsMoving", true);
            _unit.Move(positionPlayer);
            if (CanGoToTile(positionPlayer) > 1)
            {
                _gm.NbUnitOnTile(CanGoToTile(positionPlayer),positionPlayer);
            }
            stage = Event.EXIT;
        }
    }

//sortir du script
    public override void Exit()
    {
        _gm.ShowUIforUnit(true);
        foreach (var tile in _tileToClear)
        {
            _gm.moveTilemap.SetTile(_gm.moveTilemap.WorldToCell(tile), null);
        }

        _tileToClear.Clear();
        base.Exit();
    }
}