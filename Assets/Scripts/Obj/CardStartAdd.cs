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
        //start�ȊO�Ŏg���Ƃ��͍X�V�K�{�܂��̓V�[�����[�h�ōX�V����ׂ�ContentCard
        cardChange.CardAdd(sprite, attribute.fire, 1, 1, 1, 1, 1, 1);
    }
}
