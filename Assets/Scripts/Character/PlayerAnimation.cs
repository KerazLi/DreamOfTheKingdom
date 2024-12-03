using System;
using Cards.Mono;
using UnityEngine;
using Utilities;

namespace Character
{
    public class PlayerAnimation : MonoBehaviour
    {
        
        [HideInInspector]public Player player;
        private Animator animator;

        private void Awake()
        {
            player = GetComponent<Player>();
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            animator.Play("sleep");
            animator.SetBool("isSleep",true);
        }
        

        public void PlayerTurnBeginAnimation()
        {
            animator.SetBool("isSleep",false);
            animator.SetBool("isPerry",false);
        }
        public void PlayerTurnEndAnimation()
        {
            animator.SetBool(player.defense.currentValue > 0 ? "isParry": "isSleep", true);
            
        }

        public void OnPlayCardEvent(object obj)
        {
            Card card=obj as Card;
            switch (card.cardData.cardType)
            {
                case CardType.Attack:
                    animator.SetTrigger("attack");
                    break;
                case CardType.Defense:
                case CardType.Skill:
                    animator.SetTrigger("skill");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void SetSleepAnimation()
        {
            animator.Play("death");
        }
    }
}
