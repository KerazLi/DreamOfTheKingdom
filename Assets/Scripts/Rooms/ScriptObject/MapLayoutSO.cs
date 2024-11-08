using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
public class MapLayoutSO : ScriptableObject
{
    public List<MapRoomData> mapRoomDatasList = new();
    public List<LinePosition> linePositions = new();
}

[System.Serializable]
public class MapRoomData
{
    public float posX, posY;
    public int column, line;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkToRooms;
}

[System.Serializable]
public class LinePosition
{
    public SerializeVector3 startPos, endPos;
}
