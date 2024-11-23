using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class HealthBarController : MonoBehaviour
    {
        [Header("Elements")]
        public Transform HealthBarTransform;

        private UIDocument healthBarDocument;

        private ProgressBar healthBar;

        private CharacterBase currentChararcterHP;
        private VisualElement defenseElement;
        private Label defenseAmountLabel;

        private void Awake()
        {
            currentChararcterHP = GetComponent<CharacterBase>();
        }

        private void Start()
        {
            InitCharacterHealthBar();
        }

        private void MoveToWorldPosition(VisualElement element,Vector3 worldPosition,Vector2 size)
        {
            Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);
            element.transform.position = rect.position;
        }
        
        [ContextMenu("UpdateHealthBar")]
        public void InitCharacterHealthBar()
        {
            healthBarDocument = GetComponent<UIDocument>();
            healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
            defenseElement= healthBarDocument.rootVisualElement.Q<VisualElement>("Defense");
            defenseAmountLabel = defenseElement.Q<Label>("DefenseAmount");
            healthBar.highValue = currentChararcterHP.maxHp;
            MoveToWorldPosition(healthBar, HealthBarTransform.position,Vector2.zero);
        }
        
        private void Update()
        {
            UpdateHealthBar();
        }

        void UpdateHealthBar()
        {
            if (currentChararcterHP.isDead)
            {
                healthBar.style.display = DisplayStyle.None;
                return;
            }

            if (healthBar!=null)
            {
                healthBar.title = $"{currentChararcterHP.CurrentHP}/{currentChararcterHP.maxHp}";
                healthBar.value=currentChararcterHP.CurrentHP;
                healthBar.RemoveFromClassList("highHealth");
                healthBar.RemoveFromClassList("lowHealth");
                healthBar.RemoveFromClassList("middleHealth");
                healthBar.AddToClassList(currentChararcterHP.CurrentHP<=currentChararcterHP.maxHp/3?"lowHealth":currentChararcterHP.CurrentHP<=currentChararcterHP.maxHp/2?"middleHealth":"highHealth");
            }
            defenseElement.style.display=currentChararcterHP.defense.currentValue>0?DisplayStyle.Flex:DisplayStyle.None;
            defenseAmountLabel.text=currentChararcterHP.defense.currentValue.ToString();
            
            
            
        }
    }
}