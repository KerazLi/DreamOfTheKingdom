using Character;
using UnityEngine;
using Utilities;

namespace CardEffects
{
     [CreateAssetMenu(fileName = "DefenseEffect", menuName = "Effect/DefenseEffect", order = 0)]
    public class DefenseEffect : CardEffect
    {
        
        public override void Excute(CharacterBase from, CharacterBase target)
        {
            if (targetType==EffectTargetType.Self)
            {
                from.UpdateDefense(value);
            }
            if (targetType==EffectTargetType.Target)
            {
                from.UpdateDefense(value);
            }
            if (targetType==EffectTargetType.All)
            {
                from.UpdateDefense(value);
            }
        }
    }
}
