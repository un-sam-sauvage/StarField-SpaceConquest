using UnityEngine;

public class Ruche : MonoBehaviour
{
    public int nbTurnAlive;
    public void Start()
    {
        GameManager.instance.nextTurn.AddListener(ChangeTurn);
    }

    void ChangeTurn()
    {
        nbTurnAlive++;
        if (nbTurnAlive >= 3)
        {
            Destroy(gameObject);
        }
    }
}
