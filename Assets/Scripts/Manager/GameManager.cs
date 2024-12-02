using System;
using System.Collections;
using System.Collections.Generic;
using Cards.ScriptObject;
using Character;
using Event.ScriptObject;
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

        public List<Enemy> aliveEnemyList=new ();
        [Header("广播")]
        public ObjectEventSO gameWinEvent;
        public ObjectEventSO gameOverEvent;

        

        /// <summary>
        /// 更新地图布局数据，根据给定的房间位置更新房间状态。
        /// </summary>
        /// <param name="value">表示房间位置的二维向量，x轴代表列，y轴代表行。</param>
        public void UpdateMapLayoutData(object value)
        {
            if (mapLayoutSO.mapRoomDatasList.Count==0)
            {
                return;
            }
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
            aliveEnemyList.Clear();
        }

        public void OnRoomLoadEvent(object obj)
        {
            var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var enemy in enemies)
            {
                aliveEnemyList.Add(enemy);
            }
        }

        /// <summary>
        /// 当角色死亡时触发的事件处理方法。
        /// </summary>
        /// <param name="character">死亡的角色对象。</param>
        public void OnChararcterDeadEvent(object character)
        {
            // 当死亡的角色是玩家时，触发游戏结束事件。
            if (character is Player)
            {
                StartCoroutine(EventDelayAction(gameOverEvent));
            }
            
            // 当死亡的角色是敌人时，从存活敌人列表中移除，并检查是否所有敌人都已死亡。
            if (character is Enemy)
            {
                aliveEnemyList.Remove(character as Enemy);
                // 如果所有敌人都已死亡，则触发游戏胜利事件。
                if (aliveEnemyList.Count==0)
                {
                    StartCoroutine(EventDelayAction(gameWinEvent));
                }
            }
        }

        /// <summary>
        /// 延迟触发事件的协程方法。
        /// </summary>
        /// <param name="eventSo">要触发的事件对象。</param>
        IEnumerator EventDelayAction(ObjectEventSO eventSo)
        {
            // 等待1.5秒，用于延迟事件的触发
            yield return new WaitForSeconds(1.5f);
            
            // 触发事件，传递null作为事件参数，本对象作为事件源
            eventSo.RaiseEvent(null, this);
        }

        public void NewGameStart()
        {
            mapLayoutSO.mapRoomDatasList.Clear();
            mapLayoutSO.linePositions.Clear();
            
        }

    }
}
