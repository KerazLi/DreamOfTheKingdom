using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities;

[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Map/RoomData")]
public class RoomDataSO : ScriptableObject
{
    // 定义一个公共的 Sprite 变量，用于存储房间的图标
    public Sprite roomIcon;

    // 定义一个公共的 RoomType 变量，用于标识房间的类型
    public RoomType roomType;

    // 定义一个公共的 AssetReference 变量，存储需要加载的场景资源的引用
    public AssetReference sceneToLoad;
}
