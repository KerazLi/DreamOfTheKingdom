using CardEffects;
using Character;
using Event.ScriptObject;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton, backMapButton;
    public CardEffect restEffect;
    private CharacterBase player;
    [Header("广播")]
    public ObjectEventSO loadMapEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>("RestButton");
        backMapButton = rootElement.Q<Button>("BackMapButton");
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);
        restButton.clicked += OnRestButtonClicked;
        backMapButton.clicked += OnBackMapButtonClicked;
    }

    private void OnBackMapButtonClicked()
    {
        player.GameObject().SetActive(false);
        loadMapEvent.RaiseEvent(null,this);
    }

    private void OnRestButtonClicked()
    {
        restEffect.Excute(player,null);
        restButton.SetEnabled(false);
    }
}
