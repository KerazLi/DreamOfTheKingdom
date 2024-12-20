using System;
using System.Collections;
using EnemyAction;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;


namespace Character
{
    public class Enemy : CharacterBase
    {
        public EnemyActionDataSO actionDataSO;
        public EnemyAction.EnemyAction currentEnemyAction;
        protected Player player;
    

        public virtual void OnPlayerTurnBegin()
        {
            var randomIndex = Random.Range(0,actionDataSO.actions.Count);
            currentEnemyAction = actionDataSO.actions[randomIndex];
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        public virtual void OnEnemyTurnBegin()
        {
            RestDefense();
            switch (currentEnemyAction.effect.targetType)
            {
                case EffectTargetType.Self:
                    Skill();
                    break;
                case EffectTargetType.Target:
                    Attack();
                    break;
                case EffectTargetType.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Attack()
        {
            StartCoroutine(ProcessDelayAction("attack"));
        }

        public void Skill()
        {
            StartCoroutine(ProcessDelayAction("skill"));
        }
        IEnumerator ProcessDelayAction(string name)
        {
            animator.SetTrigger(name);
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.6f && !animator.IsInTransition(0)&&animator.GetCurrentAnimatorStateInfo(0).IsName(name));
            if (name=="attack")
            {
                currentEnemyAction.effect.Excute(this,player);
            }else if (name=="skill")
            {
                currentEnemyAction.effect.Excute(this,this);
            }
        }
    

    
    
    }
}
