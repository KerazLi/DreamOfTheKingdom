using Character;
using UnityEngine;
using Utilities;

namespace CardEffects
{
    [CreateAssetMenu(fileName = "HealthEffect", menuName = "Effect/HealthEffect", order = 0)]
    public class HealthEffect : CardEffect
    {
        public override void Excute(CharacterBase from, CharacterBase target)
        {
            if (targetType==EffectTargetType.Self)
            {
                from.HealHealth(value);
                Debug.Log($"自身回复了{value}点生命");
            }
            if (targetType==EffectTargetType.All)
            {
                from.HealHealth(value);
            }
        }
    }
}
