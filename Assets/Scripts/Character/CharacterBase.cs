using System;
using UnityEngine;
using Variable;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    protected Animator animator;
    public IntVariable hp;
    public int CurrentHP { get => hp.currentValue;set => hp.SetValue(value);}

    private int MaxHP
    {
        get => hp.maxValue;
    }

    private bool isDead;

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
        if (CurrentHP>damage)
        {
            CurrentHP -= damage;
            Debug.Log("CurrentHP"+CurrentHP);
        }else
        {
            CurrentHP = 0;
            isDead = true;
        }
    }
}
