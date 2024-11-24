using System;
using Character;
using UnityEngine;
using Utilities;

namespace CardEffects
{
    [CreateAssetMenu(fileName = "StrengthEffect", menuName = "Effect/StrengthEffect", order = 0)]
    public class StrengthEffect : CardEffect
    {
        public override void Excute(CharacterBase from, CharacterBase target)
        {
            switch (targetType)
            {
                case EffectTargetType.Self:
                    from.SetupStrenth(value,true);
                    break;
                case EffectTargetType.Target:
                    target.SetupStrenth(value,false);
                    break;
                case EffectTargetType.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
