using System;
using CardEffects;
using Character;
using Unity.Mathematics;
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
        private VisualElement buffElement;
        private Label buffRound;
        private VisualElement intentAction;
        private Label intentAmount;
        private Enemy enemy;
        [Header("Spirte")]
        public Sprite buffIcon;
        public Sprite debuffIcon;

        private void OnEnable()
        {
            currentChararcterHP = GetComponent<CharacterBase>();
            enemy = GetComponent<Enemy>();
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
            buffElement = healthBarDocument.rootVisualElement.Q<VisualElement>("Buff");
            buffRound = buffElement.Q<Label>("BuffAmount");
            intentAction = healthBarDocument.rootVisualElement.Q<VisualElement>("Intent");
            intentAmount = intentAction.Q<Label>("IntentAmount");
            buffElement.style.display = DisplayStyle.None;
            intentAction.style.display = DisplayStyle.None;
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
            buffElement.style.display=currentChararcterHP.buffRound.currentValue>0?DisplayStyle.Flex:DisplayStyle.None;
            buffRound.text=currentChararcterHP.buffRound.currentValue.ToString();
            buffElement.style.backgroundImage=currentChararcterHP.baseStrenth>1?new StyleBackground(buffIcon):new StyleBackground(debuffIcon);
        }
        /// <summary>
        /// 玩家回合调用
        /// </summary>
        public void UpdateIntentAction()
        {
            intentAction.style.display = DisplayStyle.Flex;
            intentAction.style.backgroundImage = new StyleBackground(enemy.currentEnemyAction.intentSprite);
            var value = enemy.currentEnemyAction.effect.value;
            if (enemy.currentEnemyAction.effect.GetType()==typeof(DamageEffect))
            {
                value = (int)math.round(enemy.currentEnemyAction.effect.value * enemy.baseStrenth);
            }

            intentAmount.text = value.ToString();
        }

        public void HideIntentAction()
        {
            intentAction.style.display = DisplayStyle.None;
        }
    }
}
