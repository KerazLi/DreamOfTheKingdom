using Event.ScriptObject;
using UnityEngine;

namespace Variable
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        public int maxValue;
        public int currentValue;
        public IntEventSO ValueChangedEvent;
        [TextArea]
        [SerializeField]private string description;

        public void SetValue(int value)
        {
            currentValue = value;
            ValueChangedEvent?.RaiseEvent(currentValue, this);
        }
    }
}
