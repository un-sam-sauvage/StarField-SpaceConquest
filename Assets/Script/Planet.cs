using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public string planetName;
    
    public int popRessources;
    public int commonOreRessources;
    public int rareOreRessources;


    public Vector3 GetPos()
    {
        return transform.position;
    }
}
