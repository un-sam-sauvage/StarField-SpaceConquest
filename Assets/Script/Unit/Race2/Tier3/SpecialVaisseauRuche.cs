using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpecialVaisseauRuche : SpecialEffectShip
{
    private GameManager _gm;
    
    private Unit _unit;
    
    private List<Vector3> tilesAround;
    
    private bool _isUsingSpecialEffect;
    
    public GameObject ruche;
    private GameObject _currentRuche;
    public override void Use(Unit unit)
    {
        if (_currentRuche == null)
        {
            _isUsingSpecialEffect = true;
            _gm = GameManager.instance;
            _unit = unit;
            tilesAround = StaticVoid.GetAdjacentTiles(_unit.GetPos());
            tilesAround.Remove(_unit.GetPos());
            _gm.ShowUIforUnit(false);
            foreach (var tile in tilesAround)
            {
                _gm.moveTilemap.SetTile(_gm.moveTilemap.WorldToCell(tile),_gm.movementTile);
            }
        }
        else
        {
            Debug.Log("il y a déjà une ruche");
        }
    }

    void SetRuche(Vector3Int ruchePos)
    {
        _currentRuche = Instantiate(ruche, ruchePos, quaternion.identity);
        _gm.ShowUIforUnit(true);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isUsingSpecialEffect)
        {
            foreach (var tile in tilesAround)
            {
                if (Input.mousePosition == tile)
                {
                   SetRuche(_gm.moveTilemap.WorldToCell(tile));
                   _isUsingSpecialEffect = false;
                }
            }

        }
    }
}
