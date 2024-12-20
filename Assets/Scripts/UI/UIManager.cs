using System;
using UnityEngine;
using Utilities;

public class UIManager : MonoBehaviour
{
    [Header("面板")] 
    public GameObject gamePlayPanel;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject pickCardPanel;
    public GameObject RestRoomPanel;

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;
        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.EliteEnemy:
            case RoomType.Boss:
                gamePlayPanel.SetActive(true);
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                gameWinPanel.SetActive(true);
                break;
            case RoomType.RestRoom:
                RestRoomPanel.SetActive(true);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void HideAllPanel()
    {
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        pickCardPanel.SetActive(false);
        RestRoomPanel.SetActive(false);
    }
    public void OnGameOverEvent()
    {
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }
    public void OnGameWinEvent()
    {
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }
    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
        Debug.Log("打开选择卡牌的界面");
    }
    public void OnPickCardFinishEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
