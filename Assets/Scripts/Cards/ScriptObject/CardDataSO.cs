using UnityEngine;
using Utilities;

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
       //TODO:执行效果
    }
}