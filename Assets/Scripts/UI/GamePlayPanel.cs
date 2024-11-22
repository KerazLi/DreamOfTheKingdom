using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Label energyAmountLabel, drawAmountLabel, discardAmountLabel,turnLabel;
    private Button endTurnButton;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        energyAmountLabel = rootElement.Q<Label>("EnergyAmount");
        drawAmountLabel = rootElement.Q<Label>("DrawAmount");
        discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
        endTurnButton = rootElement.Q<Button>("EndTurn");
        turnLabel = rootElement.Q<Label>("TurnLabel");

        drawAmountLabel.text = "0";
        discardAmountLabel.text = "0";
        energyAmountLabel.text = "0";
        turnLabel.text = "游戏开始";
    }

    public void UpdateDrawDeckAmount(int amount)
    {
        drawAmountLabel.text = amount.ToString();
    }
    public void UpdateDiscardDeckAmount(int amount)
    {
        discardAmountLabel.text = amount.ToString();
    }
}
