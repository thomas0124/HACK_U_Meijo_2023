using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPresenter : MonoBehaviour
{
    [SerializeField]
    private List<Button> _joinButton;
    [HideInInspector]
    public List<Button> joinButton;
    [SerializeField]
    private List<Button> _leaveButton;
    [SerializeField]
    private JoinRoomPun _joinRoom;
    [SerializeField]
    private LeaveRoomPun _leaveRoom;
    private void Awake()
    {
        foreach (Button button in _joinButton)
        {
            button.onClick.AddListener(() =>
            {
                _joinRoom.JoinRoom();
            });
        }
        foreach (Button button in _leaveButton)
        {
            button.onClick.AddListener(() => 
            {
                _leaveRoom.LeaveRoom();
            });
        }
    }
    public void makeJoinRoomButton()
    {
        foreach (Button button in joinButton)
        {
            button.onClick.AddListener(() =>
            {
                _joinRoom.JoinRoom();
            });
        }
    }
}
