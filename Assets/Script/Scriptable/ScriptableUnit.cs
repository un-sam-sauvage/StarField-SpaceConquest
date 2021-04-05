using UnityEngine;

[CreateAssetMenu(fileName = "NameOfUnit",menuName = "ScriptableObjects/Unit")]
public class ScriptableUnit : ScriptableObject
{
    public string name;
    public int life;
    public int atk;
    public int movement;
    public int shield;
    public int attackRange;
    public bool canHeal;
    public RuntimeAnimatorController animator;
}
