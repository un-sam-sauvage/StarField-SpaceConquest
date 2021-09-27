using System.Collections.Generic;
using UnityEngine;

public class Ruche : MonoBehaviour
{
    public int nbTurnAlive;
    private GameManager _gm;
    public void Start()
    {
        _gm = GameManager.instance;
        _gm.nextTurn.AddListener(ChangeTurn);
    }

    void ChangeTurn()
    {
        if (EnemiToDamage().Count > 0)
        {
            foreach (var unit in EnemiToDamage())
            {
                unit.life -= 2;
                if (unit.life <=0)
                {
                    unit.unitAnimator.SetBool("IsDead", true);
                }
            }
        }
        nbTurnAlive++;
        if (nbTurnAlive >= 3)
        {
            Destroy(gameObject);
        }
    }

    List<Unit> EnemiToDamage()
    {
        List<Unit> enemis = new List<Unit>();
        foreach (var player in _gm.players)
        {
            if (player != _gm.currentPlayer)
            {
                foreach (var unit in player.GetComponent<PlayerInfos>().units)
                {
                    if (unit.GetPos() == gameObject.transform.position)
                    {
                        enemis.Add(unit);
                    }
                }
            }
        }
        return enemis;
    }
}
