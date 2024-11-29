using System;
using Cards.ScriptObject;
using Character;
using Event.ScriptObject;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Utilities;
using Object = UnityEngine.Object;

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
        [HideInInspector] public bool isAnimating;
        [HideInInspector] public bool isAvailiable;
        [HideInInspector]public Player player;
        private BoxCollider2D _boxCollider2D;
        private float offset;
        private float size;
        public Object entry;
        [Header("广播事件")] 
        public ObjectEventSO disscardCardEvent;
        public IntEventSO costEvent;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            offset=_boxCollider2D.offset.y;
            size=_boxCollider2D.size.y;
        }

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
            // 检查是否正在执行动画，如果是，则不执行后续代码
            if (isAnimating)
            {
                return;
            }
            
            // 将游戏对象的位置设置为原始位置加上一个单位向上的偏移量
            transform.position = new Vector3(originalPosition.x, originalPosition.y , -1f);
            entry.GameObject().transform.position=originalPosition + Vector3.up;
            _boxCollider2D.size+=Vector2.up;
            _boxCollider2D.offset+=Vector2.up/2;
            // 将游戏对象的旋转设置为单位四元数，即没有旋转
            transform.rotation = quaternion.identity;
            // 获取游戏对象的SortingGroup组件，并将排序顺序设置为20
            GetComponent<SortingGroup>().sortingOrder = 20;
        }
        
        /// <summary>
        /// 当鼠标指针离开卡片区域时调用
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            // 检查是否正在执行动画，如果正在执行，则不进行后续操作
            if (isAnimating)
            {
                return;
            }
            
            // 重置卡片的Transform组件，恢复其默认位置和状态
            RestCardTransform();
        }
        
        
        /// <summary>
        /// 重置卡片的位置、旋转和排序顺序到其原始状态。
        /// 此方法用于在游戏或应用中恢复卡片的视觉状态至其初始位置和外观。
        /// </summary>
        public void RestCardTransform()
        {
            // 重置卡片的位置和旋转到其原始值。
            // 这是恢复卡片视觉状态至其初始位置和方向的关键步骤。
            transform.position=originalPosition;
            entry.GameObject().transform.SetPositionAndRotation(originalPosition, originalRotation);
            _boxCollider2D.size=new Vector2(_boxCollider2D.size.x,size);
            _boxCollider2D.offset=new Vector2(_boxCollider2D.offset.x,offset);
            
            // 恢复卡片的排序顺序到其原始值。
            // 这确保了卡片在渲染时的层级关系与初始状态一致。
            GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
        }

        /// <summary>
        /// 执行卡牌效果
        /// </summary>
        /// <param name="from">使用卡牌的角色</param>
        /// <param name="target">卡牌效果的目标角色</param>
        public void ExecuteCardEffects(CharacterBase from,CharacterBase target)
        {
            //减少能量，回收卡牌
            costEvent.RaiseEvent(cardData.cost,this);
            // 触发弃牌事件，通知所有订阅该事件的监听者
            disscardCardEvent.RaiseEvent(this,this);
            
            // 遍历卡片效果列表，执行每个效果
            foreach (var effect in cardData.CardEffects)
            {
                // 执行卡片效果，from 表示效果的来源，target 表示效果的目标
                effect.Excute(from, target);
            }
        }

        public void UpdateCardState()
        {
            isAvailiable=cardData.cost<=player.CurrentMana;
            costText.color=isAvailiable?Color.white:Color.red;
        }


    }

    
}
