using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

    [Header("Text UI Infos Unit")] public TextMeshProUGUI unitLife;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitMovement;
    public TextMeshProUGUI unitShield;
    public TextMeshProUGUI unitAtk;

    public List<Button> unitsAttackableButton;

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
            currentState = currentUnit.GetState();
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
        if (currentUnit != null && !currentUnit.hasMoved)
        {
            _isInMovementMode = true;
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

    private List<Unit> _unitAttackable;

    public void ShowUnitAttackable(Vector3 posOfPanel, List<Unit> unitAttackable)
    {
        _unitAttackable = unitAttackable;
        panelUIUnitAttackable.transform.position = posOfPanel;
        panelUIUnitAttackable.SetActive(true);
        for (int i = 0; i < unitAttackable.Count; i++)
        {
            unitsAttackableButton[i].GetComponentInChildren<TextMeshProUGUI>().text = unitAttackable[i].unitName;
        }

        for (int i = unitAttackable.Count; i < unitsAttackableButton.Count; i++)
        {
            unitsAttackableButton[i].gameObject.SetActive(false);
        }

        int nbActiveButton = 0;
        foreach (var button in unitsAttackableButton)
        {
            if (button.IsActive())
            {
                nbActiveButton++;
            }
        }
        if (nbActiveButton == 0)
        {
            unitsAttackableButton[0].gameObject.SetActive(true);
            unitsAttackableButton[0].GetComponentInChildren<TextMeshProUGUI>().text = "no Unit attackable";
        }
        else
        {
            var padding = panelUIUnitAttackable.GetComponent<RectMask2D>().padding;
            padding.y = 185 - 58 * (nbActiveButton-1);
            panelUIUnitAttackable.GetComponent<RectMask2D>().padding = padding;
        }

    }

    public void SelectUnitToAttack(int buttonIndex)
    {
        foreach (var unit in _unitAttackable)
        {
            if (unitsAttackableButton[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text == unit.unitName)
            {
                unitSelectedForAttack = unit.gameObject;
                selectUnitToAttack.Invoke();
                return;
            }
        }

        Debug.LogWarning("Aucune unité correspondante");
    }
}