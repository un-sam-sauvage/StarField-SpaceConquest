using UnityEngine;

[CreateAssetMenu(fileName = "NameOfUnit", menuName = "ScriptableObjects/Unit")]
public class ScriptableUnit : ScriptableObject
{
    public enum Tier
    {
        Tier1,
        Tier2,
        Tier3,
    }

    [Header("Stats")]
    public string type;
    public int atk;
    public int life;
    public int shield;
    public int movement;

    public Tier shipTier;

    [Header("Cost")] 
    public int popCost;
    public int ressourcesCost;
    public int rareRessourcesCost;
    
    [Header("Animator")]
    public RuntimeAnimatorController animator;
    
}