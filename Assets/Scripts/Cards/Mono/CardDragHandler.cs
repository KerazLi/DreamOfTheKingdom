using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Cards.Mono
{
    public class CardDragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        public GameObject arrowPrefab;
        private GameObject currentArrow;
        private Card currentCard;
        private bool canMove;
        private bool canExecute;
        private CharacterBase target;

        private void Awake()
        {
            currentCard = GetComponent<Card>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            switch (currentCard.cardData.cardType)
            {
                case CardType.Attack:
                    currentArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                    break;
                case CardType.Defense:
                case CardType.Skill:
                    canMove = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (canMove &&Camera.main!=null)
            {
                currentCard.isAniamting = true;
                Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);
                
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
                currentCard.transform.position = worldPos;
                canExecute = worldPos.y > 1f;

            }
            else
            {
                if (eventData.pointerEnter==null)
                {
                    return;
                }

                if (eventData.pointerEnter.CompareTag("Enemy"))
                {
                    canExecute = true;
                    target = eventData.pointerEnter.GetComponent<CharacterBase>();
                    return;
                }

                canExecute = true;
                target = null;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (currentArrow!=null)
            {
                Destroy(currentArrow);
            }
            if (canExecute)
            {
                currentCard.ExecuteCardEffects(currentCard.player,target);
            }
            else
            {
                currentCard.RestCardTransform();
                currentCard.isAniamting = false;
            }
        }
    }
}
