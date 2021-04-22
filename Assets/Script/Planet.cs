using UnityEngine;

public class Planet : MonoBehaviour
{
    public PlayerInfos currentOwner; 
        
    public string planetName;
    
    public int popRessources;
    public int commonOreRessources;
    public int rareOreRessources;

    public int defense;

    public bool isConquered;
    
    public Vector3 GetPos()
    {
        return transform.position;
    }
}
