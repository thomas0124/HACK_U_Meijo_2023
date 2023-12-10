using Photon.Pun;
using UnityEngine;

public class LeaveRoomPun : MonoBehaviourPunCallbacks
{
    public void LeaveRoom()
    {
        Debug.Log("LeaveRoom");
        PhotonNetwork.LeaveRoom();
    }
}
