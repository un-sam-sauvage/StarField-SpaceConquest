using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public State currentState;

    public List<GameObject> buttonsUnit;
    public List<GameObject> players;

    private int _turn;

    private bool _isInMovementMode;

    [HideInInspector] public Vector3 initialUnitPosition;

    public Tilemap tilemap;

    public Tile movementTile;
    public Tile attackTile;

    private PlayerInfos _currentPlayer;

    [HideInInspector] public List<Unit> currentPlayerUnits;

    [HideInInspector] public GameObject unitSelectedForAttack;

    [HideInInspector] public Unit currentUnit;

    [Header("Text")] public TextMeshProUGUI unitLife;
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
        currentState?.Process();
    }

    public void NextTurn()
    {
        Debug.Log("on passe au tour suivant");
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

    bool AllUnitsOfCurrentPlayerHasPlayed()
    {
        foreach (var unit in currentPlayerUnits)
        {
            if (!unit.hasPlayed)
            {
                return false;
            }
        }

        return true;
    }

    public void NextUnit()
    {
        currentState.Exit();
        currentUnit.hasPlayed = true;
        _isInMovementMode = false;
        currentUnit = null;
        ShowUIforUnit(false);
        if (AllUnitsOfCurrentPlayerHasPlayed())
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
        _isInMovementMode = false;
        currentState.Exit();
        if (currentUnit != null && !currentUnit.hasAttacked)
        {
            currentUnit.SetState(new UnitAttackDistState(currentUnit));
            currentState = new UnitAttackDistState(currentUnit);
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
        _isInMovementMode = true;
        if (currentUnit != null && !currentUnit.hasMoved)
        {
            currentUnit.SetState(new UnitMovementState(currentUnit));
            currentState = new UnitMovementState(currentUnit);
            currentUnit.hasMoved = true;
        }
        else
        {
            //TODO rendre le bouton non interactahble
            Debug.Log("l'unité à déjà bougé");
        }
    }

    public void ResetUnitMovement()
    {
        if (_isInMovementMode)
        {
            currentUnit.Move(initialUnitPosition);
        }
    }

    public void ShowUIforUnit(bool setActive)
    {
        foreach (var element in buttonsUnit)
        {
            element.SetActive(setActive);
        }
    }
}