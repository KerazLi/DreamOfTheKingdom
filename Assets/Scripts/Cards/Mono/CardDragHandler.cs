using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Cards.Mono
{
    public class CardDragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        private Card currentCard;
        private bool canMove;
        private bool canExecute;

        private void Awake()
        {
            currentCard = GetComponent<Card>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            switch (currentCard.cardData.cardType)
            {
                case CardType.Attack:
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
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (canExecute)
            {
                //TODO:卡牌效果
            }
            else
            {
                currentCard.RestCardTransform();
                currentCard.isAniamting = false;
            }
        }
    }
}
