using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentCard : MonoBehaviour
{
    public GameObject cardPrehub;
    public CardInInventory cardInInventory;
    private void Start()
    {
        CardUpdate();
    }
    public void CardUpdate()
    {
        foreach(Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < cardInInventory.cardStatusList.Count; i++)
        {
            GameObject obj = Instantiate(cardPrehub, Vector3.zero, Quaternion.identity, this.transform);
            obj.GetComponent<Image>().sprite = cardInInventory.cardStatusList[i].creature;
        }
    }
}
