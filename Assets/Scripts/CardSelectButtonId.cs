using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectButtonId : MonoBehaviour
{
    public int id;
    public CardInInventory cardInInventory;
    public void idUpdate()
    {
        cardInInventory.SelectCardId = id;
    }
}
