using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GetEnemyCard : MonoBehaviourPunCallbacks
{
    public SpriteSpritSerializer spriteSpritSerializer;
    int count = -1;
    byte[][] getByte;
    int j;
    public Sprite enemySprite;
    public void SendSprite(Sprite sprite)
    {
        byte[] a = spriteSpritSerializer.SerializeSprite(sprite);
        byte[][] b = spriteSpritSerializer.SendSpritBytes(a);
        Debug.Log(b);
        Debug.Log(b.Length);
        photonView.RPC(nameof(SetCount), RpcTarget.Others, b.Length);
        for (int i = 0; i < b.Length; i++)
        {
            photonView.RPC(nameof(SendByte), RpcTarget.Others, b[i]);
        }
    }

    [PunRPC]
    void SetCount(int length)
    {
        count = length;
        getByte = new byte[count][];
    }
    [PunRPC]
    void SendByte(byte[] c)
    {
        getByte[j] = c;
        j++;
    }

    private void Update()
    {
        if(count == j)
        {
            byte[] d = spriteSpritSerializer.GetSpritBytes(getByte, count);
            if (spriteSpritSerializer.GetSpritBytes(getByte, count) == null) return;
            enemySprite = (Sprite)spriteSpritSerializer.DeserializeSprite(d);
        }
    }
}
