using System;
using Character;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

namespace CardEffects
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "Effect/DamageEffect")]
    public class DamageEffect : CardEffect
    {
        public override void Excute(CharacterBase from, CharacterBase target)
        {
            
            if (target==null)
            {
                return;
            }

            switch (targetType)
            {
                case EffectTargetType.Self:
                    break;
                case EffectTargetType.Target:
                    var damage = (int)math.round(value * from.baseStrenth);
                    target.TakeDamage(damage);
                    Debug.Log($"造成了{value}的伤害");
                    break;
                case EffectTargetType.All:
                    foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        enemy.GetComponent<CharacterBase>().TakeDamage(value);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}