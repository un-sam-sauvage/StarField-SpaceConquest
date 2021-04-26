using UnityEngine;

public class UnitPlanetConquestState : State
{
    private Unit _unit;
    private GameManager _gm;
    private Planet _planetConquerable;

    public UnitPlanetConquestState(Unit unit)
    {
        _unit = unit;
    }

    public override void Enter()
    {
        _gm = GameManager.instance;
        _planetConquerable = GetPlanetConquerable();
        if (PlanetIsOwnByCurrentPlayer() || GetPlanetConquerable() == null)
        {
            Debug.Log("La planète appartient déjà au joueur");
            stage = Event.EXIT;
        }
        else
        {
            PlanetConquest();
        }
        base.Enter();
    }

    Planet GetPlanetConquerable()
    {
        foreach (var planet in _gm.planets)
        {
            if (planet.GetPos() == _unit.GetPos())
            {
                return planet;
            }
        }

        return null;
    }

    bool PlanetIsOwnByCurrentPlayer()
    {
        foreach (var planet in _gm.currentPlayer.planetOwnByPlayer)
        {
            if (planet.GetPos() == _planetConquerable.GetPos())
            {
                return true;
            }
        }

        return false;
    }

    void PlanetConquest()
    {
        if (_planetConquerable.isConquered)
        {
            if (_planetConquerable.defense > 0)
            {
                int damage = _planetConquerable.defense - _unit.atk;
                if (damage > 0)
                {
                    _planetConquerable.currentOwner.planetOwnByPlayer.Remove(_planetConquerable);
                    _gm.currentPlayer.planetOwnByPlayer.Add(_planetConquerable);
                    stage = Event.EXIT;
                }
                else if (damage < 0)
                {
                    _unit.life -= damage - _unit.shield;
                    Debug.Log("la planète avait trop de défense");
                    stage = Event.EXIT;
                }
                else
                {
                    Debug.Log("vous n'aviez pas assez d'attaque pour conquérir cette planète");
                    stage = Event.EXIT;
                }
            }
        }

        _gm.currentPlayer.planetOwnByPlayer.Add(_planetConquerable);
        stage = Event.EXIT;
    }
}