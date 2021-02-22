using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : State
{
    private GameManager _gm;
    private List<Vector3> _tileToClear = new List<Vector3>();
    private Unit _unit;

    public UnitAttackState(Unit unit)
    {
        _unit = unit;
    }

    public override void Enter()
    {
        _gm = GameManager.instance;
        foreach (var tile in DrawPlayerMovement.GetAdjacentTiles(_unit.GetPos()))
        {
            _gm.tilemap.SetTile(_gm.tilemap.WorldToCell(tile), _gm.attackTile);
            _tileToClear.Add(tile);
        }

        base.Enter();
    }

    bool IsInMyTeam()
    {
        foreach (var unit in _gm.currentPlayerUnits)
        {
            if (_gm.unitSelectedForAttack.GetComponent<Unit>() == unit)
            {
                return true;
            }
        }

        return false;
    }

    public override void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = _gm.tilemap.WorldToCell(mouseWorldPos);

        if (Input.GetMouseButtonDown(0) && _gm.tilemap.GetTile(coordinate) == _gm.attackTile &&
            _gm.tilemap.WorldToCell(_gm.unitSelectedForAttack.transform.position) == coordinate && !IsInMyTeam())
        {
            Debug.Log("j'attaque cette unité");
            Unit unitAttacked = _gm.unitSelectedForAttack.GetComponent<Unit>();
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
    }

    public override void Exit()
    {
        foreach (var tile in _tileToClear)
        {
            _gm.tilemap.SetTile(_gm.tilemap.WorldToCell(tile), null);
        }

        _tileToClear.Clear();
        base.Exit();
    }
}