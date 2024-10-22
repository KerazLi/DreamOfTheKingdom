using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
        /// <summary>
        /// 对应的事件对象。
        /// </summary>
        public EventSO<T> eventSO;
    
        /// <summary>
        /// 当事件被触发时执行的响应操作。
        /// </summary>
        public UnityEvent<T> response;
    
        /// <summary>
        /// 当组件启用时，注册事件处理函数。
        /// </summary>
        private void OnEnable()
        {
            if (eventSO!=null)
            {
                eventSO.OnEventRaised += OnEventRaised;
            }
        }
    
        /// <summary>
        /// 当组件禁用时，注销事件处理函数。
        /// </summary>
        private void OnDisable()
        {
            if (eventSO!=null)
            {
                eventSO.OnEventRaised -= OnEventRaised;
            }
        }
    
        /// <summary>
        /// 事件处理函数，当事件被触发时调用。
        /// </summary>
        /// <param name="value">事件传递的值。</param>
        private void OnEventRaised(T value)
        {
            response.Invoke(value);
        }
    
}
