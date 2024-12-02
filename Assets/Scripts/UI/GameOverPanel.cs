using Event.ScriptObject;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class GameOverPanel : MonoBehaviour
    {
        private Button backToStartButton;
        public ObjectEventSO loadMenuEvent;
        public ObjectEventSO GameOverEvent;
    

        private void OnEnable()
        {
            backToStartButton=GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton");
            GameOverEvent.RaiseEvent(null,this);
            backToStartButton.clicked+=BackToStart;
        }

        private void BackToStart()
        {
            loadMenuEvent.RaiseEvent(null,this);
        }
    }
}
