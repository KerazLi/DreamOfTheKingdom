using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    //列
    public int column;
    //行
    public int line;
    private SpriteRenderer _spriteRenderer;
    public RoomDataSo roomData;
    public RoomState roomState;

    private void Start()
    {
        SetupRoom(0,0,roomData);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Room clicked"+roomData.roomType);
    }

    /// <summary>
    /// 设置房间的属性和图标。
    /// </summary>
    /// <param name="column">房间所在的列号。</param>
    /// <param name="line">房间所在的行号。</param>
    /// <param name="roomData">房间的数据，包括图标等信息。</param>
    public void SetupRoom(int column, int line, RoomDataSo roomData)
    {
        // 将输入的列号赋值给类成员变量column
        this.column = column;
        // 将输入的行号赋值给类成员变量line
        this.line = line;
        // 将输入的房间数据赋值给类成员变量roomData
        this.roomData = roomData;
        // 设置房间的图标为房间数据中提供的图标
        _spriteRenderer.sprite = roomData.roomIcon;
    }

}
