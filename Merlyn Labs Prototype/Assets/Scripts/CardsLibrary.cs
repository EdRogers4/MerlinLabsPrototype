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
                //scriptGameManager.isPlantFeet = true;
                //StartCoroutine(scriptGameManager.GainArmor(scriptGameManager.modifierPlantFeet));
                //StartCoroutine(scriptGameManager.GainVengeance(5, enemyToAttack));
                scriptGameManager.PlayerAttack(5, enemyToAttack);
                break;
            case 4:
                scriptGameManager.PlayerAttack(scriptGameManager.playerArmor, enemyToAttack);
                break;
            case 5:
                scriptGameManager.isHiltPunch = true;
                scriptGameManager.PlayerAttack(scriptGameManager.modifierHiltPunch, enemyToAttack);
                break;
            case 6:
                scriptGameManager.isKnightsResolve = true;
                StartCoroutine(scriptGameManager.GainArmor(6));
                break;
            case 7:
                scriptGameManager.isFlurry = true;
                scriptGameManager.PlayerAttack(6, enemyToAttack);
                break;
            case 8:
                scriptGameManager.counterRoundhouse = 2;
                scriptGameManager.PlayerAttack(7, enemyToAttack);
                break;
            case 9:
                scriptGameManager.isLightningArc = true;
                StartCoroutine(scriptGameManager.LightningArc(enemyToAttack));
                break;

        }
    }
}
