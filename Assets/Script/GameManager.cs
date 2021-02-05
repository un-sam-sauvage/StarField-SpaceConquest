using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private State _currentState;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = new Player1();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_currentState.name);
        _currentState.Process();
    }

    public void NextTurn()
    {
        _currentState.stage = State.Event.EXIT;
    }
}