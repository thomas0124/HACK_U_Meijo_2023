using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SendPhotonMessage : MonoBehaviourPunCallbacks
{
    AfterKariLoading afterKariLoading;
    private void Awake()
    {
        afterKariLoading = GetComponent<AfterKariLoading>();
    }
    public void SendComp()
    {
        photonView.RPC(nameof(CallbackSummonCard), RpcTarget.Others, true);
    }
    [PunRPC]
    void CallbackSummonCard(bool index)
    {
        afterKariLoading.eComp = index;
    }
    public void reconnect()
    {
        PhotonNetwork.ReconnectAndRejoin();
    }
}
