using System;
using System.Collections;
using Character;
using EnemyAction;
using UnityEngine;
using UnityEngine.Timeline;
using Utilities;
using Random = UnityEngine.Random;


public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionDataSO;
    public EnemyAction.EnemyAction currentEnemyAction;
    protected Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Debug.Log(VariableTool.Skill.GetHashCode());
    }

    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0,actionDataSO.actions.Count);
        currentEnemyAction = actionDataSO.actions[randomIndex];
    }
    public virtual void OnEnemyTurnBegin()
    {
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
