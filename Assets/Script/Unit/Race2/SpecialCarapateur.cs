using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCarapateur : SpecialEffectShip
{
    private GameManager _gm;
    private Unit _unit;

    public override void Use(Unit unit)
    {
        _gm = GameManager.instance;
        _unit = unit;
        if (UnitAroundThisUnit().Count > 0)
        {
            _gm.ShowUIforUnit(false);
            _gm.ShowUnitSelectable(_unit.GetPos(), UnitAroundThisUnit());
            _gm.selectUnit.AddListener(ShieldUnitSelected);
        }
        else
        {
            Debug.Log("il n'y a pas d'unité à shielder autour");
        }
    }

    void ShieldUnitSelected()
    {
        Unit unitSelected = _gm.unitSelected.GetComponent<Unit>();
        unitSelected.shield ++;
        _unit.unitAnimator.SetBool("IsDead", true);
        _gm.ShowUIforUnit(true);
    }

    List<Unit> UnitAroundThisUnit()
    {
        List<Vector3> tilesAroundUnit = DrawPlayerMovement.GetAdjacentTiles(_unit.GetPos());
        tilesAroundUnit.Add(_unit.GetPos());
        List<Unit> unitAroundThisUnit = new List<Unit>();
        foreach (var playerUnit in _gm.currentPlayerUnits)
        {
            foreach (var tile in tilesAroundUnit)
            {
                if (playerUnit.GetPos() == tile && playerUnit != _unit)
                {
                    unitAroundThisUnit.Add(playerUnit);
                }
            }
        }

        return unitAroundThisUnit;
    }
}