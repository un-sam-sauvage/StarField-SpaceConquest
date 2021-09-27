using System.Collections.Generic;
using UnityEngine;

public class SpecialBiomasse : SpecialEffectShip
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
            _gm.selectUnit.AddListener(HealUnitSelected);
        }
        else
        {
            Debug.Log("il n'y a pas d'unité à soigner autour");
        }
    }

    //soigne l'unité sélectionnée
    void HealUnitSelected()
    {
        Unit unitSelected = _gm.unitSelected.GetComponent<Unit>();
        unitSelected.life += 2;
        unitSelected.life = Mathf.Clamp(unitSelected.life, 0, unitSelected.thisUnit.life);
        _unit.unitAnimator.SetBool("IsDead", true);
        _gm.ShowUIforUnit(true);
    }
    

    //récupère toutes les unités autour de l'unité de biomasse qui est en train de jouer

}