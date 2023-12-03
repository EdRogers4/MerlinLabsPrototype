using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPositioner : MonoBehaviour
{
    public Transform[] card;
    [SerializeField] private Transform[] positionDefault;
    [SerializeField] private Transform[] positionDefaultLeft;
    [SerializeField] private Transform[] positionDefaultRight;
    [SerializeField] private Transform[] positionHighlighted;
    [SerializeField] private float speedMoveHighlighted;
    [SerializeField] private float speedRotateHighlighted;
    [SerializeField] private Canvas canvas;
    private Transform transformCurrentCard;
    private Transform transformCurrentTarget;
    private int currentCardHighlighted;
    private bool isNoCardsHighlighted;
    private bool isCardDrag;

    void Start()
    {
        
    }

    void Update()
    {

        if ((Input.GetMouseButtonUp(0)) && isCardDrag)
        {
            isCardDrag = false;
        }

        if (currentCardHighlighted > 0)
        {
            float step = speedMoveHighlighted * Time.deltaTime;

            if (!isCardDrag)
            {
                transformCurrentCard.position = Vector2.MoveTowards(transformCurrentCard.position, transformCurrentTarget.position, step);
                transformCurrentCard.rotation = Quaternion.Slerp(transformCurrentCard.rotation, transformCurrentTarget.rotation, speedRotateHighlighted * Time.deltaTime);
            }

            for (int i = 0; i < card.Length; i++)
            {
                if (i != currentCardHighlighted -1)
                {
                    if (i < currentCardHighlighted - 1)
                    {
                        card[i].position = Vector2.MoveTowards(card[i].position, positionDefaultLeft[i].position, step);
                        card[i].rotation = Quaternion.Slerp(card[i].rotation, positionDefaultLeft[i].rotation, speedRotateHighlighted * Time.deltaTime);
                    }
                    else if (i > currentCardHighlighted - 1)
                    {
                        card[i].position = Vector2.MoveTowards(card[i].position, positionDefaultRight[i].position, step);
                        card[i].rotation = Quaternion.Slerp(card[i].rotation, positionDefaultRight[i].rotation, speedRotateHighlighted * Time.deltaTime);
                    }
                }
            }
        }
        else if (isNoCardsHighlighted)
        {
            for (int i = 0; i < card.Length; i++)
            {
                float step = speedMoveHighlighted * Time.deltaTime;
                card[i].position = Vector2.MoveTowards(card[i].position, positionDefault[i].position, step);
                card[i].rotation = Quaternion.Slerp(card[i].rotation, positionDefault[i].rotation, speedRotateHighlighted * Time.deltaTime);
            }
        }
    }

    public void HighlightCard(int index)
    {
        isNoCardsHighlighted = false;
        currentCardHighlighted = index;
        transformCurrentCard = card[index - 1];
        transformCurrentTarget = positionHighlighted[index - 1];
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

    public void DragHandler(BaseEventData data)
    {
        isCardDrag = true;
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 cardPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out cardPosition);
        card[currentCardHighlighted - 1].position = canvas.transform.TransformPoint(cardPosition);
    }
}
