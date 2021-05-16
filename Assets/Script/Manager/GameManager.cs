using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public State currentState;

    public GameObject buttonsUnit;
    public List<GameObject> players;
    public List<GameObject> nbUnitOnTile;
    public GameObject panelUIUnitAttackable;

    public Tilemap boardTilemap;
    public Tilemap moveTilemap;

    public Tile movementTile;
    public Tile attackTile;

    [HideInInspector] public List<Unit> currentPlayerUnits;

    [HideInInspector] public GameObject unitSelectedForAttack;

    [HideInInspector] public Unit currentUnit;

    [HideInInspector] public UnityEvent selectUnitToAttack;

    [HideInInspector] public Vector3 initialUnitPosition;
    [HideInInspector] public Vector3 initialEnemiUnitPosition;
    [HideInInspector] public List<Planet> planets;

    public PlayerInfos currentPlayer;

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
        currentPlayer = players[_turn].GetComponent<PlayerInfos>();
        currentPlayerUnits = currentPlayer.units;
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

        foreach (var unit in currentPlayerUnits)
        {
            unit.hasAttacked = false;
            unit.hasMoved = false;
            unit.hasPlayed = false;
        }

        currentPlayer = players[_turn].GetComponent<PlayerInfos>();
        currentPlayerUnits = currentPlayer.units;
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
        if (currentUnit.GetComponent<Unit>().hasAttacked)
        {
            unitSelectedForAttack.GetComponent<Unit>().Move(initialEnemiUnitPosition);
        }

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

    public void SetUnitToConquestMode()
    {
        _isInMovementMode = false;
        currentState?.Exit();
        if (currentUnit != null && !currentUnit.hasAttacked)
        {
            currentUnit.SetState(new UnitPlanetConquestState(currentUnit));
            currentState = currentUnit.GetState();
            currentUnit.hasAttacked = true;
        }
        else
        {
            //TODO rendre le bouton non interactable
            Debug.Log("l'unité a déjà attaqué");
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
        buttonsUnit.SetActive(setActive);
    }

    private List<Unit> _unitAttackable;

    public void ShowUnitAttackable(Vector3 posOfPanel, List<Unit> unitAttackable)
    {
        _unitAttackable = unitAttackable;
        panelUIUnitAttackable.transform.position = posOfPanel + new Vector3(2f, 0, 0);
        panelUIUnitAttackable.SetActive(true);
        for (int i = 0; i < unitAttackable.Count; i++)
        {
            unitsAttackableButton[i].gameObject.SetActive(true);
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
            var padding = panelUIUnitAttackable.GetComponent<RectMask2D>().padding;
            padding.y = 210;
            panelUIUnitAttackable.GetComponent<RectMask2D>().padding = padding;
        }
        else
        {
            var padding = panelUIUnitAttackable.GetComponent<RectMask2D>().padding;
            padding.y = 210 - 58 * (nbActiveButton - 1);
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

    public void NbUnitOnTile(int nbUnit, Vector3 posGO)
    {
        Instantiate(nbUnitOnTile[nbUnit--], posGO - new Vector3(0, .5f, 0), quaternion.identity);
    }
}