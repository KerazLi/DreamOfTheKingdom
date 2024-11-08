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
    
    public CardDataSO cardData;

    private void Start()
    {
        InintCardData(cardData);
    }

    public void InintCardData(CardDataSO cardData)
    {
        //cardData = data;
        cardSprite.sprite = cardData.cardImage;
        costText.text = cardData.cost.ToString();
        descriptionText.text = cardData.cardDescription;
        typeText.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Skill => "技能",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

}
