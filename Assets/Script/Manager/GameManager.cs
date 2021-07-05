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
    public GameObject panelUIUnitAttackable;

    public List<GameObject> players;
    public List<GameObject> nbUnitOnTile;
    
    private List<GameObject> _nbUnitOnCase = new List<GameObject>();
    
    public Tilemap boardTilemap;
    public Tilemap moveTilemap;

    public Tile movementTile;
    public Tile attackTile;

    [HideInInspector] public List<Unit> currentPlayerUnits;

    [HideInInspector] public GameObject unitSelected;

    [HideInInspector] public Unit currentUnit;

    [HideInInspector] public UnityEvent selectUnit;
    [HideInInspector] public UnityEvent nextTurn;

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
        nextTurn.Invoke();
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
            unitSelected.GetComponent<Unit>().Move(initialEnemiUnitPosition);
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
            if (currentUnit.thisUnit.shipTier == ScriptableUnit.Tier.Tier3)
            {
                currentUnit.hasMoved = true;
                //TODO rendre le bouton de l'attack non interactable
            }
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
            if (currentUnit.thisUnit.shipTier == ScriptableUnit.Tier.Tier3)
            {
                currentUnit.hasAttacked = true;
                //TODO rendre le bouton de l'attack non interactable
            }
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

    public void SetUnitToSpecialMode()
    {
        _isInMovementMode = false;
        currentState?.Exit();
        if (currentUnit != null && !currentUnit.hasUsedSpecialEffect)
        {
            currentUnit.gameObject.GetComponent<SpecialEffectShip>().Use(currentUnit);
            currentUnit.hasUsedSpecialEffect = true;
        }
        else
        {
            //TODO rendre le bouton non interctable
            Debug.Log("l'effet spécial de l'unité a déjà été utilisé");
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

    public void ShowUnitSelectable(Vector3 posOfPanel, List<Unit> unitSelectable)
    {
        _unitAttackable = unitSelectable;
        panelUIUnitAttackable.transform.position = posOfPanel + new Vector3(2f, 0, 0);
        panelUIUnitAttackable.SetActive(true);
        for (int i = 0; i < unitSelectable.Count; i++)
        {
            unitsAttackableButton[i].gameObject.SetActive(true);
            unitsAttackableButton[i].GetComponentInChildren<TextMeshProUGUI>().text = unitSelectable[i].unitName;
        }

        for (int i = unitSelectable.Count; i < unitsAttackableButton.Count; i++)
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

    public void UnitIsSelected(int buttonIndex)
    {
        foreach (var unit in _unitAttackable)
        {
            if (unitsAttackableButton[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text == unit.unitName)
            {
                unitSelected = unit.gameObject;
                selectUnit.Invoke();
                return;
            }
        }


        Debug.LogWarning("Aucune unité correspondante");
    }

    public void NbUnitOnTile(int nbUnit, Vector3 posGO)
    {
        GameObject obj = Instantiate(nbUnitOnTile[nbUnit--], posGO - new Vector3(0, .5f, 0), quaternion.identity);
        foreach (var item in _nbUnitOnCase)
        {
            if (item.transform.position == obj.transform.position)
            {
                Debug.Log("je détruis l'item");
                Destroy(item);
            }
        }
        obj.GetComponentInChildren<TextMeshPro>().text = $"{nbUnit+1}";
        _nbUnitOnCase.Add(obj);
    }
}