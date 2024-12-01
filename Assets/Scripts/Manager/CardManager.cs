using System;
using System.Collections.Generic;
using Cards.ScriptObject;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;
using Random = UnityEngine.Random;

namespace Manager
{
    public class CardManager : MonoBehaviour
    {
        // PoolTool用于对象池管理，优化性能
        public PoolTool poolTool;
        // cardDataList存储卡片数据，用于游戏逻辑
        public List<CardDataSO> cardDataList;
        
        [Header("卡牌库")]
        public CardLibrarySO newGameCardLibrary;
        
        public CardLibrarySO currentCardLibrary;

        private int previousIndex;
        
        
        // 当脚本实例被创建时，Awake方法会被调用
        private void Awake()
        {
            
            // 初始化卡片数据列表，确保当前卡片库为空，避免数据重复或冲突
            InitializeCardDataList();
            
            // 遍历新游戏卡片库中的每一张卡片，将它们添加到当前卡片库中
            foreach (var item in newGameCardLibrary.cardLibraryList)
            {
                currentCardLibrary.cardLibraryList.Add(item);
            }
        }
        
        // 初始化卡片数据列表
        private void InitializeCardDataList()
        {
            // 使用Addressables加载所有卡片数据，当加载完成时调用OnCardDataLoaded
            Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
            //测试
            currentCardLibrary.cardLibraryList.RemoveAll(x=>x.cardData==null);
        }
        
        // 当卡片数据加载完成时调用此方法
        private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
        {
            // 检查加载操作是否成功
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // 如果成功，将加载的卡片数据分配给cardDataList
                cardDataList = new(handle.Result);
            }
            else
            {
                // 如果失败，输出错误日志
                Debug.LogError("No CardData Found");
            }
        }

        /// <summary>
        /// 获取一个卡片对象。
        /// </summary>
        /// <returns>从对象池中获取的卡片对象。</returns>
        public GameObject GetCardObject()
        {
            var cardObj = poolTool.GetObjFromPool();
            cardObj.transform.localScale=Vector3.zero;
            // 从对象池工具中获取一个对象
            return cardObj;
        }
        
        /// <summary>
        /// 释放一个卡片对象回对象池。
        /// </summary>
        /// <param name="obj">需要释放的卡片对象。</param>
        public void ReleaseCardObject(GameObject obj)
        {
            // 将对象释放回对象池
            poolTool.ReleaseObjToPool(obj);
        }

        public CardDataSO GetNewCardData()
        {
            var randomIndex = 0;
            do
            {
                randomIndex = Random.Range(0, cardDataList.Count);
            } while (previousIndex==randomIndex);
            previousIndex=randomIndex;
            return cardDataList[randomIndex];
        }

        private void OnDisable()
        {
            currentCardLibrary.cardLibraryList.Clear();
        }

        public void UnlockCard(CardDataSO newCardData)
        {
            var newCard = new CardLibraryEntry
            {
                cardData = newCardData,
                cardCount = 1
            };
            if (currentCardLibrary.cardLibraryList.Contains(newCard))
            {
                var target=currentCardLibrary.cardLibraryList.Find(x=>x.cardData==newCardData);
                target.cardCount++;
            }
            else
            {
                currentCardLibrary.cardLibraryList.Add(newCard);
            }
        }
    }
}
