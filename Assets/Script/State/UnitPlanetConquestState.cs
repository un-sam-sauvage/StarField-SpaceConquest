using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlanetConquestState : State
{
    private Unit _unit;
    private GameManager _gm;
    
    public UnitPlanetConquestState(Unit unit)
    {
        _unit = unit;
    }

    public override void Enter()
    {
        _gm = GameManager.instance;
        base.Enter();
    }

    Planet GetPlanetConquerable()
    {
        foreach (var planet in _gm.planets)
        {
            if (planet.GetPos() == _unit.GetPos())
            {
                return planet;
            }
        }

        return null;
    }

    public override void Update()
    {
        if (GetPlanetConquerable() !=null)
        {
            Debug.Log("il y a une planète");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}