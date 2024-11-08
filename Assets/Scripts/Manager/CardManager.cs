using System;
using System.Collections.Generic;
using Cards.ScriptObject;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;

namespace Manager
{
    public class CardManager : MonoBehaviour
    {
        // PoolTool用于对象池管理，优化性能
        public PoolTool poolTool;
        // cardDataList存储卡片数据，用于游戏逻辑
        public List<CardDataSO> cardDataList;
        
        // 当脚本实例被创建时，Awake方法会被调用
        private void Awake()
        {
            InitializeCardDataList();
        }
        
        // 初始化卡片数据列表
        private void InitializeCardDataList()
        {
            // 使用Addressables加载所有卡片数据，当加载完成时调用OnCardDataLoaded
            Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
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
    }
}
