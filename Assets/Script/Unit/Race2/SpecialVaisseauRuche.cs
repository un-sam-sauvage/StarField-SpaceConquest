using System.Collections.Generic;
using UnityEngine;

public class SpecialVaisseauRuche : SpecialEffectShip
{
    private GameManager _gm;
    private Unit _unit;
    private List<Vector3> tilesAround;
    public GameObject ruche;
    public override void Use(Unit unit)
    {
        _gm = GameManager.instance;
        _unit = unit;
        tilesAround = StaticVoid.GetAdjacentTiles(_unit.GetPos());
        tilesAround.Remove(_unit.GetPos());
        _gm.ShowUIforUnit(false);
        //_gm.ShowUnitSelectable(_unit.GetPos(), tilesAround);
        _gm.selectUnit.AddListener(SetRuche);
    }

    void SetRuche()
    {
        //Instantiate(ruche,)   
        _gm.ShowUIforUnit(true);
    }
}
