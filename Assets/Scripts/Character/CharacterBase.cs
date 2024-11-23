using System;
using UnityEngine;
using Variable;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    protected Animator animator;
    public IntVariable hp;
    public IntVariable defense;
    public GameObject buff, debuff;
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
}
