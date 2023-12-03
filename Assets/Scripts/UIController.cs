using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    //ルーレット用
    public UnityEngine.UI.Image[] commandlist;
    private float countTime;
    private int lastTime;
    private float fireTime;
    private float speed = 10;
    private float lottery;
    bool justOnce = true;//一度だけ処理を行うためのフラグ
    public bool isBlinking = false;//ルーレット点滅
    int resultValue;//ルーレットの結果を保存
    public bool isStop = false;//ルーレット停止
    
    void Start()
    {
        InitializeRoulett();//ルーレット初期化
    }
    
    public IEnumerator Roulett()
    {
        while(true)
        {
            //タイマーを使ってルーレットを回す
            // タイマーA
            countTime += Time.deltaTime * speed;
            if (countTime > commandlist.Length)
            {
                countTime = 0f;
            }
            // タイマーB
            fireTime += Time.deltaTime;
            if (lastTime != (int)countTime)
            {
                fireTime = 0f;
                foreach (var command in commandlist)
                {
                    command.color = new Color(1, 1, 1);
                }
                lastTime = (int)countTime;
                commandlist[(int)countTime].color = new Color(1, 0, 0);
            }

            //  isStopがtrueのときにルーレットを減速
            if (isStop)
            {
                lottery = Random.Range(990, 997) * 0.001f;
                speed *= lottery;
            }
            if (fireTime >= 2.5 && justOnce)
            {
                fireTime = 0;
                justOnce = false;
            }
            if (speed <= 0.05f && isBlinking == false)
            {
                isStop = false;
                isBlinking = true;
                speed = 0f;

                // ここで他スクリプトに(int)countTimeを送信 
                setValue((int)countTime);
                
                //止まったところを点滅させるコルーチンの起動
                StartCoroutine((Blinking(commandlist[(int)countTime])));
                
            }
        }
    }

//HPバー

    //自分のキャラクターのHPをHPバーに反映する
    public void ChangeAllySlider(int nowhp, Slider hpSlider_A, Text hpText_A)
    {
        Debug.Log("<ChangeAllySlider>");
        hpSlider_A.value = nowhp;
        //テキストを変更
        hpText_A.text = "HP  " + (char)nowhp;
    }

    //相手のキャラクターのHPをHPバーに反映する
    public void ChangeEnemySlider(int nowhp, Slider hpSlider_E)
    {
        Debug.Log("<ChangeEnemySlider>");
        hpSlider_E.value = nowhp;
    }

//パネル

    //パネルを非表示にする
    public void HiddenPanel(Canvas canvas)
    {
        Debug.Log("<HiddenPanel>");
        canvas.enabled = false;
    }

    //パネルを表示する
    public void DisplayPanel(Canvas canvas)
    {
        Debug.Log("<DisplayPanel>");
        canvas.enabled = true;
    }

//ルーレット

    public IEnumerator RunRoulett()
    {
        while(true)
        {
            StartCoroutine(Roulett());
            yield return null;
        }
    }

    //ボタンを押したらルーレットを止める
    public void OnClickedButton()
    {
        Debug.Log("<OnClickedButton>");
        isStop = true;
    }

    //ルーレットの結果（リスト番号:0-5）を保存する
    public void setValue(int value)
    {
        Debug.Log("<setValue>");
        resultValue = value;
    }

    //ルーレットの結果を他スクリプトに送信するための関数
    public int getValue()
    {
        Debug.Log("<getValue>");
        return this.resultValue;
    }


    //コルーチン本体
    private IEnumerator Blinking(UnityEngine.UI.Image _image)
    {
        Debug.Log("<IEnumerator Blinking>");
        // ３回点滅する
        while(isBlinking)
        {
            _image.color = new Color(1, 0, 0);

            yield return new WaitForSeconds(0.25f);

            _image.color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.25f);
        }
        yield break;
    }

    //共通の初期化処理をまとめたメソッド
    private void InitializeRoulett()
    {
        Debug.Log("<InitializeRoulett>");
        resultValue = 0;
        isStop = false;
        isBlinking = false;
        speed = 10;
        justOnce = true;
        countTime = 0f;
        fireTime = 0f;
    }
}
