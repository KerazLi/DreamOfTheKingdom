using Event.ScriptObject;
using UnityEngine;

namespace Rooms.Mono
{
    public class FinishRoom : MonoBehaviour
    {
        public ObjectEventSO loadMapEvent;
        private void OnMouseDown()
        {
            //返回地图
            loadMapEvent.RaiseEvent(null,this);
        }
    }
}