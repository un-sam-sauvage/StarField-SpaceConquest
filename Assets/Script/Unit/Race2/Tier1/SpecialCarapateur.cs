using UnityEngine;

public class SpecialCarapateur : SpecialEffectShip
{
    private GameManager _gm;
    private Unit _unit;

    public override void Use(Unit unit)
    {
        _gm = GameManager.instance;
        _unit = unit;
        if (StaticVoid.GetAlliesAroundThisUnit(_unit).Count > 0)
        {
            _gm.ShowUIforUnit(false);
            _gm.ShowUnitSelectable(_unit.GetPos(), StaticVoid.GetAlliesAroundThisUnit(_unit));
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

  
}