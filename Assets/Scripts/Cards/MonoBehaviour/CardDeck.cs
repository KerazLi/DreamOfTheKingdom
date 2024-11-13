using System.Collections.Generic;
using Cards.MonoBehaviour;
using Cards.ScriptObject;
using DG.Tweening;
using Manager;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

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
    
    /// <summary>
    /// 测试初始化牌堆的方法
    /// </summary>
    private void Start()
    {
        InitializeDeck();
        DrawCard(3);
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
        //TODO:洗牌/更新抽牌堆or弃牌堆的数字
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
    /// 从抽牌堆中抽牌的方法
    /// </summary>
    /// <param name="amount">要抽取的卡片数量</param>
    private void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count==0)
            {
                //TODO:洗牌/更新抽牌堆OR弃牌堆的数字
            }
            CardDataSO currentCard=drawDeck[0];
            drawDeck.RemoveAt(0);
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
                    () => currentCard.isAniamting = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
            };
            
            // 设置卡牌的排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }


}