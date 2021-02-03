using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : State
{
    public Player1()
    {
        
    }

    //=~= void start
    public override void Enter()
    {
        base.Enter();
    }
//=~= void Update 
    public override void Update()
    {
        //base.Update();
        stage = Event.EXIT;
    }
//sortir du script
    public override void Exit()
    {
        base.Exit();
    }
}
