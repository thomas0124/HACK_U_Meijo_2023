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

        // �o�C�g�z��ɃV���A���C�Y���邽�߂�Texture2D���쐬
        Texture2D texture = sprite.texture;

        // Texture2D��PNG�`���̃o�C�g�z��ɕϊ�
        byte[] bytes = texture.EncodeToPNG();

        return bytes;
    }

    private static object DeserializeSprite(byte[] bytes)
    {
        // PNG�`���̃o�C�g�z�񂩂�Texture2D�𕜌�
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        // Texture2D��Sprite�ɕϊ�
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        return sprite;
    }
}