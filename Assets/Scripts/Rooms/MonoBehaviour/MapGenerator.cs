using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour
{
    // 使用 FormerlySerializedAs 属性标记变量，以便与之前序列化时保持兼容
    // 该变量保存了地图配置的单例对象
    [Header("地图配置表")] public MapConfigSo mapConfigSo;

    [Header("地图设置")] public MapLayoutSO mapLayout;

    // 房间预设对象，用于在地图生成时实例化房间
    [Header("预制体")] public Room roomPrefab;

    public LineRenderer linePrefab;

    // 边界区域的宽度，用于在地图边缘留出空白区域
    [Header("变量")] public float border;

    // 保存已经生成的房间实例的列表
    private readonly List<Room> _rooms = new();

    //保存房间之间的连线的列表
    private readonly List<LineRenderer> lines = new();


    public List<RoomDataSO> roomDataList = new();

    private Dictionary<RoomType, RoomDataSO> roomDataDict = new();

    // 每列房间的宽度，用于计算房间位置和布局
    private float _columnWidth;

    // 用于存储生成房间时的位置信息
    private Vector3 _generatedPosition;

    // 用于存储屏幕高度的私有变量
    private float _screenHeight;

    // 用于存储屏幕宽度的私有变量
    private float _screenWidth;

    /// <summary>
    /// 初始化屏幕宽高和列宽度。
    /// </summary>
    private void Awake()
    {
        // 计算屏幕高度，基于主相机的正交投影大小乘以2
        if (Camera.main != null)
        {
            _screenHeight = Camera.main.orthographicSize * 2;

            // 计算屏幕宽度，基于屏幕高度和相机的宽高比
            _screenWidth = _screenHeight * Camera.main.aspect;
        }
        else
        {
            throw new Exception("Camera.main is null");
        }

        // 计算每列的宽度，将屏幕宽度分成房间蓝图数量加1份
        _columnWidth = _screenWidth / (mapConfigSo.roomBluePrints.Count + 1);

        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }

    private void OnEnable()
    {
        if (mapLayout.mapRoomDatasList.Count >0)
        {
            LoadMapConfig();
        }else
        {
            GreateMap();
        }
    }


    /*private void Start()
    {
        GreateMap();
    }*/

    /// <summary>
    /// 生成地图
    /// </summary>
    public void GreateMap()
    {
        //创建前一列的房间
        List<Room> prevoiusRooms = new();

        // 遍历地图配置中的每个房间蓝图
        for (var column = 0; column < mapConfigSo.roomBluePrints.Count; column++)
        {
            // 获取当前遍历到的房间蓝图
            var roomBluePrint = mapConfigSo.roomBluePrints[column];

            // 随机决定当前房间蓝图的房间数量
            var amount = Random.Range(roomBluePrint.min, roomBluePrint.max);

            // 计算房间起始高度
            var startHeight = _screenHeight / 2 - _screenHeight / (amount + 1);

            // 初始化房间生成位置
            _generatedPosition = new Vector3(-_screenWidth / 2 + border + _columnWidth * column, startHeight, 0);

            // 设置房间的起始位置
            var newPosition = _generatedPosition;

            // 计算房间之间的垂直间距
            var roomGapY = _screenHeight / (amount + 1);

            //创建当前列的房间列表
            List<Room> currentRooms = new();

            // 根据随机决定的房间数量生成房间
            for (var i = 0; i < amount; i++)
            {
                // 随机偏移房间的位置
                /*if (column==mapConfigSo.roomBluePrints.Count-1)
                {
                    newPosition.x = _screenWidth / 2 - 1.2f * 2;
                }else if (column!=0)
                {
                    newPosition.x=_generatedPosition.x+Random.Range(-1.2f/2,1.2f/2);
                }*/

                // 计算当前房间的实际位置
                newPosition.y = startHeight - roomGapY * i;

                // 在计算出的位置实例化房间对象，并将其添加到当前变换下
                var room = Instantiate(roomPrefab, newPosition, quaternion.identity, this.transform);
                RoomType newType = GetRandomRoomType(mapConfigSo.roomBluePrints[column].roomType);
                if (column==0)
                {
                    room.roomState = RoomState.Attainable;
                }
                else
                {
                    room.roomState = RoomState.Locked;
                }

                // 设置房间属性
                room.SetupRoom(column, i, roomDataDict[newType]);

                // 将生成的房间添加到房间列表中
                _rooms.Add(room);

                // 将生成的房间添加到当前列的房间列表中
                currentRooms.Add(room);
            }

            //判断当前列是否为第一列，如果不是则生成房间之间的连线
            if (prevoiusRooms.Count > 0)
            {
                //创建房间连线
                CreateConnnection(prevoiusRooms, currentRooms);
            }

            prevoiusRooms = currentRooms;
        }
        SaveMapConfig();
    }

    /// <summary>
    /// 房间连线
    /// </summary>
    /// <param name="column1">第一列</param>
    /// <param name="column2">第二列</param>
    private void CreateConnnection(List<Room> column1, List<Room> column2)
    {
        HashSet<Room> connectionColumn2Rooms = new();

        foreach (var room in column1)
        {
            //随机选择一个房间作为连线目标
            var targetRoom = ConnectionToRandomRoom(room, column2,false);

            //将目标房间添加到已连接房间集合中
            connectionColumn2Rooms.Add(targetRoom);
        }

        foreach (var room in column2)
        {
            //如果房间已经存在，则跳过
            if (connectionColumn2Rooms.Contains(room)) continue;

            //随机选择一个房间作为连线目标
            var targetRoom = ConnectionToRandomRoom(room, column1,true);
        }
    }

    /// <summary>
    ///  </summary>随机连线
                                                                  ///
    /// <param name="room">房间连线的起始点</param>
    /// <param name="column2">房间连线的终点</param>
    /// <returns></returns>
    private Room ConnectionToRandomRoom(Room room, List<Room> column2,bool check)
    {
        Room targetRoom = column2[Random.Range(0, column2.Count)];
        if (check)
        {
            targetRoom.linkToRooms.Add(new(room.column,room.line));
        }
        else
        {
            room.linkToRooms.Add(new(targetRoom.column,targetRoom.line));
        }

        //创建房间的连线
        var line = Instantiate(linePrefab, room.transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);

        lines.Add(line);
        return targetRoom;

    }

    /// <summary>
    ///重新生成房间
    /// </summary>
    [ContextMenu("ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        // 销毁所有现有房间对象
        foreach (var room in _rooms) Destroy(room.gameObject);

        //销毁所有房间连线
        foreach (var line in lines) Destroy(line.gameObject);

        // 清空房间 连线列表
        _rooms.Clear();
        lines.Clear();

        // 重新生成地图和连线
        GreateMap();
    }

    /// <summary>
    /// 根据房间类型获取随机房间数据
    /// </summary>
    /// <param name="roomType">房间类型</param>
    /// <returns>对应房间类型的房间数据</returns>
    private RoomDataSO GetRandomRoomData(RoomType roomType)
    {
        // 从字典中根据房间类型获取对应的房间数据
        return roomDataDict[roomType];
    }


    /// <summary>
    /// 从给定的房间类型标志中随机选择一种房间类型
    /// </summary>
    /// <param name="flags">包含多种可能房间类型的标志</param>
    /// <returns>随机选择的一种房间类型</returns>
    private RoomType GetRandomRoomType(RoomType flags)
    {
        // 将房间类型标志按逗号分隔，转换为字符串数组
        string[] options = flags.ToString().Split(',');

        // 随机选择一个房间类型选项
        string randomOption = options[Random.Range(0, options.Length)];

        // 将字符串的房间类型转换为RoomType枚举
        RoomType roomType = (RoomType)Enum.Parse(typeof(RoomType), randomOption);

        // 返回随机选择的房间类型
        return roomType;
    }

    /// <summary>
    /// 保存地图配置，包括房间布局和线条位置信息
    /// </summary>
    private void SaveMapConfig()
    {
        // 初始化地图房间数据列表和线条位置列表
        mapLayout.mapRoomDatasList = new();
        mapLayout.linePositions = new();

        // 遍历所有房间，将每个房间的数据添加到地图房间数据列表中
        foreach (var room in _rooms)
        {
            mapLayout.mapRoomDatasList.Add(new MapRoomData
            {
                column = room.column,
                line = room.line,
                posX = room.transform.position.x,
                posY = room.transform.position.y,
                roomData = room.roomData,
                roomState = room.roomState,
                linkToRooms = room.linkToRooms
            });
        }

        // 遍历所有线条，将每条线条的位置信息添加到线条位置列表中
        foreach (var line in lines)
        {
            mapLayout.linePositions.Add(new LinePosition
            {
                startPos = new SerializeVector3(line.GetPosition(0)),
                endPos = new SerializeVector3(line.GetPosition(1))
            });
        }
    
    }

    public void LoadMapConfig()
    {
        foreach (var mapRoomData in mapLayout.mapRoomDatasList)
        {
            var newPosition = new Vector3(mapRoomData.posX, mapRoomData.posY);
            var newRoom = Instantiate(roomPrefab, newPosition, Quaternion.identity);
            newRoom.roomState = mapRoomData.roomState;
            newRoom.SetupRoom(mapRoomData.column, mapRoomData.line, mapRoomData.roomData);
            newRoom.linkToRooms = mapRoomData.linkToRooms;
            _rooms.Add(newRoom);
        }

        foreach (var linePosition in mapLayout.linePositions)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, linePosition.startPos.ToVector3());
            line.SetPosition(1, linePosition.endPos.ToVector3());
            lines.Add(line);
        }
    }

}