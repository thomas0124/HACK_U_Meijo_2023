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
    }

    //ルーレットを開始
    public void StartRoulette()
    {
        speed = 10;

        //OnRouletteでUpdateのオンオフを管理
        isOn_Roulette = true;
    }

    //ルーレットを停止
    public void StopRoulette()
    {
        //ルーレットを止めた時のリストの画像を点滅させる
        OnOff_target = commandlist[(int)countTime];
        speed = 0;
        isOn_Roulette = false;

        Debug.Log(OnOff_target);

        // コルーチンの起動
        StartCoroutine(Blinking());
    }

    // コルーチン本体
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
