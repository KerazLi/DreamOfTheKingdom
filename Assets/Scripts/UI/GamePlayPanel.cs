using Event.ScriptObject;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GamePlayPanel : MonoBehaviour
    {
        [Header("事件广播")] public ObjectEventSO playerTurnEndEvent;
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
            endTurnButton.clicked += OnEndTurnButtonClick;
            drawAmountLabel.text = "0";
            discardAmountLabel.text = "0";
            energyAmountLabel.text = "0";
            turnLabel.text = "游戏开始";
        }

        private void OnEndTurnButtonClick()
        {
            playerTurnEndEvent.RaiseEvent(null, this);
        }

        public void UpdateDrawDeckAmount(int amount)
        {
            drawAmountLabel.text = amount.ToString();
        }
        public void UpdateDiscardDeckAmount(int amount)
        {
            discardAmountLabel.text = amount.ToString();
        }

        public void OnEnemyTurnBegin()
        {
            endTurnButton.SetEnabled(false);
            turnLabel.text = "敌方回合";
            turnLabel.style.color = Color.red;
        }
        public void OnPlayerTurnBegin()
        {
            endTurnButton.SetEnabled(true);
            turnLabel.text = "玩家回合";
            turnLabel.style.color = Color.white;
        }
        public void UpdateEnergyAmount(int amount)
        {
            energyAmountLabel.text = amount.ToString();
        }
    }
}
