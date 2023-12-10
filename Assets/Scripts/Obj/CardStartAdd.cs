using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStartAdd : MonoBehaviour
{
    private CardChange cardChange;
    public Sprite sprite;
    private void Awake()
    {
        cardChange = GetComponent<CardChange>();
    }
    void Start()
    {
        //start以外で使うときは更新必須またはシーンロードで更新するべきContentCard
        cardChange.CardAdd(sprite, attribute.fire, 1, 1, 1, 1, 1, 1);
    }
}
