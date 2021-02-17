using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private State _currentState;

    public List<GameObject> players;

    private int _turn;

    public Tilemap tilemap;

    public Tile movementTile;
    public Tile attackTile;

    private PlayerInfos _currentPlayer;

    public List<Unit> currentPlayerUnits;

    [HideInInspector] public Unit currentUnit;

    public TextMeshProUGUI unitLife;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitMovement;
    public TextMeshProUGUI unitShield;
    public TextMeshProUGUI unitAtk;

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
        currentPlayerUnits = _currentPlayer.units;
    }

    private void Update()
    {
        _currentState?.Process();
    }

    public void NextTurn()
    {
        if (_turn >= players.Count - 1)
        {
            _turn = 0;
        }
        else
        {
            _turn++;
        }

        //TODO réinitialisez les variables de l'unités (hasMoved , hasAttacked, hasPlayed)
        _currentPlayer = players[_turn].GetComponent<PlayerInfos>();
        currentPlayerUnits = _currentPlayer.units;
    }

    public void NextUnit()
    {
        bool allUnisOfCurrentPlayerHasPlayed;
        currentUnit.hasPlayed = true;
        if (!allUnisOfCurrentPlayerHasPlayed)
        {
            NextTurn();
        }
    }

    public void ShowCurrentUnitInfos(Unit unit)
    {
        unitAtk.text = $"atk : {unit.atk.ToString()}";
        unitLife.text = $"life : {unit.life.ToString()}";
        unitMovement.text = $"movement : {unit.movement.ToString()}";
        unitName.text = $"name : {unit.unitName}";
        unitShield.text = $"shield : {unit.shield.ToString()}";
    }

    public void SetUnitToAttackMode()
    {
        if (!currentUnit.hasAttacked)
        {
            currentUnit.SetState(new UnitAttackState(currentUnit));
            _currentState = new UnitAttackState(currentUnit);
            currentUnit.hasAttacked = true;
        }
        else
        {
            //TODO rendre le bouton non interactahble
            Debug.Log("l'unité à déjà attaqué");
        }
    }

    public void SetUnitToMovementMode()
    {
        if (!currentUnit.hasMoved)
        {
            currentUnit.SetState(new UnitMovementState(currentUnit));
            _currentState = new UnitMovementState(currentUnit);
            currentUnit.hasMoved = true;
        }
        else
        {
            //TODO rendre le bouton non interactahble
            Debug.Log("l'unité à déjà bougé");
        }
    }
}