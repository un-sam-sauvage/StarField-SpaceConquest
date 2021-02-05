﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : State
{
    public Player1()
    {
        name = STATE.Player1;
    }

    //=~= void start
    public override void Enter()
    {
        Debug.Log("début du tour du joueur 1");
        base.Enter();
    }
//=~= void Update 
    public override void Update()
    {
        nextState = new Player2();
        Debug.Log("dans l'update de player 1");
        base.Update();
    }
//sortir du script
    public override void Exit()
    {
        Debug.Log("fin du tour du joueur 1");
        base.Exit();
        nextState.Process();
    }
}