using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MakeCard : MonoBehaviour
{
    public CardInInventory cardInInventory;
    public RawImage cardImage;

    public void makeCard()
    {
        var statusInfo = WebRequestManager.Instance.statusInfo;
        int type = 0;
        switch (statusInfo.type)
        {
            case "Red":
                type = 0;
                break;
            case "Blue":
                type = 1;
                break;
            case "Green":
                type = 2;
                break;
            case "Yellow":
                type = 3;
                break;
            case "Keyplate":
                type = 4;
                break;
        }
        UnityEngine.Random.InitState(DateTime.Now.Second);
        int[] skill = new int[] { UnityEngine.Random.Range(1, 41), UnityEngine.Random.Range(1, 41), UnityEngine.Random.Range(1, 44), UnityEngine.Random.Range(1, 41) };
        Texture2D texture2D = cardImage.texture as Texture2D;
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, cardImage.texture.width, cardImage.texture.height),new Vector2(0.5f,0.5f));
        cardInInventory.cardStatusList.Add(new CardStatus(sprite, type, statusInfo.hp, statusInfo.attack, statusInfo.specialAttack, statusInfo.defense, statusInfo.specialDefense, statusInfo.speed, skill));
    }
}
