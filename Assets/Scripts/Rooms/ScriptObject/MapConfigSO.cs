using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图配置单例类
/// </summary>
[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO")]
public class MapConfigSo : ScriptableObject
{
    /// <summary>
    /// 房间蓝图列表，存储所有房间的蓝图信息
    /// </summary>
    public List<RoomBluePrint> roomBluePrints;
}


/// <summary>
///     表示房间蓝图的类，用于定义房间的基本属性。
/// </summary>
[Serializable]
public class RoomBluePrint
{
    /// <summary>
    /// 房间的最小尺寸,房间的最大尺寸。
    /// </summary>
    public int min, max;

    /// <summary>
    /// 房间的类型，决定了房间的功能和特征。
    /// </summary>
    public RoomType roomType;
}