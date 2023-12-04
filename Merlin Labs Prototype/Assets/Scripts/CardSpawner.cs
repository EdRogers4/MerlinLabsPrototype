using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject prefabCard;
    [SerializeField] private Transform spawnPointCard;
    [SerializeField] private GameManager scriptGameManager;
    [SerializeField] private CardPositioner scriptCardPositioner;
    [SerializeField] private CardLibrary scriptCardLibrary;
    [SerializeField] private Canvas canvas;
    [SerializeField] private int[] cardsToSpawn;
    private GameObject newCard;

    private void Start()
    {
        SpawnCards();
    }

    public void SpawnCards()
    {
        for (int i = 0; i < 5; i++)
        {
            newCard = Instantiate(prefabCard, spawnPointCard.position, spawnPointCard.rotation);
            newCard.transform.SetParent(canvas.transform);
            scriptCardPositioner.listCardTransform.Add(newCard.transform);
            scriptCardPositioner.AssignCurrentCardPositions(5);
            var thisCard = newCard.GetComponent<Card>();
            thisCard.scriptGameManager = scriptGameManager;
            thisCard.scriptCardPositioner = scriptCardPositioner;
            thisCard.scriptCardLibrary = scriptCardLibrary;
            thisCard.canvas = canvas;
            thisCard.textEnergyCost.text = "" + scriptCardLibrary.energyCost[cardsToSpawn[i]];
            thisCard.textCardName.text = scriptCardLibrary.cardName[cardsToSpawn[i]];
            thisCard.textCardDescription.text = scriptCardLibrary.cardDescription[cardsToSpawn[i]];
        }

        scriptCardPositioner.isNoCardsHighlighted = true;
    }
}
