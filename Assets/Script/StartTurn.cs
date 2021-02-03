using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurn : State
{
    public StartTurn()
    {
        timer = 5;
        name = STATE.StartTurn;
    }

    public override void Enter()
    {
        Debug.Log("Bienvenue dans le village");
        base.Enter();
    }

    public override void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //nextState = new CupidonTurn();
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        Debug.Log("le village s'endort");
        base.Exit();
    }
}