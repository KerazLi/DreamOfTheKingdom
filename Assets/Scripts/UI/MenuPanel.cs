using System;
using Event.ScriptObject;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MenuPanel : MonoBehaviour
    {
        private VisualElement rootElement;
        private Button newGameButton,quitButton;
        public ObjectEventSO newGameEvent;

        private void OnEnable()
        {
            rootElement = GetComponent<UIDocument>().rootVisualElement;
            newGameButton = rootElement.Q<Button>("NewGameButton");
            quitButton = rootElement.Q<Button>("QuitButton");
            newGameButton.clicked+=OnNewGameButtonClicked;
            quitButton.clicked+=OnQuitButtonClicked;
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }

        private void OnNewGameButtonClicked()
        {
            newGameEvent.RaiseEvent(null,this);
        }
    }
}
