using System;
using System.Collections.Generic;
using Cards.ScriptObject;
using Event.ScriptObject;
using Manager;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace UI
{
    public class PickCardPanel : MonoBehaviour
    {
        public CardManager cardManager;
        private VisualElement rootElement;
        public VisualTreeAsset cardTemplate;
        private VisualElement cardContainer;
        private CardDataSO currentCardData;
        private Button confirmButton;
        private List<Button> cardButtons = new();
        [Header("广播")]
        public ObjectEventSO pickCardEvent;

        /// <summary>
        /// 在对象启用时初始化UI元素。
        /// </summary>
        private void OnEnable()
        {
            // 获取UIDocument组件的根视觉元素。
            rootElement = GetComponent<UIDocument>().rootVisualElement;
            // 通过选择器获取名为"Container"的视觉元素，用于存放卡片。
            cardContainer = rootElement.Q<VisualElement>("Container");
            confirmButton = rootElement.Q<Button>("ConfirmButton");
        
            // 循环创建并初始化3张卡片。
            for (int i = 0; i < 3; i++)
            {
                // 实例化卡片模板。
                var card = cardTemplate.Instantiate();
                // 从卡片管理器获取新的卡片数据。
                var data = cardManager.GetNewCardData();
                // 通过选择器获取卡片上的按钮元素。
                var cardButton = card.Q<Button>("Card");
                // 初始化卡片及其数据。
                InitCard(card, data);
                // 将卡片添加到卡片容器中。
                cardContainer.Add(card);
                // 将卡片按钮添加到卡片按钮列表中。
                cardButtons.Add(cardButton);
                // 为卡片按钮的点击事件添加监听。
                cardButton.clicked += () => OnCardClicked(cardButton, data);
            }

            confirmButton.clicked += OnConfirmButtonClicked;
        }

        private void OnConfirmButtonClicked()
        {
            cardManager.UnlockCard(currentCardData);
            pickCardEvent.RaiseEvent(null,this);
        }

        /// <summary>
        /// 处理卡片点击事件的方法。
        /// </summary>
        /// <param name="cardButton">被点击的卡片按钮。</param>
        /// <param name="data">被点击卡片的相关数据。</param>
        private void OnCardClicked(Button cardButton, CardDataSO data)
        {
            // 将当前点击的卡片数据设置为当前卡片数据。
            currentCardData = data;
    
            // 当卡片被点击时，遍历所有卡片按钮，禁用被点击的卡片按钮，其他卡片按钮启用。
            for (int i = 0; i < cardButtons.Count; i++)
            {
                // 如果当前遍历到的卡片按钮与被点击的卡片按钮相同，则禁用该按钮。
                if (cardButtons[i] == cardButton)
                {
                    cardButtons[i].SetEnabled(false);
                }
                // 如果当前遍历到的卡片按钮与被点击的卡片按钮不同，则启用该按钮。
                else
                {
                    cardButtons[i].SetEnabled(true);
                }
            }
        }

        /// <summary>
        /// 初始化卡片视图。
        /// </summary>
        /// <param name="card">要初始化的卡片视图元素。</param>
        /// <param name="cardData">卡片的数据源，包含卡片的各种信息。</param>
        public void InitCard(VisualElement card, CardDataSO cardData)
        {
            // 查询并获取卡片视图中名为 "CardSprite" 的视觉元素，用于显示卡片的图像。
            var cardSpriteElement = card.Q<VisualElement>("CardSprite");
            // 查询并获取卡片视图中名为 "CardCost" 的标签元素，用于显示卡片的成本。
            var cardCost = card.Q<Label>("CardCost");
            // 查询并获取卡片视图中名为 "CardDescription" 的标签元素，用于显示卡片的描述。
            var cardDescription = card.Q<Label>("CardDescription");
            // 查询并获取卡片视图中名为 "CardType" 的标签元素，用于显示卡片的类型。
            var cardType = card.Q<Label>("CardType");
    
            // 设置卡片视觉元素的背景图像为卡片数据源中提供的图像。
            cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
            // 设置卡片成本标签的文本为卡片数据源中成本值的字符串表示。
            cardCost.text = cardData.cost.ToString();
            // 设置卡片描述标签的文本为卡片数据源中提供的描述文本。
            cardDescription.text = cardData.cardDescription;
            // 根据卡片数据源中的卡片类型，设置卡片类型标签的文本。
            cardType.text = cardData.cardType switch
            {
                CardType.Attack => "攻击",
                CardType.Defense => "防御",
                CardType.Skill => "技能",
                // 如果卡片类型不在预期范围内，抛出异常。
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
