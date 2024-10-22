using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
[CustomEditor(typeof(EventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
   // 定义一个泛型事件对象，用于存储和管理事件
   private EventSO<T> baseEventSO;
   
   // 重写OnInspectorGUI方法，以在检查器窗口中显示自定义界面
   public override void OnInspectorGUI()
   {
       // 调用基类的OnInspectorGUI方法，确保默认行为得以执行
       base.OnInspectorGUI();

       foreach (var VARIABLE in GetListeners())
       {
           EditorGUILayout.LabelField(VARIABLE.ToString());//显示监视器的名称
       }
   }
   
   // 在脚本编辑器窗口启用时调用
   private void OnEnable()
   {
       // 检查baseEventSO是否为空，如果为空，则将其初始化为当前目标对象
       if (baseEventSO == null)
       {
           baseEventSO = target as EventSO<T>;
       }
   }
   
   // 获取所有监听该事件的MonoBehaviour对象的列表
   private List<MonoBehaviour> GetListeners()
   {
       // 初始化一个空的MonoBehaviour列表，用于存储事件监听器
       List<MonoBehaviour> listener = new();

       if (baseEventSO == null || baseEventSO.OnEventRaised == null)
       {
           return listener;
       }

       // 获取事件所有订阅者的引用
       var subscribers = baseEventSO.OnEventRaised.GetInvocationList();
       // 遍历所有订阅者
       foreach (var item in subscribers)
       {
           // 尝试将订阅者的Target转换为MonoBehaviour类型
           var mono = item.Target as MonoBehaviour;
           // 如果转换成功且不为空，则将该MonoBehaviour对象添加到监听器列表中
           if (mono != null)
           {
               listener.Add(mono);
           }
       }
       // 返回包含所有监听器的列表
       return listener;
   }
}
