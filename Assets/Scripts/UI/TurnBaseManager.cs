using System;
using System.Collections;
using Event.ScriptObject;
using UnityEngine;
using Utilities;

namespace UI
{
    public class TurnBaseManager : MonoBehaviour
    {
        public GameObject playerObj;
        private bool isPlayerTurn ;
        private bool isEnemyTurn;
        public bool battleEnd = true;
        private float timeCounter;
        public float enemyTurnDuration;
        public float playerTurnDuration;
        [Header("事件广播")] 
        public ObjectEventSO playerTurnBegin;
        public ObjectEventSO enemyTurnBegin;
        public ObjectEventSO enemyTurnEnd;
        

        private void Update()
        {
            if (battleEnd)
            {
                return;
            }

            if (isEnemyTurn)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= enemyTurnDuration)
                {
                    timeCounter = 0f;
                    //敌人回合结束
                    EnemyTurnEnd();
                    //玩家回合开始
                    isPlayerTurn = true;
                    isEnemyTurn = false;
                }
            }

            if (isPlayerTurn)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter >= playerTurnDuration)
                {
                    timeCounter = 0f;
                    //玩家回合开始
                    PlayerTurnBegin();
                    isPlayerTurn = false;
                }
            }
        }
        [ContextMenu("Game Start")]
        private void GameStart()
        {
            isPlayerTurn = true;
            isEnemyTurn = false;
            battleEnd = false;
            timeCounter = 0;
        }

        public void PlayerTurnBegin()
        {
            playerTurnBegin.RaiseEvent(null,this);
            Debug.Log("PlayerTurnBegin");
        }
        
        public void EnemyTurnBegin()
        {
            enemyTurnBegin.RaiseEvent(null,this);
            isEnemyTurn=true;
            Debug.Log("EnemyTurnBegin");
        }
        public void EnemyTurnEnd()
        {
            isEnemyTurn=false;
            enemyTurnEnd.RaiseEvent(null,this);
            Debug.Log("EnemyTurnEnd");
        }

        public void OnRoomLoadedEvent(object obj)
        {
            Room room = obj as Room;
            switch (room.roomData.roomType)
            {
                case RoomType.MinorEnemy:
                case RoomType.EliteEnemy:
                case RoomType.Boss:
                    playerObj.SetActive(true);
                    GameStart();
                    break;
                case RoomType.Shop:
                case RoomType.Treasure:
                    playerObj.SetActive(false);
                    break;
                case RoomType.RestRoom:
                    playerObj.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnLoadMapEvent()
        {
            battleEnd = true;
            playerObj.SetActive(false);
        }



    }
}
