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
            animator.Play( VariableTool.IsSleep);
            animator.SetBool(VariableTool.IsSleep,true);
        }
        

        public void PlayerTurnBeginAnimation()
        {
            animator.SetBool(VariableTool.IsSleep,false);
            animator.SetBool(VariableTool.IsParry,false);
        }
        public void PlayerTurnEndAnimation()
        {
            animator.SetBool(player.defense.currentValue > 0 ? VariableTool.IsParry : VariableTool.IsSleep, true);
            Debug.Log(VariableTool.IsParry);
            Debug.Log(VariableTool.IsSleep);
        }

        public void OnPlayCardEvent(object obj)
        {
            Card card=obj as Card;
            switch (card.cardData.cardType)
            {
                case CardType.Attack:
                    animator.SetTrigger(VariableTool.Attack);
                    break;
                case CardType.Defense:
                case CardType.Skill:
                    animator.SetTrigger(VariableTool.Skill);
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
