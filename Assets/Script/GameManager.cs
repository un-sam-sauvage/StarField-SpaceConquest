using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private State _currentState;
    public bool changeTurn;
    
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
        _currentState = new Player1();
    }

    private void Update()
    {
        _currentState.Process();
    }

    public void NextTurn()
    {
        changeTurn = !changeTurn;
    }
}