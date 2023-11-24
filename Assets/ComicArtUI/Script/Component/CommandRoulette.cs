using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class CommandRoulette : MonoBehaviour
{
    //ルーレットの中身が入るリスト
    [SerializeField] Image[] commandlist;
    private float countTime;
    private int lastTime;
    private float speed = 10;
    private float lottery;


    //ルーレットが止まった時のアイテムを記憶
    private Image OnOff_target = null;

    //ルーレットの回転のオンオフ判別用
    private bool isOn_Roulette = false;

    // Start is called before the first frame update
    void Start()
    {
        //フレームレートを60に固定
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn_Roulette)
        {
            countTime += Time.deltaTime * speed;
            if (countTime > commandlist.Length)
            {
                countTime = 0f;
            }

            //ルーレットを回す処理
            if (lastTime != (int)countTime)
            {
                foreach (var command in commandlist)
                {
                    command.color = new Color(1, 1, 1);
                }
                lastTime = (int)countTime;
                commandlist[(int)countTime].color = new Color(1, 0, 0);
            }
        }
        else
        {
            lottery = Random.Range(990, 997) * 0.001f;
            speed *= lottery;
        }
    }

    //ルーレットを開始
    public void StartRoulette()
    {
        speed = 10;
        countTime = 0f;

        //OnRouletteでUpdateのオンオフを管理
        isOn_Roulette = true;
    }

    //ルーレットを停止
    public void StopRoulette()
    {

        //ルーレットを止めた時のリストの画像を記録
        OnOff_target = commandlist[(int)countTime];
        
        isOn_Roulette = false;

        Debug.Log(OnOff_target);

        if(speed <= 0)
        {
            // ルーレットを止めて点滅させるコルーチンの起動
            StartCoroutine(Blinking());
        }
    }

    // ルーレットを止めて点滅させるコルーチン本体
    private IEnumerator Blinking()
    {

        // ルーレットが停止してる間点滅し続ける
        while (!isOn_Roulette) 
        {
            OnOff_target.color = new Color(1, 0, 0);

            yield return new WaitForSeconds(0.25f);

            OnOff_target.color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
