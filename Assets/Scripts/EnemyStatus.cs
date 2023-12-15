using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//testコードなので消してもいいです。

public class EnemyStatus : MonoBehaviourPunCallbacks
{
    private PhotonView photonViewControl;

    private int hp = 200;
    private int Enemy_hp = 100;
    private void Awake()
    {
        photonViewControl = GetComponent<PhotonView>();
    }
    private void Start()
    {
        SendStatus(hp);
        StartCoroutine(enumerator());
    }
    public void SendStatus(int hp)
    {
        photonViewControl.RPC(nameof(SetEnemyStatus), RpcTarget.Others, 200);
    }
    [PunRPC]
    private void SetEnemyStatus(int hp)
    {
        Enemy_hp = hp;
    }
    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(5.0f);
    }
}
