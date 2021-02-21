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

    [HideInInspector] public Animator unitAnimator;

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
        Quaternion oldRotation = transform.rotation;
        Sequence mySequence = DOTween.Sequence();
        Vector3 dir = positionToGo - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        mySequence.Append(transform.DORotate(new Vector3(newRotation.x, newRotation.y, newRotation.z), .2f));
        mySequence.Append(transform.DOMove(positionToGo, 1f));
        mySequence.Append(transform.DORotate(new Vector3(oldRotation.x, oldRotation.y, oldRotation.z), .2f).OnComplete(()=> unitAnimator.SetBool("IsMoving", false)));
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
                SetColor(Color.red);
            }
        }
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
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