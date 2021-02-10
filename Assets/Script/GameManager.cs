using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private State _currentState;
    
    public List<GameObject> players;
    
    private int _turn;
    private int _unit;
    
    public Tilemap tilemap;
    
    public Tile movementTile;

    private PlayerInfos _currentPlayer;

    private List<Unit> _currentPlayerUnits;
    private Unit _currentUnit;
    #region singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _currentPlayer = players[_turn].GetComponent<PlayerInfos>();
        _currentPlayerUnits = _currentPlayer.units;
        _currentUnit = _currentPlayerUnits[_unit];
        _currentUnit.SetState(new PlayerMovementState(_currentUnit.range,_currentUnit));
        _currentState = new PlayerMovementState(_currentUnit.range, _currentUnit);
    }

    private void Update()
    {
        _currentState.Process();
        Debug.Log(_turn);
    }

    public void NextTurn()
    {
        if (_turn >= players.Count-1)
        {
            _turn = 0;
        }
        else
        {
            _turn++;
        }

        _currentPlayer = players[_turn].GetComponent<PlayerInfos>();
        _currentPlayerUnits = _currentPlayer.units;
        //_currentPlayer.SetState(new PlayerMovementState(_currentPlayer.range, _currentPlayer));
        //_currentState = new PlayerMovementState(_currentPlayer.range, _currentPlayer);
    }

    public void NextUnit()
    {
        if (_unit >= _currentPlayer.units.Count-1)
        {
            _unit = 0;
            NextTurn();
        }
        else
        {
            _unit++;
        }
        _currentPlayerUnits = _currentPlayer.units;
        _currentUnit = _currentPlayerUnits[_unit];
        _currentUnit.SetState(new PlayerMovementState(_currentUnit.range,_currentUnit));
        _currentState = new PlayerMovementState(_currentUnit.range, _currentUnit);
    }
}