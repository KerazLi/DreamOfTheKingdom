using System;
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
        currentEnemyAction.effect.Excute(this,player);
    }

    public void Skill() => currentEnemyAction.effect.Excute(this, this);
}
