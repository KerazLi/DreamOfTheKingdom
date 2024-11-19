using System.Collections.Generic;
using UnityEngine;
using Utilities;
using CardEffects;


namespace Cards.ScriptObject
{
    [CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO", order = 0)]
    public class CardDataSO : ScriptableObject
    {
       public string cardName;
       public Sprite cardImage;
       public int cost;
       public CardType cardType;
       [TextArea]
       public string cardDescription;
       //执行效果
       public List<CardEffect> CardEffects;
    }
}

