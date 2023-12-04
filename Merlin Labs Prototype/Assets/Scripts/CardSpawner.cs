using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject prefabCard;
    [SerializeField] private Transform spawnPointCard;
    [SerializeField] private CardPositioner scriptCardPositioner;
    [SerializeField] private Canvas canvas;
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
            newCard.GetComponent<Card>().scriptCardPositioner = scriptCardPositioner;
            newCard.GetComponent<Card>().canvas = canvas;
        }

        scriptCardPositioner.isNoCardsHighlighted = true;
    }
}
