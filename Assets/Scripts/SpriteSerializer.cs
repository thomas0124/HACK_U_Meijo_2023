using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class SpriteSerializer
{
    public static void Register()
    {
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Sprite), (byte)'S', SerializeSprite, DeserializeSprite);
    }

    private static byte[] SerializeSprite(object customObject)
    {
        Sprite sprite = (Sprite)customObject;

        // バイト配列にシリアライズするためにTexture2Dを作成
        Texture2D texture = sprite.texture;

        // Texture2DをPNG形式のバイト配列に変換
        byte[] bytes = texture.EncodeToPNG();

        return bytes;
    }

    private static object DeserializeSprite(byte[] bytes)
    {
        // PNG形式のバイト配列からTexture2Dを復元
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        // Texture2DをSpriteに変換
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        return sprite;
    }
}