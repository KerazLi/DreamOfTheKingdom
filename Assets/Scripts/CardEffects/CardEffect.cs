using UnityEngine;
using Utilities;

namespace CardEffects
{
    [CreateAssetMenu(fileName = "CardEffect", menuName = "Effect/CardEffect", order = 0)]
    public abstract class CardEffect : ScriptableObject
    {
        public int value;
        public EffectTargetType targetType;
        public abstract void Excute(CharacterBase from ,CharacterBase target);
    }
}
