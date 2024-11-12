using System;
using Cards.ScriptObject;
using UnityEngine;
using Utilities;

namespace Manager
{
    /// <summary>
    /// 游戏管理器类，负责管理游戏的各种逻辑，包括地图布局数据的更新。
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// 地图布局的单例对象，用于存储和管理地图的布局数据。
        /// </summary>
        public MapLayoutSO mapLayoutSO;

        

        /// <summary>
        /// 更新地图布局数据，根据给定的房间位置更新房间状态。
        /// </summary>
        /// <param name="value">表示房间位置的二维向量，x轴代表列，y轴代表行。</param>
        public void UpdateMapLayoutData(object value)
        {
            var roomVector = (Vector2Int)value;
            // 查找当前房间，以便更新其状态为已访问
            var currentRoom =
                mapLayoutSO.mapRoomDatasList.Find(r => r.column == roomVector.x && r.line == roomVector.y);
            currentRoom.roomState = RoomState.Visited;
    
            // 查找同一列的所有房间，并将它们的状态设置为锁定，除非它们是当前房间
            var sameColumnRooms = mapLayoutSO.mapRoomDatasList.FindAll(r => r.column == roomVector.x);
            foreach (var room in sameColumnRooms)
            {
                if (room.line!=roomVector.y)
                {
                    room.roomState = RoomState.Locked;
                } 
            }
    
            // 遍历当前房间链接的所有房间，并将它们的状态设置为可到达
            foreach (var link in currentRoom.linkToRooms)
            {
                var linkRoom = mapLayoutSO.mapRoomDatasList.Find(r => r.column == link.x && r.line == link.y); 
                linkRoom.roomState = RoomState.Attainable;
            }
        }
        
    }
}
