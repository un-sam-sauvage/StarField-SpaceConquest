using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int range;
    public int atk;
    public int shield;
    public int life;
    private State _currentState;
    
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
}
