using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovementState : State
{
    private int _range;
    private PlayerInfos _playerInfos;
    private List<Vector3> _tileToClear  =new List<Vector3>();
    private GameManager _gm;

    public PlayerMovementState(int range, PlayerInfos playerInfos)
    {
        name = STATE.PlayerMovementState;
        _range = range;
        _playerInfos = playerInfos;
    }

    //=~= void start
    public override void Enter()
    {
        
        _gm = GameManager.instance;
        foreach (var tile in DrawPlayerMovement.GetMovableTile(_range, _playerInfos.GetPos()))
        {
            _gm.tilemap.SetTile(_gm.tilemap.WorldToCell(tile), _gm.movementTile);
            _tileToClear.Add(tile);
        }

        base.Enter();
    }

//=~= void Update 
    public override void Update()
    {
       //Debug.Log("dans l'update de player 2");
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = _gm.tilemap.WorldToCell(mouseWorldPos);
        Vector3 positionPlayer = _gm.tilemap.GetCellCenterLocal(coordinate);
        //move the player to the cell which was clicked
        if (Input.GetMouseButtonDown(0) && _gm.tilemap.GetTile(coordinate) == _gm.movementTile)
        {
            _playerInfos.Move(positionPlayer);
            stage = Event.EXIT;
            foreach (var tile in _tileToClear)
            {
                _gm.tilemap.SetTile(_gm.tilemap.WorldToCell(tile),null);
            }
        }
        else if (Input.GetMouseButtonDown(0) && _gm.tilemap.GetTile(coordinate) == _gm.movementTile)
        {
            Debug.Log(_gm.tilemap.GetTile(coordinate));
        }
    }

//sortir du script
    public override void Exit()
    {
        _tileToClear.Clear();
        base.Exit();
    }
}