using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChange : MonoBehaviour
{
    public CardInInventory cardInInventory;
    private CardStatus cardStatus;
    public void CardAdd(Sprite sprite,attribute attribute,int hp,int atk,int magatk,int def,int magdef,int speed)
    {
        cardStatus = new CardStatus(sprite, attribute, hp, atk, magatk, def, magdef, speed);
        cardInInventory.cardStatusList.Add(cardStatus);
    }
    public void CardRemove(int removeIndex)
    {
        cardInInventory.cardStatusList.RemoveAt(removeIndex);
    }
}
