using UnityEngine;

public class SpecialBaveur : SpecialEffectShip
{
    private GameManager _gm;
    private Unit _unit;

    public override void Use(Unit unit)
    {
        _unit = unit;
        _gm = GameManager.instance;
        if (StaticVoid.GetEnemiesAroundThisUnit(_unit).Count > 0 && !_unit.hasAttacked && !_unit.hasMoved)
        {
            _gm.ShowUIforUnit(false);
            _gm.ShowUnitSelectable(_unit.GetPos(), StaticVoid.GetEnemiesAroundThisUnit(_unit));
            _gm.selectUnit.AddListener(DownOneShield);
        }
        else
        {
            Debug.Log("il n'y a pas d'unité autour");
        }
    }

    private void DownOneShield()
    {
        Unit unitSelected = _gm.unitSelected.GetComponent<Unit>();
        unitSelected.shield -= 1;
        _gm.ShowUIforUnit(true);
        _unit.hasAttacked = true;
        _unit.hasMoved = true;
    }
}