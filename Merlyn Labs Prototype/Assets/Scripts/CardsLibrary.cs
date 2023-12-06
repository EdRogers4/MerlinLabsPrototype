using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsLibrary : MonoBehaviour
{
    public int[] energyCost;
    public string[] cardName;
    public string[] cardDescription;
    public bool[] isXCost;
    public bool[] isTargetEnemyCard;
    [SerializeField] private GameManager scriptGameManager;

    public void PlayCard(int cardIndex, int enemyToAttack)
    {
        switch (cardIndex)
        {
            case 0:
                scriptGameManager.PlayerAttack(5, enemyToAttack);
                break;
            case 1:
                StartCoroutine(scriptGameManager.GainArmor(5));
                break;
            case 2:
                StartCoroutine(scriptGameManager.GainVengeance(5, enemyToAttack));
                break;
            case 3:
                scriptGameManager.isPlantFeet = true;
                StartCoroutine(scriptGameManager.GainArmor(scriptGameManager.modifierPlantFeet));
                break;
        }
    }
}
