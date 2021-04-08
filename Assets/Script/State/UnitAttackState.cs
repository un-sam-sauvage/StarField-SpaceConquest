using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : State
{
    private GameManager _gm;
    private Unit _unit;

    public UnitAttackState(Unit unit)
    {
        _unit = unit;
    }

    public override void Enter()
    {
        _gm = GameManager.instance;
        _gm.selectUnitToAttack.AddListener(AttackUnit);
        _gm.ShowUnitAttackable(_unit.GetPos() + new Vector3(2f, 0, 0), GetUnitAttackable());
        base.Enter();
    }

    bool IsInMyTeam()
    {
        foreach (var unit in _gm.currentPlayerUnits)
        {
            if (_gm.tilemap.WorldToCell(unit.GetPos()) == _gm.tilemap.WorldToCell(_unit.GetPos()))
            {
                return true;
            }
        }

        return false;
    }

    List<Unit> GetUnitAttackable()
    {
        List<Unit> unitAttackable = new List<Unit>();
        foreach (var player in _gm.players)
        {
            foreach (var unit in player.GetComponent<PlayerInfos>().units)
            {
                if (_gm.tilemap.WorldToCell(unit.GetPos()) == _gm.tilemap.WorldToCell(_unit.GetPos()) && unit != _unit)
                {
                    unitAttackable.Add(unit);
                }
            }
        }

        return unitAttackable;
    }

    void AttackUnit()
    {
        if (GetUnitAttackable().Count > 0)
        {
            //_gm.unitSelectedForAttack;
            //TODO faire le système d'attaque classique avec les prises de dégâts et la mort.
        }
        else
        {
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}