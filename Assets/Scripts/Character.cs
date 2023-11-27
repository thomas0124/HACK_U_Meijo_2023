using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private HPBar hpBar;
    private Elements element;
    private RoulettController rourlett;
    private CommandRoulettePanel roulettPanel;

    public int H;//HP
    public int A;//攻撃力
    public int B;//防御
    public int C;//とくこう
    public int D;//とくぼう
    public int S;//すばやさ
    private int Element;//属性　：　0->火,　1->水,　2->草,　3->光,　4->闇
    private int Action;//ルーレットによって決まる行動の値 : 0->攻撃, 1->スキル1, 2->スキル２, 3->スキル３, 4->ミス
    private double Mag = 1;//スキル倍率(初期値１)
    private double damage = 0.5;//与ダメージ量(初期値はダメージ調整用の数)
    /*
    ダメージ計算式(int) : 0.5(調整用) * (A or C) / (B or D) * 属性相性(Compatibility) * ランダム数(0.85-1.00)
    A,Cは自身のステータス　　B,Dは相手のステータス
    */

    // Start is called before the first frame update
    void Start()
    {
        Element = element.getElements();//属性を取得
    }

    public int getAction()
    {
        //ルーレットのパネル表示,ルーレットスタート
        roulettPanel.DisplayPanel();
        Action = rourlett.getValue();
        //ルーレットのパネル非表示
        roulettPanel.HiddenPanel();

        return Action;
    }

    public void takeDamage(int d)
    {
        hpBar.ChangeHP(d);
    }

    //スキル１ スキル倍率1.2倍
    public void skil1()
    {
        Mag = 1.2;
    }

    //スキル2 スキル倍率0.8倍(防御を上げる)
    public void skil2()
    {
        Mag = 1.2;
    }
    //スキル3 スキル倍率1.3倍
    public void skil3()
    {
        Mag = 1.3;
    }
}
