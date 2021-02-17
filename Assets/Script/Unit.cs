using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [HideInInspector] public int movement;
    [HideInInspector] public int atk;
    [HideInInspector] public int shield;
    [HideInInspector] public int life;

    [HideInInspector] public string unitName;

    public bool hasPlayed;
    public bool hasAttacked;
    public bool hasMoved;

    private State _currentState;

    public ScriptableUnit thisUnit;

    private GameManager _gm;

    private void Awake()
    {
        unitName = thisUnit.name;
        movement = thisUnit.movement;
        atk = thisUnit.atk;
        shield = thisUnit.shield;
        life = thisUnit.life;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public void Move(Vector3 positionToGo)
    {
        gameObject.transform.DOMove(positionToGo, .7f);
    }

    public void SetState(State state)
    {
        //if(_currentState != null) => currentState.Exit
        _currentState?.Exit();

        _currentState = state;

        //if(_currentState != null) => currentState.Enter
        _currentState?.Enter();
    }

    public void OnMouseDown()
    {
        _gm = GameManager.instance;
        foreach (var unit in _gm.currentPlayerUnits)
        {
            if (unit == this)
            {
                _gm.currentUnit = this;
                _gm.ShowCurrentUnitInfos(this);
            }
        }
    }
}