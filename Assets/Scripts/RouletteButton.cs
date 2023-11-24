using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RouletteButton : MonoBehaviour
{

    //CommandRouletteを呼び出し
    public CommandRoulette commandRoulette;

    //ボタンのテキスト
    public Text buttonText = null;

    //ボタンのスタート、ストップの切り替え管理用
    //true:STOP false:START
    private bool isOn = true;

    //ボタンのSourceImage
    public Sprite sprite1;
    public Sprite sprite2;

    // Start is called before the first frame update
    void Start()
    {
        // Spriteコンポーネントを設定
        GetComponent<UnityEngine.UI.Image>().sprite = sprite1;

        //初めはボタンの表記をSTARTにする
        buttonText.text = "START";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedButton()
    {
        if (isOn)
        {
            //ボタンがSTARTと表示されている時は押したらルーレットスタート
            //ボタンの表記をSTOPに切り替え,スプライトを切替える
            commandRoulette.StartRoulette();
            buttonText.text = "STOP";
            GetComponent<UnityEngine.UI.Image>().sprite = sprite2;


            isOn = false;
        }
        else
        {
            //ボタンがSTOPと表示されている時は押したらルーレットストップ
            //ボタンの表記をSTARTに切り替え,スプライトを切替える
            commandRoulette.StopRoulette();
            buttonText.text = "START";
            GetComponent<UnityEngine.UI.Image>().sprite = sprite1;

            isOn = true;
        }
    }
}
