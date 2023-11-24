using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] UnityEngine.UI.Slider hpSlider;
    //[SerializeField] Text hpText;

    private float maxHp = 100f;
    private float nowHp;
    private float slidervalue;

    // Start is called before the first frame update
    void Start()
    {
        hpSlider = GetComponent<UnityEngine.UI.Slider>();
        //hpText = GetComponent<Text>();

        //スライダーの最大値の設定
        hpSlider.maxValue = maxHp;

        //HPを初期化
        nowHp = maxHp;

        //スライダーの現在値の設定
        hpSlider.value = nowHp;
    }

    // Update is called once per frame
    void Update()
    {
        //slidervalue = hpSlider.value;
        //hpText.text = "HP  " + slidervalue;
    }

    public void OnChangeSlider()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            nowHp = nowHp - 10f;
            hpSlider.value = nowHp / maxHp;
        }
    }
}
