using UnityEngine;
using UnityEngine.Events;

public class EventSO<T> : ScriptableObject
{
    // 使用TextArea属性，指示description字段在Inspector中显示为多行文本框
    [TextArea] 
    // description: 事件的描述信息，用于提供事件的详细说明
    public string description;
    
    // OnEventRaised: 当事件被触发时调用的委托，允许其他对象订阅此事件以接收通知
    public UnityAction<T> OnEventRaised;
    
    // lastSender: 记录最后一次触发事件的对象的字符串表示，用于追踪事件来源
    public string lastSender;
    
    /// <summary>
    /// 触发事件并通知所有订阅者。
    /// </summary>
    /// <param name="value">事件的值，类型为T，传递给事件订阅者。</param>
    /// <param name="sender">事件的发送者，记录其字符串表示以追踪事件来源。</param>
    public void RaiseEvent(T value,object sender)
    {
        // 更新最后一次事件发送者的字符串表示
        lastSender = sender.ToString();
        // 如果有订阅者，则调用他们，传递事件值
        OnEventRaised?.Invoke(value);
    }
}
