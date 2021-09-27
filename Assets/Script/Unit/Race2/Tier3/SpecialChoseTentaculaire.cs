using UnityEngine;

public class SpecialChoseTentaculaire : SpecialEffectShip
{
    private Unit _unit;
    public override void Use(Unit unit)
    {
        _unit = unit;
        if (StaticVoid.GetEnemiesAroundThisUnit(_unit).Count > 0)
        {
            foreach (var enemiesUnit in StaticVoid.GetEnemiesAroundThisUnit(_unit))
            {
                enemiesUnit.movement--;
                if (enemiesUnit.movement < 0)
                {
                    enemiesUnit.movement = 0;
                }
            }
        }
        else
        {
            Debug.Log("il n'y a pas d'unité ennemies autour");
        }
    }
}