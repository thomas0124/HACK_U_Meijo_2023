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
        photonView.RPC(nameof(AfterKariLoading), RpcTarget.Others, true);
    }
    void CallbackSummonCard(bool index)
    {
        afterKariLoading.eComp = index;
    }
}
