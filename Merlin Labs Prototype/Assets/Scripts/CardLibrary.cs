using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public int[] energyCost;
    public string[] cardName;
    public string[] cardDescription;
    [SerializeField] private GameManager scriptGameManager;

    public void PlayCard(int cardIndex)
    {
        switch (cardIndex)
        {
            case 0:
                scriptGameManager.PlayerAttack(5);
                break;
        }
    }
}
