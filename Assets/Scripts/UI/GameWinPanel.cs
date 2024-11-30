using System;
using Event.ScriptObject;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameWinPanel : MonoBehaviour
    {
        private VisualElement rootElement;
        private Button pickCardButton;
        private Button backToMapButton;
        [Header("事件广播")]
        public ObjectEventSO pickCardEvent;
        public ObjectEventSO backToMapEvent;

        private void Awake()
        {
            rootElement = GetComponent<UIDocument>().rootVisualElement;
            pickCardButton = rootElement.Q<Button>("PickCardButton");
            backToMapButton = rootElement.Q<Button>("BackToMapButton");
            backToMapButton.clicked += OnBackToMapButtonClicked;
            pickCardButton.clicked += OnPickCardButtonClicked;
        }

        private void OnPickCardButtonClicked()
        {
            pickCardEvent.RaiseEvent(null,this);
        }

        private void OnBackToMapButtonClicked()
        {
            backToMapEvent.RaiseEvent(null,this);
        }
    }
}
