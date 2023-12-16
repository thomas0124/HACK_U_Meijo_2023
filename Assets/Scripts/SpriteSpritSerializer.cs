using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class SpriteSpritSerializer : MonoBehaviour
{

    public byte[] SerializeSprite(object customObject)
    {
        Sprite sprite = (Sprite)customObject;

        // バイト配列にシリアライズするためにTexture2Dを作成
        Texture2D texture = sprite.texture;

        // Texture2DをPNG形式のバイト配列に変換
        byte[] bytes = texture.EncodeToJPG(30);

        return bytes;
    }
    public byte[][] SendSpritBytes(byte[] bytes)
    {
        float countf = (float)bytes.Length / 50000f;
        int count = Mathf.CeilToInt(countf);
        byte[][] bytess = new byte[count][];
        Debug.Log(bytes.Length);
        Debug.Log(count);
        if(count == 1)
        {
            bytess[count - 1] = new byte[bytes.Length];
            Array.Copy(bytes, 0, bytess[count - 1], 0, bytes.Length);
        }
        else
        {
            for (int i = 0; i < count - 1; i++)
            {
                bytess[i] = new byte[50000];
                Array.Copy(bytes, 50000 * i, bytess[i], 0, 50000);
            }
            bytess[count - 1] = new byte[bytes.Length % 50000];
            Array.Copy(bytes, 50000 * (count - 1), bytess[count - 1], 0, bytes.Length % 50000);
        }

        return bytess;
    }
    public byte[] GetSpritBytes(byte[][] bytess, int count)
    {
        int k = 0;
        byte[] bytes = new byte[50000 * count - 1 + bytess[count - 1].Length];
        
        foreach(byte[] i in bytess)
        {
            for (int j = 0; j < i.Length; j++)
            {
                bytes[k] = i[j];
                k++;
            }
        }

        return bytes;
    }
    public object DeserializeSprite(byte[] bytes)
    {
        // PNG形式のバイト配列からTexture2Dを復元
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        // Texture2DをSpriteに変換
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        return sprite;
    }
}
