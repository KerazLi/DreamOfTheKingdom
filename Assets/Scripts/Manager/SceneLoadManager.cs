using System;
using Event.ScriptObject;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneLoadManager : MonoBehaviour
    {
        public FadePanel fadePanel;
        private AssetReference currentScene;
        public AssetReference map;
        public AssetReference menu;
        private Vector2Int currentRoomVector;
        private Room currentRoom;
        [Header("广播")]
        public ObjectEventSO afterRoomLoadedEvent;

        public ObjectEventSO updateRoomEvent;

        private void Start()
        {
            
            currentRoomVector = Vector2Int.one*-1;
            LoadMenu();
        }

        /// <summary>
        /// 处理加载房间事件时的行为
        /// </summary>
        /// <param name="data">传递进来的数据，可以是任何类型，但预期是RoomDataSO类型</param>
        public async void OnLoadRoomEvent(object data)
        {
            // 检查传入的数据是否为RoomDataSO类型，以确保类型安全
            if (data is Room)
            {
                 currentRoom = data as Room;
                // 将data转换为RoomDataSO类型，以便访问其属性
                var currentData = currentRoom.roomData;
                
                currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.line);
                
                currentScene = currentData.sceneToLoad;
            }
            //卸载房间
            await UnloadSceneTask();
            //加载房间
            await  LoadSceneTask();
            afterRoomLoadedEvent.RaiseEvent(currentRoom,this);
        }
    
        /// <summary>
        /// 异步加载场景任务。
        /// </summary>
        /// <returns>返回一个可等待的任务。</returns>
        private async Awaitable LoadSceneTask()
        {
            // 启动当前场景的异步加载任务，使用 additive 模式加载场景，以便将新场景与当前场景合并
            var s=currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        
            // 等待异步加载任务完成
            await s.Task;
        
            // 检查加载任务是否成功完成
            if (s.Status==AsyncOperationStatus.Succeeded)
            {
                //淡出
                fadePanel.FadeOut(0.2f);
                // 如果加载成功，则将新加载的场景设置为活动场景
                SceneManager.SetActiveScene(s.Result.Scene);
            }
        }
    
        /// <summary>
        /// 异步卸载当前激活的场景。
        /// </summary>
        /// <returns>返回一个Awaitable对象，用于等待场景卸载完成。</returns>
        private async Awaitable UnloadSceneTask()
        {
            //淡出
            fadePanel.FadeIn(0.4f);
            await Awaitable.WaitForSecondsAsync(0.45f);
            // 使用场景管理器的异步方法卸载当前激活的场景
            await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()) ?? throw new InvalidOperationException("Unable to unload scene"));   
        }

        /// <summary>
        /// 异步加载地图场景。
        /// </summary>
        /// <returns>无返回值。</returns>
        public async void LoadMap()
        {
            // 等待当前场景卸载完成，以避免场景间的冲突。
            await UnloadSceneTask();
            if (currentRoomVector!=Vector2.one*-1)
            {
                updateRoomEvent.RaiseEvent(currentRoomVector,this);
            }
        
            // 设置当前场景为地图，准备加载新的地图场景。
            currentScene = map;
        
            // 等待新场景加载完成，确保场景切换的流畅性。
            await LoadSceneTask();
        }
        public async void LoadMenu()
        {
            if (currentScene!=null)
            {
                await UnloadSceneTask();
            }
            // 等待当前场景卸载完成，以避免场景间的冲突。
            //await UnloadSceneTask();
        
            // 设置当前场景为地图，准备加载新的地图场景。
            currentScene = menu;
        
            // 等待新场景加载完成，确保场景切换的流畅性。
            await LoadSceneTask();
        }

    }
}
