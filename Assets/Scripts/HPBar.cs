using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HPBar : MonoBehaviour
{

    [SerializeField] UnityEngine.UI.Slider hpSlider;
    [SerializeField] Text hpText;

    private int nowHp;
    private int slidervalue;

    // Start is called before the first frame update
    void Start()
    {
        Character character;
        character = GetComponentInParent<Transform>();
        //HPを初期化
        nowHp = character.H;

        //スライダーの現在値の設定
        hpSlider.value = nowHp;
    }

    public void ChangeHP(int damage){

        nowHp -= damage;
        hpSlider.value = nowHp;
    }
}
