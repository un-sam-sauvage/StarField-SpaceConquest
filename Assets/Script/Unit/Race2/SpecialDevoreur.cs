using UnityEngine;

public class SpecialDevoreur : SpecialEffectShip
{
    private GameManager _gm;
    private Unit _unit;
    public override void Use(Unit unit)
    {
        _gm = GameManager.instance;
        _unit = unit;
        if (StaticVoid.GetEnemiesAroundThisUnit(_unit).Count > 0)
        {
            _gm.ShowUIforUnit(false);
            _gm.ShowUnitSelectable(_unit.GetPos(), StaticVoid.GetEnemiesAroundThisUnit(_unit));
            _gm.selectUnit.AddListener(AttractUnit);
        }
        else
        {
            Debug.Log("il n'y a pas d'unité à attirer autour");
        }
    }

    void AttractUnit()
    {
        Unit unitSelected = _gm.unitSelected.GetComponent<Unit>();
        unitSelected.Move(_unit.GetPos());
        int damageDone = _unit.atk - unitSelected.shield;
        if (damageDone > 0)
        {
            unitSelected.life -= damageDone;
            if (unitSelected.life <= 0)
            {
                unitSelected.unitAnimator.SetBool("IsDead", true);
                _gm.ShowUIforUnit(true);
            }
        }
    }
}
