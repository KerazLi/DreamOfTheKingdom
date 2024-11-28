using System;
using System.Collections.Generic;
using Event.ScriptObject;
using UnityEngine;
using Utilities;

public class Room : MonoBehaviour
{
    //列
    public  int column;

    //行
    public  int line;
    public RoomDataSO roomData;
    public RoomState roomState;
    private SpriteRenderer _spriteRenderer;
    public List<Vector2Int> linkToRooms = new ();
    
    [Header("广播")]
    public ObjectEventSO loadRoomEvent;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    /// <summary>
    /// 测试代码
    /// </summary>
    private void Start()
    {
        //SetupRoom(0, 0, roomData);
    }

    private void OnMouseDown()
    {
        //Debug.Log("Room clicked" + roomData.roomType);
        if (roomState==RoomState.Attainable)
        {
            loadRoomEvent.RaiseEvent(this, this);
        }
    }

    /// <summary>
    /// 设置房间的属性和图标。
    /// </summary>
    /// <param name="column">房间所在的列号。</param>
    /// <param name="line">房间所在的行号。</param>
    /// <param name="roomData">房间的数据，包括图标等信息。</param>
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        // 将输入的列号赋值给类成员变量column
        this.column = column;
        // 将输入的行号赋值给类成员变量line
        this.line = line;
        // 将输入的房间数据赋值给类成员变量roomData
        this.roomData = roomData;
        // 设置房间的图标为房间数据中提供的图标
        _spriteRenderer.sprite = roomData.roomIcon;

        // 根据房间状态设置精灵渲染器的颜色
        _spriteRenderer.color = roomState switch
        {
            // 当房间状态为锁定时，设置颜色为黑色
            RoomState.Locked => new(0.0f,0.0f,0.0f,1f),
            // 当房间状态为已访问时，同样设置颜色为黑色
            RoomState.Visited => new(0.0f,0.0f,0.0f,1f),
            // 当房间状态为可到达时，设置颜色为白色
            RoomState.Attainable => Color.white,
            // 如果房间状态不在预期范围内，抛出异常
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
