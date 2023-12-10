using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu]
public class CardInInventory : ScriptableObject
{
    public List<CardStatus> cardStatusList = new List<CardStatus>();
}

[Serializable]
public class CardStatus
{
    public Sprite creature;
    public attribute attribute;
    public int hp;
    public int atk;
    public int magatk;
    public int def;
    public int magdef;
    public int speed;

    public CardStatus(Sprite sprite, attribute attribute, int hp, int atk, int magatk, int def, int magdef, int speed)
    {
        this.creature = sprite;
        this.attribute = attribute;
        this.hp = hp;
        this.atk = atk;
        this.magatk = magatk;
        this.def = def;
        this.magdef = magdef;
        this.speed = speed;
    }
}

public enum attribute
{
    fire,
    water,
    grass,
    light,
    dark
}
