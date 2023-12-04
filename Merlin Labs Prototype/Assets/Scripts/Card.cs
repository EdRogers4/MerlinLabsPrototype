using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public CardPositioner scriptCardPositioner;
    public Canvas canvas;

    void Start()
    {
        EventTrigger evTrig = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnterEvent = new EventTrigger.Entry();
        pointerEnterEvent.eventID = EventTriggerType.PointerEnter;
        pointerEnterEvent.callback.AddListener(CardHighlight);
        evTrig.triggers.Add(pointerEnterEvent);


        EventTrigger.Entry pointerExitEvent = new EventTrigger.Entry();
        pointerExitEvent.eventID = EventTriggerType.PointerExit;
        pointerExitEvent.callback.AddListener(CardUnhighlight);
        evTrig.triggers.Add(pointerExitEvent);


        EventTrigger.Entry dragEvent = new EventTrigger.Entry();
        dragEvent.eventID = EventTriggerType.Drag;
        dragEvent.callback.AddListener(CardDrag);
        evTrig.triggers.Add(dragEvent);
    }

    void Update()
    {
        
    }

    public void PlayCard()
    {
        scriptCardPositioner.listCardTransform.Remove(this.transform);
        scriptCardPositioner.AssignCurrentCardPositions(scriptCardPositioner.listCardTransform.Count);
        Destroy(this.gameObject);
    }

    private void CardHighlight(BaseEventData eventData)
    {
        PointerEventData pointerData = (PointerEventData)eventData;
        scriptCardPositioner.currentCardHighlighted = scriptCardPositioner.listCardTransform.IndexOf(this.transform) + 1;
        scriptCardPositioner.HighlightCard(scriptCardPositioner.listCardTransform.IndexOf(this.transform) + 1);
    }

    private void CardUnhighlight(BaseEventData eventData)
    {
        scriptCardPositioner.UnhighlightCard();
    }

    private void CardDrag(BaseEventData eventData)
    {
        scriptCardPositioner.isCardDrag = true;
        scriptCardPositioner.currentCard = this;
        PointerEventData pointerData = (PointerEventData)eventData;
        Vector2 cardPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out cardPosition);
        scriptCardPositioner.listCardTransform[scriptCardPositioner.currentCardHighlighted - 1].position = canvas.transform.TransformPoint(cardPosition);
    }
}
