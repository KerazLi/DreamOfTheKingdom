using System.Collections.Generic;
using CardEffects;
using UnityEngine;

namespace EnemyAction
{
    [CreateAssetMenu(fileName = "EnemyActionDataSO", menuName = "EnemyAction/EnemyActionDataSO", order = 0)]
    public class EnemyActionDataSO : ScriptableObject
    {
        public List<EnemyAction> actions;
    }

    [System.Serializable]
    public struct EnemyAction
    {
        public Sprite intentSprite;
        public CardEffect effect;
    }
}
