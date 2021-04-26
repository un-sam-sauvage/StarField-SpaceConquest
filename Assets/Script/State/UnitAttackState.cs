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
        _gm.ShowUnitAttackable(_unit.GetPos(), GetUnitAttackable());
        base.Enter();
    }

    bool IsInMyTeam(Unit targetUnit)
    {
        foreach (var unitAlly in _gm.currentPlayerUnits)
        {
            if (targetUnit.unitName == unitAlly.unitName)
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
                if (_gm.boardTilemap.WorldToCell(unit.GetPos()) == _gm.boardTilemap.WorldToCell(_unit.GetPos()) &&
                    !IsInMyTeam(unit))
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
            Debug.Log("j'attaque cette unité");
            Unit unitAttacked = _gm.unitSelectedForAttack.GetComponent<Unit>();
            _gm.initialEnemiUnitPosition = _gm.unitSelectedForAttack.GetComponent<Unit>().GetPos();
            _gm.unitSelectedForAttack.GetComponent<Unit>().Move(_unit.GetPos() + Vector3.right);
            _unit.Rotate(_gm.unitSelectedForAttack.transform.position);
            int damageDone = _unit.atk - unitAttacked.shield;
            if (damageDone > 0)
            {
                unitAttacked.life -= damageDone;
                _unit.unitAnimator.SetBool("IsShooting", true);
            }
            else
            {
                Debug.Log("vous n'aviez pas assez d'atk pour infliger des dégâts");
            }

            if (unitAttacked.life > 0)
            {
                int damageReceived = unitAttacked.atk - _unit.shield;
                if (damageReceived > 0)
                {
                    _unit.life -= damageReceived;
                }
                else
                {
                    Debug.Log("l'ennemi n'avait pas assez d'atk pour vous infliger des dégâts");
                    stage = Event.EXIT;
                }

                if (_unit.life <= 0)
                {
                    _unit.unitAnimator.SetBool("IsDead", true);
                    Debug.Log("Votre unité est morte");
                    stage = Event.EXIT;
                }
                else
                {
                    stage = Event.EXIT;
                }
            }
            else
            {
                Debug.Log("l'unité ennemie a été détruite");
                unitAttacked.unitAnimator.SetBool("IsDead", true);
                stage = Event.EXIT;
            }

            _gm.ShowCurrentUnitInfos(_unit);
        }
        else
        {
            Debug.Log("je sors");
            stage = Event.EXIT;
        }
    }

    public override void Exit()
    {
        _gm.panelUIUnitAttackable.SetActive(false);
        base.Exit();
    }
}