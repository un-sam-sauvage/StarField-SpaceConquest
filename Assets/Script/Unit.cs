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
    [HideInInspector] public int attackRange;

    [HideInInspector] public string unitName;

    [HideInInspector] public Animator unitAnimator;

    [HideInInspector]public bool hasPlayed;
    [HideInInspector]public bool hasAttacked;
    [HideInInspector]public bool hasMoved;
    
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
        attackRange = thisUnit.attackRange;
        unitAnimator = GetComponent<Animator>();
        unitAnimator.runtimeAnimatorController = thisUnit.animator;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public void Move(Vector3 positionToGo)
    {
        Debug.Log("je lance l'animation");
        Rotate(positionToGo);
        transform.DOMove(positionToGo, 1f).OnComplete(()=> unitAnimator.SetBool("IsMoving", false));
        
    }

    public void Rotate(Vector3 positionToLookAt)
    {
        Vector3 dir = positionToLookAt - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.DORotate(new Vector3(0,0,Quaternion.AngleAxis(angle-90, Vector3.forward).z*100), .35f);
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
        _gm.unitSelectedForAttack = gameObject;
        foreach (var unit in _gm.currentPlayerUnits)
        {
            if (unit == this && _gm.currentUnit == null && !hasPlayed)
            {
                _gm.currentUnit = this;
                _gm.ShowCurrentUnitInfos(this);
                _gm.initialUnitPosition = GetPos();
                _gm.ShowUIforUnit(true);
            }
        }
    }

    public void TriggerDeadByAnimation()
    {
        gameObject.SetActive(false);
        Debug.Log("l'unité a été détruite");
    }

    public void TriggerEndShooting()
    {
        unitAnimator.SetBool("IsShooting", false);
    }
}