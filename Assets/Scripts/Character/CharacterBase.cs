using System;
using Event.ScriptObject;
using UnityEngine;
using Utilities;
using Variable;

namespace Character
{
    public class CharacterBase : MonoBehaviour
    {
        public int maxHp;
        protected Animator animator;
        public IntVariable hp;
        public IntVariable buffRound;
        public IntVariable defense;
        public GameObject buff, debuff;
        public float baseStrenth = 1f;
        public float strengthEffect = 0.5f;
        public bool isDead;
        public int CurrentHP { get => hp.currentValue;set => hp.SetValue(value);}
        [Header("广播")] public ObjectEventSO characterDeadEvent;

        private int MaxHP
        {
            get => hp.maxValue;
        }

        

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            defense.currentValue = 0;
        }
        protected virtual void Start()
        {
            hp.maxValue = maxHp;
            CurrentHP = MaxHP;
            buffRound.currentValue = 0;
        }

        

        /// <summary>
        /// 角色受到伤害时调用的方法。
        /// </summary>
        /// <param name="damage">攻击者对角色造成的伤害值。</param>
        public virtual void TakeDamage(int damage)
        {
            if (damage>defense.currentValue)
            {
                damage -= defense.currentValue;
                defense.currentValue = 0;
            }else
            {
                defense.currentValue-=damage;
                damage = 0;
            }
            /*// 计算实际伤害，确保伤害值不会小于0
            var currentDamage = (damage - defense.currentValue)>=0?(damage - defense.currentValue):0;
            // 更新防御力，确保防御力不会小于0
            defense.currentValue = (defense.currentValue - currentDamage)>=0?0:(defense.currentValue - currentDamage);*/
            if (CurrentHP>damage)
            {
                // 如果当前生命值大于所受伤害，减少生命值并触发受击动画
                CurrentHP -= damage;
                Debug.Log("Damge"+damage);
                animator.SetTrigger("hit");
                Debug.Log("CurrentHP"+CurrentHP);
            }else
            {
                // 如果当前生命值不足以承受伤害，将生命值设为0，并标记角色为死亡状态
                CurrentHP = 0;
                isDead = true;
                animator.SetBool("isDead", isDead);
                characterDeadEvent.RaiseEvent(this,this);
            }
            
        }
        

        public void UpdateDefense(int amount)
        {
            var value = defense.currentValue + amount;
            defense.SetValue(value);
        }

        public void RestDefense()
        {
            defense.SetValue(0);
        }

        public void HealHealth(int amount)
        {
            CurrentHP += amount;
            CurrentHP=Math.Min(CurrentHP,MaxHP);
            buff.SetActive(true);
        }

        public void SetupStrenth(int round, bool isPositive)
        {
            if (isPositive)
            {
                float newStrenth = baseStrenth + strengthEffect;
                baseStrenth = Mathf.Min(newStrenth, 1.5f);
                buff.SetActive(true);
            }
            else
            {
                debuff.SetActive(true);
                baseStrenth=1-strengthEffect;
            }

            var currentRound = buffRound.currentValue + round;
            buffRound.SetValue(Mathf.Approximately(baseStrenth, 1) ? 0 : currentRound);
        }

        public void UpdateStrengthRound()
        {
            buffRound.SetValue(buffRound.currentValue - 1);
            if (buffRound.currentValue<=0)
            {
                buffRound.SetValue(0);
                baseStrenth = 1;
            }
        }
    }
}
