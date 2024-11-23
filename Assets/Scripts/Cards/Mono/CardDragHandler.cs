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
            if (!currentCard.isAvailiable)
            {
                return;
            }
            switch (currentCard.cardData.cardType)
            {
                case CardType.Attack:
                    canMove = false;
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
            if (!currentCard.isAvailiable)
            {
                return;
            }
            
            // 检查是否可以移动并且主相机存在
            if (canMove)
            {
                // 设置当前卡片为正在动画状态
                currentCard.isAnimating = true;

                // 获取鼠标在屏幕上的位置，并设置其Z轴为10，以便后续转换到世界坐标
                Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);

                // 将屏幕坐标转换为世界坐标
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

                // 将当前卡片的位置设置为转换后的世界坐标
                currentCard.transform.position = worldPos;

                // 如果世界坐标的y值大于1，则可以执行某些操作（例如：放置卡片）
                canExecute = worldPos.y > 1f;
            }


            // 如果鼠标没有悬停在任何对象上，则直接返回
                if (eventData.pointerEnter == null)
                {
                    return;
                }

                // 如果鼠标悬停在标记为"Enemy"的对象上
                if (eventData.pointerEnter.CompareTag("Enemy"))
                {
                    // 设置可以执行某些操作，并将目标设置为悬停的敌人角色
                    canExecute = true;
                    target = eventData.pointerEnter.GetComponent<CharacterBase>();
                    return;
                }
            


        }

        /// <summary>
        /// 在拖拽结束时调用，处理拖拽结束后的逻辑。
        /// </summary>
        /// <param name="eventData">拖拽事件的数据。</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!currentCard.isAvailiable)
            {
                return;
            }
            // 销毁当前箭头对象，如果存在
            if (currentArrow!=null)
            {
                Destroy(currentArrow);
            }
            
            // 根据是否可以执行操作，决定当前卡片的操作
            if (canExecute)
            {
                // 执行卡片效果，如果可以执行
                currentCard.ExecuteCardEffects(currentCard.player,target);
            }
            else
            {
                // 如果不能执行，恢复卡片的位置，并设置动画状态为false
                currentCard.RestCardTransform();
                currentCard.isAnimating = false;
            }
        }

        /// <summary>
        /// 当对象被禁用时调用的方法
        /// </summary>
        private void OnDisable()
        {
            // 设置canExecute为false，表示不能执行某些操作
            canExecute = false;
            // 设置canMove为false，表示不能进行移动
            canMove = false;
        }
    }
}
