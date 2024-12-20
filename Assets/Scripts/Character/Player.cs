using System;
using Cards.Mono;
using Unity.VisualScripting;
using Variable;

namespace Character
{
    public class Player : CharacterBase
    {
        public IntVariable playerMana;
        public int maxMana;
        public int CurrentMana { 
            get=> playerMana.currentValue;
            set => playerMana.SetValue(value);
        }

        private void OnEnable()
        {
            playerMana.maxValue = maxMana;
            CurrentMana = playerMana.maxValue;
        }

        public void NewTurn()
        {
            CurrentMana = maxMana;
        }

        public void UpdateMana(int cost)
        {
            CurrentMana -= cost;
            if (CurrentMana<=0)
            {
                CurrentMana = 0;
            }
        }

        public void NewGame()
        {
            CurrentHP = maxHp;
            isDead = false;
            buffRound.currentValue = 0;
            defense.currentValue = 0;
            NewTurn();
        }

    }
}
