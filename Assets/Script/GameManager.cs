using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private State _currentState;
    public List<GameObject> players;
    public bool changeTurn;
    private int _turn;
    public Tilemap tilemap;
    public Tile movementTile;

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
        PlayerInfos player = players[_turn].GetComponent<PlayerInfos>();
        player.SetState(new PlayerMovementState(player.range, player));
        _currentState = new PlayerMovementState(player.range, player);
    }

    private void Update()
    {
        _currentState.Process();
    }

    public void NextTurn()
    {
        PlayerInfos player = players[_turn].GetComponent<PlayerInfos>();
        player.SetState(new PlayerMovementState(player.range, player));
    }
}