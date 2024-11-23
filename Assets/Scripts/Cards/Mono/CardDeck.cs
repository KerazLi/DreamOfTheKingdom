using System.Collections.Generic;
using Cards.ScriptObject;
using DG.Tweening;
using Event.ScriptObject;
using Manager;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

namespace Cards.Mono
{
    public class CardDeck : MonoBehaviour
    {
        // CardManager用于管理卡片，这里声明一个CardManager类型的成员变量
        public CardManager cardManager;
        // 抽牌堆，用于存储待抽取的卡片数据
        private  List<CardDataSO> drawDeck=new();
        // 弃牌堆，用于存储已经被弃置的卡片数据
        private   List<CardDataSO> discardDeck=new();
        // 手牌堆，用于存储玩家手中的卡片对象
        [SerializeField] private List<Card> handCard=new();
        public CardLayoutManager cardLayoutManager;

        public Vector3 deckPosition;
        [Header("事件广播")]
        [SerializeField] private IntEventSO drawCountEvent;
        [SerializeField] private IntEventSO discardCountEvent;
    
        /// <summary>
        /// 测试初始化牌堆的方法
        /// </summary>
        private void Start()
        {
            InitializeDeck();
            //DrawCard(3);
        }
    
        /// <summary>
        /// 初始化抽牌堆，根据CardManager中的卡片库来填充抽牌堆
        /// </summary>
        public void InitializeDeck()
        {
            drawDeck.Clear();
            foreach (var cardData in cardManager.currentCardLibrary.cardLibraryList)
            {
                for (int i = 0; i < cardData.cardCount; i++)
                {
                    drawDeck.Add(cardData.cardData);
                }
            }
            
            ShuffleDeck();
        }
    
        /// <summary>
        /// 用于测试抽牌的函数
        /// </summary>
        [ContextMenu("抽牌")]
        private void TestDrawCard()
        {
            DrawCard(1);
        }
        
        /// <summary>
        /// 新回合的抽牌,事件监听
        /// </summary>
        public void NewTurnDrawCard()
        {
            DrawCard(4);
        }


        /// <summary>
        /// 从抽牌堆中抽牌的方法
        /// </summary>
        /// <param name="amount">要抽取的卡片数量</param>
        private void DrawCard(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (drawDeck.Count==0)
                {
                    foreach (var item in discardDeck)
                    {
                       drawDeck.Add(item); 
                    }
                    ShuffleDeck();
                }
                CardDataSO currentCard=drawDeck[0];
                drawDeck.RemoveAt(0);
                //更新UI数量
                drawCountEvent.RaiseEvent(drawDeck.Count,this);
                var card = cardManager.GetCardObject().GetComponent<Card>();
                card.InintCardData(currentCard);
                card.transform.position=deckPosition;
                handCard.Add(card);
                var delay = i * 0.2f;
                SetCardLayout(delay);
            }
        
        }

        /// <summary>
        /// 设置卡片布局
        /// </summary>
        private void SetCardLayout(float delay)
        {
            // 遍历每一张手牌
            for (int i = 0; i < handCard.Count; i++)
            {
                // 获取当前卡片
                Card currentCard = handCard[i];
            
                // 根据当前卡片的索引和手牌总数，获取卡片应该显示的位置和旋转信息
                CardTransform cardTransform = cardLayoutManager.GetCardTransform(i, handCard.Count);
            
                // 设置当前卡片的位置为计算得到的位置
                //currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);
            

                //卡牌的缩放为1
                currentCard.transform.DOScale(Vector3.one, 0.2f).SetDelay(delay).onComplete= () =>
                {
                    currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete =
                        () => currentCard.isAnimating = false;
                    currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
                };
            
                // 设置卡牌的排序
                currentCard.GetComponent<SortingGroup>().sortingOrder = i;
                currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
            }
        }
        /// <summary>
        /// 洗牌
        /// </summary>
        private void ShuffleDeck()
        {
            discardDeck.Clear();
            //更新UI显示数量
            drawCountEvent.RaiseEvent(drawDeck.Count,this);
            discardCountEvent.RaiseEvent(discardDeck.Count,this);

            for (int i = 0; i < drawDeck.Count; i++)
            {
                CardDataSO cardData = drawDeck[i];
                int randomIndex = Random.Range(1,drawDeck.Count);
                drawDeck[i] = drawDeck[randomIndex];
                drawDeck[randomIndex] = cardData;
            }
        }
        /// <summary>
        /// 弃牌
        /// </summary>
        /// <param name="card"></param>
        public void DiscardCard(object obj)
        {
            Card card=obj as Card;
            if (card != null)
            {
                discardDeck.Add(card.cardData);
                handCard.Remove(card);
                cardManager.ReleaseCardObject(card.gameObject);
            }

            discardCountEvent.RaiseEvent(discardDeck.Count,this);
            SetCardLayout(0f);
        }

        public void OnPlayerTurnEnd()
        {
            foreach (var item in handCard)
            {
               discardDeck.Add(item.cardData);
               cardManager.ReleaseCardObject(item.gameObject);
            }
            handCard.Clear();
            discardCountEvent.RaiseEvent(discardDeck.Count,this);
        }


    }
}
