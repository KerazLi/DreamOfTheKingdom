using System;
using Cards.ScriptObject;
using TMPro;
using UnityEngine;
using Utilities;

public class Card : MonoBehaviour
{
    [Header("组件")] 
    public SpriteRenderer cardSprite;

    public TextMeshPro costText, descriptionText, typeText;
    
    [HideInInspector]public CardDataSO cardData;

    /*private void Start()
    {
        InintCardData(cardData);
    }*/

    public void InintCardData(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        descriptionText.text = data.cardDescription;
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Skill => "技能",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

}
