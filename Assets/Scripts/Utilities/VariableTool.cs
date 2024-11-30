using UnityEngine;

namespace Utilities
{
    public class VariableTool : MonoBehaviour
    {
        public static readonly int Hit = Animator.StringToHash("hit");
        public static readonly int IsDead = Animator.StringToHash("isDead");
        public static readonly int IsSleep = Animator.StringToHash("isSleep");
        public static readonly int IsParry = Animator.StringToHash("isParry");
        public static readonly int Attack = Animator.StringToHash("attack");
        public static readonly int Skill = Animator.StringToHash("skill");
    }
}