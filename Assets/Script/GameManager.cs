using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public State currentState;

    public List<GameObject> buttonsUnit;
    public List<GameObject> players;

    public GameObject panelUIUnitAttackable;
    
    public Tilemap tilemap;

    public Tile movementTile;
    public Tile attackTile;

    [HideInInspector] public List<Unit> currentPlayerUnits;

    [HideInInspector] public GameObject unitSelectedForAttack;

    [HideInInspector] public Unit currentUnit;

    [HideInInspector] public UnityEvent selectUnitToAttack;

    [HideInInspector] public Vector3 initialUnitPosition;
    
    private PlayerInfos _currentPlayer;
    
    private int _turn;

    private bool _isInMovementMode;

    [Header("Text UI Infos Unit")]
    public TextMeshProUGUI unitLife;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitMovement;
    public TextMeshProUGUI unitShield;
    public TextMeshProUGUI unitAtk;
    
    public List<TextMeshProUGUI> unitsAttackableText;
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
        currentState?.Exit();
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
        unitName.text = $"name : {unit.unitType}";
        unitShield.text = $"shield : {unit.shield.ToString()}";
    }

    public void SetUnitToAttackMode()
    {
        _isInMovementMode = false;
        currentState?.Exit();
        if (currentUnit != null && !currentUnit.hasAttacked)
        {
            currentUnit.SetState(new UnitAttackState(currentUnit));
            currentState = new UnitAttackState(currentUnit);
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

    public void ShowUnitAttackable(Vector3 posOfPanel, List<Unit> unitAttackable)
    {
        panelUIUnitAttackable.transform.position = posOfPanel;
        panelUIUnitAttackable.SetActive(true);
        for (int i = 0; i < unitAttackable.Count; i++)
        {
            unitsAttackableText[i].text = unitAttackable[i].unitName;
        }
    }

    public void SelectUnitToAttack(GameObject unitToAttack)
    {
        unitSelectedForAttack = unitToAttack;
        selectUnitToAttack.Invoke();
    }
}