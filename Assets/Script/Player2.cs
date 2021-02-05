using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : State
{
    public Player2()
    {
        name = STATE.Player2;
    }

    //=~= void start
    public override void Enter()
    {
        Debug.Log("début du tour du joueur 2");
        base.Enter();
    }
//=~= void Update 
    public override void Update()
    {
        nextState = new Player1();
        Debug.Log("dans l'update de player 2");
        if (!GameManager.instance.changeTurn)
        {
            stage = Event.EXIT;
        }
    }
//sortir du script
    public override void Exit()
    {
        Debug.Log("fin du tour du joueur 2");
        nextState.Process();
        base.Exit();
    }
}