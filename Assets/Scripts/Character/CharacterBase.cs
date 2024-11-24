using System;
using UnityEngine;
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
        public int CurrentHP { get => hp.currentValue;set => hp.SetValue(value);}

        private int MaxHP
        {
            get => hp.maxValue;
        }

        public bool isDead;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }
        protected virtual void Start()
        {
            hp.maxValue = maxHp;
            CurrentHP = MaxHP;
            buffRound.currentValue = 0;
        }

        public virtual void TakeDamage(int damage)
        {
            var currentDamage = (damage - defense.currentValue)>=0?(damage - defense.currentValue):0;
            var currentDefense = (defense.currentValue - currentDamage)>=0?0:(defense.currentValue - currentDamage);
            if (CurrentHP>currentDamage)
            {
                CurrentHP -= currentDamage;
                Debug.Log("CurrentHP"+CurrentHP);
            }else
            {
                CurrentHP = 0;
                isDead = true;
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
