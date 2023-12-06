using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject prefabCard;
    [SerializeField] private Transform spawnPointCard;
    [SerializeField] private GameManager scriptGameManager;
    [SerializeField] private CardPositioner scriptCardPositioner;
    [SerializeField] private CardsLibrary scriptCardsLibrary;
    [SerializeField] private Canvas canvas;
    [SerializeField] private int[] cardsToSpawn;
    [SerializeField] private List<int> listDeck;
    private GameObject newCard;

    private void Start()
    {
        SpawnCards();
    }

    public void SpawnCards()
    {
        if (listDeck.Count <= 0)
        {
            for (int i = 0; i < 10; i++)
            {
                listDeck.Add(i);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            var newCardIndex = listDeck[(Random.Range(0, listDeck.Count - 1))];
            cardsToSpawn[i] = newCardIndex;
            listDeck.Remove(newCardIndex);
        }

        for (int i = 0; i < 5; i++)
        {
            newCard = Instantiate(prefabCard, spawnPointCard.position, spawnPointCard.rotation);
            newCard.transform.SetParent(canvas.transform);
            scriptCardPositioner.listCardTransform.Add(newCard.transform);
            scriptCardPositioner.AssignCurrentCardPositions(5);
            var thisCard = newCard.GetComponent<Card>();
            thisCard.cardIndex = cardsToSpawn[i];
            thisCard.scriptGameManager = scriptGameManager;
            thisCard.scriptCardPositioner = scriptCardPositioner;
            thisCard.scriptCardsLibrary = scriptCardsLibrary;
            thisCard.canvas = canvas;

            if (scriptCardsLibrary.isXCost[thisCard.cardIndex])
            {
                thisCard.textEnergyCost.text = "X";
            }
            else
            {
                thisCard.textEnergyCost.text = "" + scriptCardsLibrary.energyCost[cardsToSpawn[i]];
            }

            thisCard.textCardName.text = scriptCardsLibrary.cardName[cardsToSpawn[i]];
            thisCard.textCardDescription.text = scriptCardsLibrary.cardDescription[cardsToSpawn[i]];
        }

        scriptCardPositioner.isNoCardsHighlighted = true;
    }
}
