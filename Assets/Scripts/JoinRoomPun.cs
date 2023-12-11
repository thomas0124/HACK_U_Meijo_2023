using Photon.Pun;
using UnityEngine;

public class JoinRoomPun : MonoBehaviourPunCallbacks
{
    public void JoinRoom()
    {
        Debug.Log("JoinRoom");
        PhotonNetwork.JoinRandomRoom();
    }
}
