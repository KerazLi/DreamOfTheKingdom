using System;

// 定义房间类型枚举，表示地牢中不同功能的房间
[Flags]
public enum RoomType
{
    // 包含弱小敌人的房间
    MinorEnemy = 1,

    // 包含精英敌人的房间
    EliteEnemy = 2,

    // 商店房间，可能可以购买物品或服务
    Shop = 4,

    // 包含宝藏的房间
    Treasure = 8,

    // 休息室，可能允许玩家恢复状态
    RestRoom = 16,

    // 包含强大敌人的房间
    Boss = 32
}

// 定义房间状态枚举，表示房间的探索状态
public enum RoomState
{
    // 房间已被发现但未访问
    Looked,

    // 房间已被访问
    Visited,

    // 房间可以被访问但未被发现
    Attainable
}