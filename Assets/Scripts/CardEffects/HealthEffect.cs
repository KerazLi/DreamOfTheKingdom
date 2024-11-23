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
            }
            if (targetType==EffectTargetType.Target)
            {
                from.HealHealth(value);
            }
            if (targetType==EffectTargetType.All)
            {
                from.HealHealth(value);
            }
        }
    }
}
