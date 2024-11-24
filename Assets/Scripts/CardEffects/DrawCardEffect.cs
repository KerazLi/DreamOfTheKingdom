using Character;
using Event.ScriptObject;
using UnityEngine;

namespace CardEffects
{
    [CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Effect/DrawCardEffect", order = 0)]
    public class DrawCardEffect : CardEffect
    {
        public IntEventSO drawCardEvent;
        public override void Excute(CharacterBase from, CharacterBase target)
        {
            drawCardEvent.RaiseEvent(value,this);
        }
    }
}
