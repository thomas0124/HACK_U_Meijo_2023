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
        List<int> skill = new List<int>() { 1, 1, 1 };
        cardChange.CardAdd(sprite, 1, 1, 1, 1, 1, 1, 1, skill);
    }
}
