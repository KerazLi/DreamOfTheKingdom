
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour
{
    // 使用 FormerlySerializedAs 属性标记变量，以便与之前序列化时保持兼容
    // 该变量保存了地图配置的单例对象
    [FormerlySerializedAs("mapConfigSO")] public MapConfigSo mapConfigSo;
    
    // 房间预设对象，用于在地图生成时实例化房间
    public Room roomPrefab;
    
    // 用于存储屏幕高度的私有变量
    private float _screenHeight;
    
    // 用于存储屏幕宽度的私有变量
    private float _screenWidth;
    
    // 每列房间的宽度，用于计算房间位置和布局
    private float _columnWidth;
    
    // 用于存储生成房间时的位置信息
    private Vector3 _generatedPosition;
    
    // 边界区域的宽度，用于在地图边缘留出空白区域
    public float border;
    
    // 保存已经生成的房间实例的列表
    private List<Room> _rooms=new();

    /// <summary>
    /// 在游戏对象觉醒时初始化屏幕宽高和列宽度。
    /// </summary>
    private void Awake()
    {
        // 计算屏幕高度，基于主相机的正交投影大小乘以2
        _screenHeight = Camera.main.orthographicSize * 2;
        
        // 计算屏幕宽度，基于屏幕高度和相机的宽高比
        _screenWidth = _screenHeight * Camera.main.aspect;
        
        // 计算每列的宽度，将屏幕宽度分成房间蓝图数量加1份
        _columnWidth = _screenWidth / (mapConfigSo.roomBluePrints.Count + 1);
    }


    private void Start()
    {
        GreateMap();
    }
    
    /// <summary>
    /// 生成地图
    /// </summary>
    public void GreateMap()
    {
        // 遍历地图配置中的每个房间蓝图
        for (int column = 0; column < mapConfigSo.roomBluePrints.Count; column++)
        {
            // 获取当前遍历到的房间蓝图
            var roomBluePrint = mapConfigSo.roomBluePrints[column];
            // 随机决定当前房间蓝图的房间数量
            var amount = Random.Range(roomBluePrint.min, roomBluePrint.max);
            // 计算房间起始高度
            var startHeight = _screenHeight / 2 - _screenHeight / (amount + 1);
            // 初始化房间生成位置
            _generatedPosition = new Vector3(-_screenWidth / 2 + border + _columnWidth * column, startHeight, 0);
            // 设置房间的起始位置
            var newPosition = _generatedPosition;
            // 计算房间之间的垂直间距
            var roomGapY = _screenHeight / (amount + 1);
            // 根据随机决定的房间数量生成房间
            for (int i = 0; i < amount; i++)
            {
                // 随机偏移房间的位置
                /*if (column==mapConfigSo.roomBluePrints.Count-1)
                {
                    newPosition.x = _screenWidth / 2 - 1.2f * 2;
                }else if (column!=0)
                {
                    newPosition.x=_generatedPosition.x+Random.Range(-1.2f/2,1.2f/2);
                }*/

                // 计算当前房间的实际位置
                newPosition.y = startHeight - roomGapY * i;
                // 在计算出的位置实例化房间对象，并将其添加到当前变换下
                var room = Instantiate(roomPrefab, newPosition, quaternion.identity, transform);
                // 将生成的房间添加到房间列表中
                _rooms.Add(room);
            }
        }
    }
    /// <summary>
    /// 重新生成房间
    /// </summary>
    [ContextMenu("ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        // 销毁所有现有房间对象
        foreach (var room in _rooms)
        {
            Destroy(room.gameObject);
        }
        
        // 清空房间列表
        _rooms.Clear();
        
        // 重新生成地图
        GreateMap();
    }

}
