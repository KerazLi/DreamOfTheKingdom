using System;
using System.Collections;
using Event.ScriptObject;
using UnityEngine;

namespace UI
{
    public class TurnBaseManager : MonoBehaviour
    {
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
        


    }
}
