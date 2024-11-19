using System;
using Cards.ScriptObject;
using Character;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Utilities;

namespace Cards.Mono
{
    public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        // 定义卡片的视觉组件和文本信息
        [Header("组件")] 
        public SpriteRenderer cardSprite;
        
        public TextMeshPro costText, descriptionText, typeText;
        
        // 卡片的数据源，包括图片、费用、描述和类型
        [HideInInspector]public CardDataSO cardData;
        
        // 保存卡片的原始位置、旋转、层级顺序和动画状态，用于重置卡片状态
        [Header("原始的数据")] 
        [HideInInspector]public Vector3 originalPosition;
        [HideInInspector] public Quaternion originalRotation;
        [HideInInspector]public int originalLayerOrder;
        [HideInInspector] public bool isAniamting;
        public Player player;
        [Header("广播事件")] public ObjectEventSO disscardCardEvent;
        
        /// <summary>
        /// 初始化卡片数据
        /// </summary>
        /// <param name="data">卡片数据源，包含卡片的所有必要信息</param>
        public void InintCardData(CardDataSO data)
        {
            cardData = data;
            cardSprite.sprite = data.cardImage;
            costText.text = data.cost.ToString();
            descriptionText.text = data.cardDescription;
            typeText.text = data.cardType switch
            {
                CardType.Attack => "攻击",
                CardType.Defense => "防御",
                CardType.Skill => "技能",
                _ => throw new ArgumentOutOfRangeException()
            };
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        
        /// <summary>
        /// 更新卡片的位置和旋转
        /// </summary>
        /// <param name="position">新的位置</param>
        /// <param name="rotation">新的旋转</param>
        public void UpdatePositionRotation(Vector3 position,Quaternion rotation)
        {
            originalPosition = position;
            originalRotation = rotation;
            originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
        }
        
        /// <summary>
        /// 当鼠标指针进入卡片区域时调用
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isAniamting)
            {
                return;
            }
        
            transform.position = originalPosition + Vector3.up;
            transform.rotation = quaternion.identity;
            GetComponent<SortingGroup>().sortingOrder = 20;
        }
        
        /// <summary>
        /// 当鼠标指针离开卡片区域时调用
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (isAniamting)
            {
                return;
            }
            RestCardTransform();
        }
        
        /// <summary>
        /// 重置卡片的位置、旋转和层级顺序到原始状态
        /// </summary>
        public void RestCardTransform()
        {
            transform.SetPositionAndRotation(originalPosition,originalRotation);
            GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
        }

        public void ExecuteCardEffects(CharacterBase from,CharacterBase target)
        {
            //TODO:减少能量，回收卡牌
            disscardCardEvent.RaiseEvent(this,this);
            foreach (var effect in cardData.CardEffects)
            {
                effect.Excute(from, target);
            }
        }


    }
}
