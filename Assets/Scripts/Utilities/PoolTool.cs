using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Utilities
{
    public class PoolTool : MonoBehaviour
    {
        public GameObject objPrefab;
        private ObjectPool<GameObject> pool;

        private void Start()
        {
            // 初始化一个GameObject的对象池
            pool = new ObjectPool<GameObject>(
                // 定义创建对象的函数，当池中没有可用对象时调用
                createFunc: () => Instantiate(objPrefab, transform),
                
                // 定义从池中获取对象时执行的操作
                actionOnGet: obj => obj.SetActive(true),
                
                // 定义将对象释放回池中时执行的操作
                actionOnRelease: obj => obj.SetActive(false),
                
                // 定义销毁对象时执行的操作
                actionOnDestroy: obj => Destroy(obj),
                
                // 是否启用集合检查，这里设置为false，以提高性能
                collectionCheck: false,
                
                // 设置对象池的默认容量
                defaultCapacity: 10,
                
                // 设置对象池的最大容量
                maxSize: 100
                );
            PreFillPool(7);
        }

        /// <summary>
        /// 预先填充对象池以初始化或补充资源。
        /// </summary>
        /// <param name="count">要预填充的对象数量。</param>
        private void PreFillPool(int count)
        {
            // 创建一个数组以临时存储从对象池获取的游戏对象。
            var preFillArray = new GameObject[count];
            
            // 从对象池中获取指定数量的游戏对象，并将其存储在数组中。
            for (int i = 0; i < count; i++)
            {
                preFillArray[i]= pool.Get();
            }
            
            // 遍历数组中的每个游戏对象，将其释放回对象池。
            foreach (var gameObject in preFillArray)
            {
                pool.Release(gameObject);
            }
        }
        /// <summary>
        /// 从对象池中获取一个对象。
        /// </summary>
        /// <returns>返回从对象池中获取的游戏对象。</returns>
        public GameObject GetObjFromPool()
        {
            return pool.Get();
        }
        
        /// <summary>
        /// 将一个对象释放回对象池。
        /// </summary>
        /// <param name="obj">要释放回对象池的游戏对象。</param>
        public void ReleaseObjToPool(GameObject obj)
        {
            pool.Release(obj);
        }
    }
}