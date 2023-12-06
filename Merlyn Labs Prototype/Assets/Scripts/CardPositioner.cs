using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPositioner : MonoBehaviour
{
    public Card currentCard;
    public bool isCardDrag;
    public bool isNoCardsHighlighted;
    public int currentCardHighlighted;
    public int enemyTargeted;
    public List<Transform> listCardTransform;
    [SerializeField] private GameObject[] beneathCardAreas;
    [SerializeField] private List<Transform> listPositionsDefault;
    [SerializeField] private List<Transform> listPositionsHighlighted;
    [SerializeField] private Transform[] positionDefault5;
    [SerializeField] private Transform[] positionDefault4;
    [SerializeField] private Transform[] positionDefault3;
    [SerializeField] private Transform[] positionDefault2;
    [SerializeField] private Transform[] positionDefault1;
    [SerializeField] private Transform[] positionHighlighted5;
    [SerializeField] private Transform[] positionHighlighted4;
    [SerializeField] private Transform[] positionHighlighted3;
    [SerializeField] private Transform[] positionHighlighted2;
    [SerializeField] private Transform[] positionHighlighted1;
    [SerializeField] private float speedMoveHighlighted;
    [SerializeField] private float speedRotateHighlighted;
    [SerializeField] private Transform transformCurrentCard;
    [SerializeField] private Transform transformCurrentTarget;
    [SerializeField] private Image[] enemyHighlight;
    [SerializeField] private Sprite spriteEnemyTargeted;

    void Start()
    {
    }

    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if ((Input.GetMouseButtonUp(0)) && isCardDrag)
        {
            isCardDrag = false;
            
            if (worldPosition.x > -7.3f && worldPosition.x < 7.8f && worldPosition.y > -0.37 && worldPosition.y < 2.7f)
            {
                currentCard.ReleaseCard();
            }
        }

        if (currentCardHighlighted > 0)
        {
            float step = speedMoveHighlighted * Time.deltaTime;

            if (!isCardDrag && transformCurrentCard != null && transformCurrentTarget != null)
            {
                transformCurrentCard.position = Vector2.MoveTowards(transformCurrentCard.position, transformCurrentTarget.position, step);
                transformCurrentCard.rotation = Quaternion.Slerp(transformCurrentCard.rotation, transformCurrentTarget.rotation, speedRotateHighlighted * Time.deltaTime);
            }

            for (int i = 0; i < listCardTransform.Count; i++)
            {
                if (i != currentCardHighlighted -1)
                {
                    if (i < currentCardHighlighted - 1)
                    {
                        Vector3 newPositionLeft = new Vector3(listPositionsDefault[i].position.x - 50f, listPositionsDefault[i].position.y, listPositionsDefault[i].position.z);
                        listCardTransform[i].position = Vector2.MoveTowards(listCardTransform[i].position, newPositionLeft, step);
                        listCardTransform[i].rotation = Quaternion.Slerp(listCardTransform[i].rotation, listPositionsDefault[i].rotation, speedRotateHighlighted * Time.deltaTime);
                    }
                    else if (i > currentCardHighlighted - 1)
                    {
                        Vector3 newPositionRight = new Vector3(listPositionsDefault[i].position.x + 50f, listPositionsDefault[i].position.y, listPositionsDefault[i].position.z);
                        listCardTransform[i].position = Vector2.MoveTowards(listCardTransform[i].position, newPositionRight, step);
                        listCardTransform[i].rotation = Quaternion.Slerp(listCardTransform[i].rotation, listPositionsDefault[i].rotation, speedRotateHighlighted * Time.deltaTime);
                    }
                }
            }
        }
        else if (isNoCardsHighlighted)
        {
            for (int i = 0; i < listCardTransform.Count; i++)
            {
                float step = speedMoveHighlighted * Time.deltaTime;
                listCardTransform[i].position = Vector2.MoveTowards(listCardTransform[i].position, listPositionsDefault[i].position, step);
                listCardTransform[i].rotation = Quaternion.Slerp(listCardTransform[i].rotation, listPositionsDefault[i].rotation, speedRotateHighlighted * Time.deltaTime);
            }
        }

        if (worldPosition.x >= 3.8f && worldPosition.x <= 8.6f && worldPosition.y >= -2.3f && worldPosition.y <= 4.7f)
        {
            if (enemyTargeted != 1)
            {
                enemyHighlight[0].sprite = spriteEnemyTargeted;
                enemyTargeted = 1;
            }
        }
        else
        {
            if (spriteEnemyTargeted != null)
            {
                enemyHighlight[0].sprite = null;
            }

            if (enemyTargeted != 2)
            {
                enemyTargeted = 0;
            }
        }


        if (worldPosition.x >= -0.9f && worldPosition.x <= 3.38f && worldPosition.y >= -2.3f && worldPosition.y <= 4.7f)
        {
            if (enemyTargeted != 2)
            {
                enemyHighlight[1].sprite = spriteEnemyTargeted;
                enemyTargeted = 2;
            }
        }
        else
        {
            if (spriteEnemyTargeted != null)
            {
                enemyHighlight[1].sprite = null;
            }

            if (enemyTargeted != 1)
            {
                enemyTargeted = 0;
            }
        }
    }

    public void HighlightCard(int index)
    {
        isNoCardsHighlighted = false;
        currentCardHighlighted = index;
        transformCurrentCard = listCardTransform[index - 1];
        transformCurrentTarget = listPositionsHighlighted[index - 1];
    }

    public void UnhighlightCard()
    {
        currentCardHighlighted = 0;
        StartCoroutine(CheckIfNoCardsAreHighlighted());
    }

    private IEnumerator CheckIfNoCardsAreHighlighted()
    {
        yield return new WaitForSeconds(0.1f);

        if (currentCardHighlighted == 0)
        {
            isNoCardsHighlighted = true;
        }
    }

    public void AssignCurrentCardPositions(int index)
    {
        listPositionsDefault.Clear();
        listPositionsHighlighted.Clear();
        transformCurrentCard = null;
        transformCurrentTarget = null;
        isNoCardsHighlighted = true;
        UnhighlightCard();

        switch (index)
        {
            case 5:
                for (int i = 0; i < positionDefault5.Length; i++)
                {
                    listPositionsDefault.Add(positionDefault5[i]);
                    listPositionsHighlighted.Add(positionHighlighted5[i]);
                }
                break;
            case 4:
                for (int i = 0; i < positionDefault4.Length; i++)
                {
                    listPositionsDefault.Add(positionDefault4[i]);
                    listPositionsHighlighted.Add(positionHighlighted4[i]);
                }
                break;
            case 3:
                for (int i = 0; i < positionDefault3.Length; i++)
                {
                    listPositionsDefault.Add(positionDefault3[i]);
                    listPositionsHighlighted.Add(positionHighlighted3[i]);
                }
                break;
            case 2:
                for (int i = 0; i < positionDefault2.Length; i++)
                {
                    listPositionsDefault.Add(positionDefault2[i]);
                    listPositionsHighlighted.Add(positionHighlighted2[i]);
                }
                break;
            case 1:
                listPositionsDefault.Add(positionDefault1[0]);
                listPositionsHighlighted.Add(positionHighlighted1[0]);
                break;
        }

        for (int i = 0; i < 5; i++)
        {
            if (i + 1 == index)
            {
                beneathCardAreas[i].SetActive(true);
            }
            else
            {
                beneathCardAreas[i].SetActive(false);
            }
        }
    }
}
