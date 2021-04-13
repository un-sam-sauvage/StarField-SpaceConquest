using UnityEngine;

[CreateAssetMenu(fileName = "NameOfUnit",menuName = "ScriptableObjects/Unit")]
public class ScriptableUnit : ScriptableObject
{
    [Header("Stats")]
    public string type;
    public int life;
    public int atk;
    public int movement;
    public int shield;
    public int attackRange;
    [Header("Cost")]
    public int popCost;
    public int ressourcesCost;
    //public bool canHeal;
    [Header("Animator")]
    public RuntimeAnimatorController animator;
}
