using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private BattleController battle;

    //ルーレット用
    public UnityEngine.UI.Image[] commandlist;
    public int resultValue;//ルーレットの結果を保存
    private int lastTime;
    private float lottery;
    private bool isStart = false;//ルーレットスタート

    private bool justOnce = false;//一度だけ処理を行うためのフラグ
    private float countTime = 0f;
    private float fireTime = 0f;
    private bool isStop = false;//ルーレット停止
    private float speed = 0;
    
    private void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        if(isStart)
        {
            isStart = false;
            isStop = false;
            speed = 10;
            justOnce = true;
            countTime = 0f;
            fireTime = 0f;
        }
        
        //タイマーを使ってルーレットを回す
        //タイマーA
        countTime += Time.deltaTime * speed;
        if (countTime > commandlist.Length)
        {
            countTime = 0f;
        }
        //タイマーB
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
            lottery = 930 * 0.001f;
            speed *= lottery;
        }
        if (fireTime >= 2.0 && justOnce)
        {
            fireTime = 0;
            justOnce = false;

            // ここで他スクリプトに(int)countTimeを送信したい
            resultValue = (int)countTime;
            battle.SendResultValue(resultValue);

            commandlist[resultValue].color = new Color(1, 1, 1);

            //止まったところを点滅させるコルーチンの起動
            StartCoroutine(Blinking(commandlist[battle.roulettResult]));
        }
    }

//HPバー

    //自分のキャラクターのHPをHPバーに反映する
    public void ChangeHPSlider(int Ally_hp, int Enemy_hp, Slider hpSlider_A, Slider hpSlider_E, Text hpText_A)
    {
        hpSlider_A.value = Ally_hp;
        hpSlider_E.value = Enemy_hp;
        //テキストを変更
        hpText_A.text = "HP  " + Ally_hp.ToString();
    }

//パネル

    //パネルを非表示にする
    public void HiddenPanel(GameObject panel)
    {
        //Debug.Log("<HiddenPanel>");
        panel.SetActive(false);
    }

    //パネルを表示する
    public void DisplayPanel(GameObject panel)
    {
        //Debug.Log("<DisplayPanel>");
        panel.SetActive(true);
    }

//ルーレット
    //ルーレットを初期化
    public void OnStartButton()
    {
        Debug.Log("ルーレットスタート");
        isStart = true;
    }

    //ルーレットを止める
    public void OnStopButton()
    {
        Debug.Log("ルーレットストップ");
        isStop = true;
    }


    //コルーチン本体
    private IEnumerator Blinking(UnityEngine.UI.Image _image)
    {
        //Debug.Log("<IEnumerator Blinking>");
        // 点滅する
        for(int i = 0; i < 10; i++)
        {
            _image.color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.25f);

            _image.color = new Color(1, 0, 0);

            yield return new WaitForSeconds(0.25f);
        }
        yield break;
    }

    
}
