using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoulettController : MonoBehaviour
{
    [SerializeField] Image[] commandlist;
    private float countTime;
    private int lastTime;
    private float fireTime;
    private float speed = 10;
    bool isStop = false;
    private float lottery;
    bool justOnce = true;
    bool isBlinking = false;
    int resultValue;//ルーレットの結果を保存

    private Character character;

    // Start is called before the first frame update
    void Start()
    {
        InitializeRoulett();
    }

    // Update is called once per frame
    void Update()
    {
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
        // isStopがtrueのときにルーレットを減速
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
            isBlinking = true;
            speed = 0f;

            // ここで他スクリプトに(int)countTimeを送信 
            setValue((int)countTime);

            // 止まったところを点滅させるコルーチンの起動
            StartCoroutine(Blinking(commandlist[(int)countTime]));
        }
    }

    public void StartRoulett()
    {
        isStop = false;
        InitializeRoulett();
    }

    public void StopRoulett()
    {
        isStop = true;
    }

    //ルーレットの結果（リスト番号:0-4）を保存する
    public void setValue(int value)
    {
        resultValue = value;
    }

    //ルーレットの結果を他スクリプトに送信するための関数
    public int getValue()
    {
        return resultValue;
    }



    // コルーチン本体
    private IEnumerator Blinking(Image _image)
    {
        // ルーレットが停止してる間点滅し続ける
        for(int i = 0; i < 3; i++)
        {
            _image.color = new Color(1, 0, 0);

            yield return new WaitForSeconds(0.25f);

            _image.color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.25f);
        }
    }

    // 共通の初期化処理をまとめたメソッド
    private void InitializeRoulett()
    {
        isBlinking = false;
        speed = 10;
        justOnce = true;
        countTime = 0f;
        fireTime = 0f;
    }
}
