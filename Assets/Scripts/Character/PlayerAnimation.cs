using System;
using Cards.Mono;
using UnityEngine;
using Utilities;

namespace Character
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int IsSleep = Animator.StringToHash("isSleep");
        private static readonly int IsParry = Animator.StringToHash("isParry");
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int Skill = Animator.StringToHash("skill");
        private Player player;
        private Animator animator;

        private void Awake()
        {
            player = GetComponent<Player>();
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            animator.Play(IsSleep);
            animator.SetBool(IsSleep,true);
        }

        public void PlayerTurnBeginAnimation()
        {
            animator.SetBool(IsSleep,false);
            animator.SetBool(IsParry,false);
        }
        public void PlayerTurnEndAnimation()
        {
            animator.SetBool(player.defense.currentValue > 0 ? IsParry : IsSleep, true);
            Debug.Log(IsParry);
            Debug.Log(IsSleep);
        }

        public void OnPlayCardEvent(object obj)
        {
            Card card=obj as Card;
            switch (card.cardData.cardType)
            {
                case CardType.Attack:
                    animator.SetTrigger(Attack);
                    break;
                case CardType.Defense:
                case CardType.Skill:
                    animator.SetTrigger(Skill);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
